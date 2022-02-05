using System.Collections;
using UnityEngine;
using BP.Core.Audio;

public class AudioTestSndtrkPlayer : MonoBehaviour
{
    [SerializeField] private AudioCue m_cue = null;
    [SerializeField] private float m_delay = 1f;
    private AudioSource m_audioSource;


    private void Start()
    {
        StartCoroutine(DelayPlay(m_delay));
    }

    private IEnumerator DelayPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_cue.Play();
    }
}
