using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core.Audio;

namespace BP.Core
{
    [RequireComponent(typeof(RequireComponent))]
    public class CreditsSceneController : MonoBehaviour
    {
        [SerializeField] private bool m_playOnAwake = false;

        //[Header("next scene")]
        //[SerializeField] private SceneAsset m_menuSceneAsset = null;
        //[SerializeField] private SceneAssetGameEvent m_requestLoadActiveSceneEvent = null;

        [Header("scene elements")]
        [SerializeField] private AudioCue m_ambientPadsCue = null;
        [SerializeField] private BoolGameEvent m_showIntroUI = null;

        [SerializeField] private BoolGameEvent m_enabledInput = null;
        [SerializeField] private FloatVariable m_fadeTime = null;

        private RequestSceneTransition m_requestSceneTransition;

        private void Awake()
        {
            m_requestSceneTransition = GetComponent<RequestSceneTransition>();
        }

        private void Start()
        {
            if(m_playOnAwake) { StartIntroSequence(); }
        }

        public void StartIntroSequence()
        {
            StartCoroutine(IntroSequence());
        }

        private IEnumerator IntroSequence()
        {
            yield return new WaitForSeconds(0.2f);
            m_enabledInput.Raise(false);
            m_ambientPadsCue.Play();
            m_showIntroUI.Raise(true);
            yield return new WaitForSeconds(m_fadeTime.Value);
            yield return new WaitForSeconds(0.5f);
            m_showIntroUI.Raise(false);
            yield return new WaitForSeconds(m_fadeTime.Value);
            m_enabledInput.Raise(true);
            //m_requestLoadActiveSceneEvent.Raise(m_menuSceneAsset);
            m_requestSceneTransition.RequestTransition();
        }
    }
}

