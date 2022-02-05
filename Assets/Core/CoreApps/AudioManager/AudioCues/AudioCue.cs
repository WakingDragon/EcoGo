using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core.Audio
{
    [CreateAssetMenu(fileName = "AudioCue", menuName = "Audio/Audio Cue")]
    public class AudioCue : ScriptableObject
    {
        [SerializeField] private AudioTrack track = AudioTrack.SFX;
        [SerializeField] private AudioClip[] clips = null;

        [Header("randomisation")]
        [SerializeField] private bool randomizeVolPitch = false;    //if false then play max pitch and vol
        [SerializeField] private float minVolume = 0f;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float minPitch = 0f;
        [SerializeField] private float maxPitch = 1f;

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
        public AudioTrack GetTrack() { return track; }
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
            return clips[Random.Range(0, clips.Length)];
        }

        private int Priority()
        {
            int p = 128;
            switch (track)
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
            switch (track)
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
                return Random.Range(minVolume, maxVolume);
            }
            return maxVolume;
        }

        private float Pitch()
        {
            if (randomizeVolPitch)
            {
                return Random.Range(minPitch, maxPitch);
            }
            return maxPitch;
        }
        #endregion
    }
}