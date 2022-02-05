using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core.Audio;

namespace BP.Core
{
    [RequireComponent(typeof(RequestSceneTransition))]
    public class TitleSceneController : MonoBehaviour
    {
        [Header("settings")]
        [SerializeField] private float m_splashDuration = 2f;
        [SerializeField] private FloatVariable m_fadeInTime = null;
        [SerializeField] private FloatVariable m_fadeOutTime = null;

        [Header("events")]
        [SerializeField] private BoolGameEvent m_toggleSplashEvent = null;
        [SerializeField] private BoolGameEvent m_enabledInput = null;

        [Header("music")]
        [SerializeField] private AudioCue m_ambientPadsCue = null;
        [SerializeField] private AudioCue m_menuRhythmCue = null;
        [SerializeField] private AudioCue m_menuCricketsCue = null;

        private RequestSceneTransition m_requestSceneTransition;

        private void Awake()
        {
            m_requestSceneTransition = GetComponent<RequestSceneTransition>();
        }

        public void Activate()  //called by listener
        {
            StartCoroutine(DoSplash());

            if (m_ambientPadsCue) { StartCoroutine(AudioAmbience()); }
            if (m_menuRhythmCue) { m_menuRhythmCue.Play(); }
            if (m_menuCricketsCue) { m_menuCricketsCue.Play(); }
        }

        private IEnumerator DoSplash()
        {
            m_enabledInput.Raise(false);
            m_toggleSplashEvent.Raise(true);
            yield return new WaitForSeconds(Mathf.Abs(m_splashDuration + m_fadeInTime.Value));

            m_toggleSplashEvent.Raise(false);
            yield return new WaitForSeconds(m_fadeOutTime.Value + m_splashDuration);
            m_requestSceneTransition.RequestTransition();
        }

        private IEnumerator AudioAmbience()
        {
            while(true)
            {
                m_ambientPadsCue.Play();
                var t = Random.Range(10f, 20f);
                yield return new WaitForSecondsRealtime(t);
            }
        }
    }
}

