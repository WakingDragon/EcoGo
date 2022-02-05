using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BP.Core.Audio
{
    [CreateAssetMenu(fileName = "audioManagerAsset", menuName = "Audio/(single) Audio Manager")]
    public class AudioManagerAsset : ScriptableObject
    {
        [Header("metronome")]
        [SerializeField] private bool m_timingWorks = false;
        [SerializeField] private bool m_audibleTicker = false;
        [SerializeField] private AudioCue m_tickerSFX = null;
        [SerializeField] private double m_bpm = 120d;
        [SerializeField] [Range(1, 6)] private int m_beatsPerBar = 4;

        [Header("channels")]
        [SerializeField] private AudioChannelParams[] m_channelParams = null;

        [Header("volume variables")]
        [SerializeField] private FloatVariable masterVolVar = null;
        [SerializeField] private FloatVariable soundtrackVolVar = null;

        #region assembly and setup
        public Dictionary<AudioTrack, AudioChannelParams> GetChannelsDictionary()
        {
            Dictionary<AudioTrack, AudioChannelParams> dict = new Dictionary<AudioTrack, AudioChannelParams>();

            for(int i = 0; i< m_channelParams.Length; i++)
            {
                if (!dict.ContainsKey(m_channelParams[i].Track()))
                {
                    dict.Add(m_channelParams[i].Track(), m_channelParams[i]);
                }
            }
            return dict;
        }
        public bool TimingWorks() { return m_timingWorks; }
        public bool TickerIsAudible() { return m_audibleTicker; }
        public AudioCue TickerCue() { return m_tickerSFX; }
        public double BPM() { return m_bpm; }
        public int BeatsPerBar() { return m_beatsPerBar; }
        #endregion

    }
}

