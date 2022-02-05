using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BP.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        public static bool isEnabled  { get; private set; }
        [SerializeField] private AudioManagerAsset m_audioAsset;
        private AudioSourcePool m_srcPool;
        private Metronome m_metronome;
        [SerializeField] private List<VoiceItem> m_audioInTray = new List<VoiceItem>();
        private List<VoiceItem> m_intrayDeletions = new List<VoiceItem>();
        [SerializeField] private List<VoiceItem> m_audioNowPlaying = new List<VoiceItem>();
        private List<VoiceItem> m_playingDeletions = new List<VoiceItem>();
        private Dictionary<AudioTrack, AudioChannelParams> m_channels;
        private double m_refreshPlayingInterval = 0.5d;
        private double m_nextRefreshPlaying;
        private double m_dspTime;

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            AssembleAudioManager();
            SetupAudioManagerComponents();
            
            isEnabled = true;
        }

        private void Start()
        {
            m_metronome.Activate();
            m_nextRefreshPlaying = AudioSettings.dspTime + m_refreshPlayingInterval;
        }

        private void Update()
        {
            m_dspTime = AudioSettings.dspTime;  //could change this based on next review time in list with min length of 1 second for loopers

            if (m_dspTime >= m_nextRefreshPlaying)
            {
                RefreshNowPlaying();
                m_nextRefreshPlaying = m_dspTime + m_refreshPlayingInterval;
            }
        }

        #region assembly
        public Metronome Metronome() { return m_metronome; }
        private void AssembleAudioManager()
        {
            if (!m_audioAsset) { Debug.Log("no audio asset in audio manager"); }
            m_channels = m_audioAsset.GetChannelsDictionary();

            m_metronome = GetComponent<Metronome>();
            if (!m_metronome) { m_metronome = gameObject.AddComponent<Metronome>(); }

            m_srcPool = GetComponent<AudioSourcePool>();
            if (!m_srcPool) { m_srcPool = gameObject.AddComponent<AudioSourcePool>(); }
            m_srcPool.Assemble(20);
        }

        private void SetupAudioManagerComponents()
        {
            SetAllVolumesToMax();
            m_metronome.Setup(m_audioAsset, this);
        }
        #endregion

        #region volume levels, toggle, pitch
        private void SetAllVolumesToMax()
        {
            foreach(KeyValuePair<AudioTrack,AudioChannelParams> param in m_channels)
            {
                param.Value.VolumeFloatVar().Value = 0f;
            }
        }
        public void ToggleMasterVolume(bool on)
        {
            ToggleChannel(AudioTrack.Master, on);
        }
        public void ToggleSoundtrackVolume(bool on)
        {
            ToggleChannel(AudioTrack.Soundtrack, on);
        }
        private void ToggleChannel(AudioTrack channel, bool on)
        {
            if (on)
            {
                SetChannelVolume(channel, 0f);
            }
            else
            {
                SetChannelVolume(channel, -80f);
            }
        }
        public void SetMasterVolume(float level)
        {
            SetChannelVolume(AudioTrack.Master, level);
        }
        public void SetAmbienceVolume(float level)
        {
            SetChannelVolume(AudioTrack.Ambience, level);
        }
        public void SetSFXVolume(float level)
        {
            SetChannelVolume(AudioTrack.SFX, level);
        }
        public void SetSoundtrackVolume(float level)
        {
            SetChannelVolume(AudioTrack.Soundtrack, level);
        }
        public void SetUIVolume(float level)
        {
            SetChannelVolume(AudioTrack.UI, level);
        }
        public void SetChannelVolume(AudioTrack channel, float level)
        {
            level = Mathf.Clamp(level, -80f, 0f);
            m_channels[channel].MixerGroup().audioMixer.SetFloat(m_channels[channel].VolControlName(), level);
            m_channels[channel].VolumeFloatVar().Value = level;
        }
        #endregion

        #region play
        public void Play(AudioCue cue)
        {
            Play(cue, m_srcPool.GetSourceFromPool());
        }

        public void Play(AudioCue cue, AudioSource src)
        {
            VoiceItem v = new VoiceItem();
            v.cue = cue;
            v.track = cue.GetTrack();
            v.source = src;
            v.timing = cue.Timing();
            v.startTime = GetStartTime(cue);

            AddToAudioInTray(v);
        }

        private double GetStartTime(AudioCue cue)
        {
            if (!m_audioAsset.TimingWorks()) { return 0d; }

            switch (cue.Timing())
            {
                case AudioTiming.Immediate:
                    return 0d;
                case AudioTiming.OnBeat:
                    return m_metronome.NextBeatTime();
                case AudioTiming.OnBar:
                    return m_metronome.NextBarTime();
            }
            return 0d;
        }
        #endregion

        #region audio intray
        private void AddToAudioInTray(VoiceItem v)
        {
            m_audioInTray.Add(v);
            if (v.track == AudioTrack.Soundtrack)
            {
                Debug.Log("added cue:" + v.cue + " to start at " + v.startTime);
                Debug.Log(m_metronome.NextBarTime());
            }
            RefreshInTray();
        }

        private void RefreshInTray()
        {
            foreach(VoiceItem v in m_audioInTray)
            {                
                v.newEndTime = m_dspTime + v.cue.SetupSourceAndPlay(v.source, m_channels[v.track],v.startTime, m_audioAsset.TimingWorks());
                if (!m_audioNowPlaying.Contains(v)) { m_audioNowPlaying.Add(v); }
                if(!m_intrayDeletions.Contains(v)) { m_intrayDeletions.Add(v); }
            }
            
            foreach(VoiceItem d in m_intrayDeletions)
            {
                if (m_audioInTray.Contains(d)) { m_audioInTray.Remove(d); }
            }
            m_intrayDeletions.Clear();
        }
        #endregion

        #region playinglist
        private void RefreshNowPlaying()
        {
            foreach (VoiceItem v in m_audioNowPlaying)
            {
                bool removeVoice = false;
                if(v.source == null || !v.source.enabled)
                {
                    removeVoice = true;
                }
                else
                {
                    if(!v.cue.Loop() && v.newEndTime < m_dspTime)
                    {
                        removeVoice = true;
                        v.source.enabled = false;
                    }
                }
                
                if(removeVoice && !m_playingDeletions.Contains(v)) 
                { 
                    m_playingDeletions.Add(v); 
                }
            }

            foreach (VoiceItem d in m_playingDeletions)
            {
                if (m_audioNowPlaying.Contains(d)) { m_audioNowPlaying.Remove(d); }
            }
            m_playingDeletions.Clear();
        }
        #endregion

        #region  onbeat and onbar functions
        public void OnBeat()
        {
            Debug.Log("beat");
        }
        public void OnBar()
        {
            Debug.Log("bar");
        }
        #endregion

        #region  metronome
        public void MetronomeOnBeat()
        {
            //Debug.Log("beat");
        }
        public void MetronomeOnBar()
        {
            //Debug.Log("bar");
        }
        #endregion
    }
}