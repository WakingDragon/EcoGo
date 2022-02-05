using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core.Audio
{
    [System.Serializable]   //TODO remove from serialization to improve performance
    public class VoiceItem
    {
        public AudioClip clip;
        public AudioTrack track;
        public AudioSource source;
        public bool loop;
        public float length;
        public float timeEnds;
        public int priority = 5;
        public bool isPlaying = false;
        public bool isGarbage = false;
        public AudioCue cue;
        public AudioTiming timing;
        public double startTime;
        public double newLength;
        public double newEndTime;
    }

    public enum AudioTrack { Master, SFX, Soundtrack, UI, Ambience }

    public enum AudioPlayType { Play, PlayLooped, PlayOneShot }

    public enum AudioTiming { Immediate, OnBeat, OnBar }

    public static class AudioStatics
    {
        public static SoundtrackTrack[] SoundtrackTracks()
        {
            var x = new SoundtrackTrack[6];
            x[0] = SoundtrackTrack.Drums;
            x[1] = SoundtrackTrack.Percussion;
            x[2] = SoundtrackTrack.Bass;
            x[3] = SoundtrackTrack.Pads;
            x[4] = SoundtrackTrack.Melody;
            x[5] = SoundtrackTrack.AmbientOneShots;
            return x;
        }

        public static int VoicesByTrack(AudioTrack track)
        {
            switch (track)
            {
                case AudioTrack.SFX:
                    return 16;
                case AudioTrack.Soundtrack:
                    return 6;
                case AudioTrack.UI:
                    return 6;
                case AudioTrack.Ambience:
                    return 4;
                default:
                    return 1;
            }
        }
    }
}