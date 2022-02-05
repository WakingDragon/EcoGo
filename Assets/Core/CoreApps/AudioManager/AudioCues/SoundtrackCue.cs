using UnityEngine;

namespace BP.Core.Audio
{
    [CreateAssetMenu(fileName = "newSondtrackCue", menuName = "Audio/Soundtrack Cue")]
    public class SoundtrackCue : ScriptableObject
    {
        private AudioTrack m_track = AudioTrack.Soundtrack;
        [SerializeField] private AudioClip[] m_clips = null;

        [Header("randomisation")]
        [SerializeField] private float m_volume = 1f;
        [SerializeField] private float m_pitch = 1f;

        [Header("playback")]
        [SerializeField] private bool loop = false;
        [SerializeField] private bool m_oneShot = true;
        [SerializeField] private AudioTiming m_audioTiming = AudioTiming.Immediate;
        [SerializeField] [Range(0, 1)] private float spatialBlend = 0f;

        [Header("priority: SFX only, 256=lowest priority")]
        [SerializeField] [Range(128, 256)] private int m_priority = 128;

        [Header("soundtrack only")]
        [SerializeField] private SoundtrackTrack m_soundtrackType;

        #region get/set
        public void SetLooping(bool isLooping) { loop = isLooping; }
        public bool Loop() { return loop; }
        public AudioTrack GetTrack() { return m_track; }
        public AudioTiming Timing() { return m_audioTiming; }
        public SoundtrackTrack SoundtrackTrack() { return m_soundtrackType; }
        #endregion

        #region play
        public void Play()
        {
            if (AudioManager.isEnabled) { AudioManager.instance.Play(this); }
        }

        public void Play(AudioSource src)
        {
            if (AudioManager.isEnabled) { AudioManager.instance.Play(this, src); }
        }

        public double SetupSourceAndPlay(AudioSource src, AudioChannelParams channel, double startTime, bool useTiming)
        {
            src.enabled = true;

            var _pitch = Pitch();
            var _clip = SelectClip();

            src.playOnAwake = false;
            src.loop = loop;
            src.volume = Volume();
            src.pitch = _pitch;
            src.priority = Priority();
            src.clip = _clip;
            src.spatialBlend = SpatialBlend();
            src.outputAudioMixerGroup = channel.MixerGroup();

            //scheduling
            if(m_audioTiming == AudioTiming.Immediate || !useTiming)
            {
                if (m_oneShot)
                {
                    src.PlayOneShot(_clip);
                }
                else
                {
                    src.Play();
                }
            }
            else
            {
                src.PlayScheduled(startTime);
            }
           
            //return length of sample at correct pitch
            return _clip.length / _pitch;
        }

        public AudioClip SelectClip()
        {
            return m_clips[Random.Range(0, m_clips.Length)];
        }

        private int Priority()
        {
            int p = 128;
            switch (m_track)
            {
                case AudioTrack.SFX:
                    p = m_priority;
                    break;
                case AudioTrack.Soundtrack:
                    p = 30;
                    break;
                case AudioTrack.UI:
                    p = 10;
                    break;
                case AudioTrack.Ambience:
                    p = 100;
                    break;
            }
            return p;
        }

        private float SpatialBlend()
        {
            float b = 0f;
            switch (m_track)
            {
                case AudioTrack.SFX:
                    b = spatialBlend;
                    break;
                case AudioTrack.Soundtrack:
                    b = 0f;
                    break;
                case AudioTrack.UI:
                    b = 0f;
                    break;
                case AudioTrack.Ambience:
                    b = spatialBlend;
                    break;
            }
            return b;
        }

        private float Volume()
        {
            if (randomizeVolPitch)
            {
                return Random.Range(minVolume, m_volume);
            }
            return m_volume;
        }

        private float Pitch()
        {
            if (randomizeVolPitch)
            {
                return Random.Range(minPitch, m_pitch);
            }
            return m_pitch;
        }
        #endregion
    }
}