using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BP.Core.Audio
{
    [CreateAssetMenu(fileName ="new_audioChannel",menuName ="Audio/Channel Params")]
    public class AudioChannelParams : ScriptableObject
    {
        [SerializeField] private AudioTrack m_track;
        [SerializeField] private string m_mixerGroupName = "";   //must be the string in the mixerGroup
        [SerializeField] private AudioMixerGroup m_mixerGroup = null;
        [SerializeField] private string m_volControlString = "";
        [SerializeField] private FloatVariable m_volumeFloatVar = null;

        #region get/set
        public AudioTrack Track() { return m_track; }
        public string MixerGroupName() { return m_mixerGroupName; }
        public AudioMixerGroup MixerGroup() { return m_mixerGroup; }
        public string VolControlName() { return m_volControlString; }
        public FloatVariable VolumeFloatVar() { return m_volumeFloatVar; }
        #endregion
    }
}

