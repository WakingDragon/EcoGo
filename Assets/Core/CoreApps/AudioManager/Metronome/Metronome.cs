using UnityEngine;

namespace BP.Core.Audio
{
    public class Metronome : MonoBehaviour
    {
        private bool m_timingWorks;
        private bool m_isAudible = false;
        private AudioCue m_ticker;
        private double m_bpm = 120d;
        private int m_beatsPerBar = 4;
        private bool m_metronomeRunning;
        private int m_beat;
        private double m_beatInterval;
        private double m_timeForNextBeat;
        private double m_timeForNextBar;
        private AudioManager m_manager;

        private void Update()
        {
            if(!m_timingWorks || !m_metronomeRunning) { return; }

            if(AudioSettings.dspTime >= m_timeForNextBeat)
            {
                if (m_beat == 0)
                {
                    m_manager.MetronomeOnBar();
                }
                else
                {
                    m_manager.MetronomeOnBeat();
                }
                Beat();
            }
        }

        public void Setup(AudioManagerAsset asset, AudioManager manager)
        {
            m_manager = manager;
            m_timingWorks = asset.TimingWorks();
            m_isAudible = asset.TickerIsAudible();
            m_ticker = asset.TickerCue();
            m_bpm = asset.BPM();
            m_beatsPerBar = asset.BeatsPerBar();
        }

        public void Activate()
        {
            m_metronomeRunning = true;
            m_beatInterval = 60d / m_bpm;
            RestartMetronome();
        }

        public double NextBeatTime() { return m_timeForNextBeat; }
        public double NextBarTime() { return m_timeForNextBar; }

        private void Beat()
        {
            m_beat = Utils.Mod(m_beat + 1, m_beatsPerBar);
            m_timeForNextBeat = AudioSettings.dspTime + m_beatInterval;
            m_timeForNextBar = TimeForNextbar(m_timeForNextBeat, m_beat);
            if (m_isAudible) { m_ticker.Play(); }
        }

        private double TimeForNextbar(double timeForNextBeat, int nextBeat)
        {
            return timeForNextBeat + m_beatInterval * (m_beatsPerBar - nextBeat);
        }

        public void RestartMetronome()
        {
            m_timeForNextBeat = AudioSettings.dspTime + 0.5d;
            m_timeForNextBar = TimeForNextbar(m_timeForNextBeat, 0);
            m_beat = 3;
        }
    }
}

