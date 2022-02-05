using System.Collections;
using UnityEngine;
using BP.Core.Audio;

public class AudioTestSFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioCue m_cue = null;
    [SerializeField] private float m_delay = 1f;
    private AudioSource m_audioSource;


    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        StartCoroutine(InfinitePlayLoop());
    }

    private IEnumerator InfinitePlayLoop()
    {
        WaitForSeconds waitTime = new WaitForSeconds(m_delay);
        while(true)
        {
            if (m_audioSource)
            {
                m_cue.Play(m_audioSource);
            }
            else
            {
                m_cue.Play();
            }
            yield return waitTime;
        }
    }
}
