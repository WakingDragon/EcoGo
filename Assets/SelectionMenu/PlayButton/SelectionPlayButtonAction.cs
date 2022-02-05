using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using BP.Core.Audio;

namespace BP.SelectionMenu
{
    [RequireComponent(typeof(RequestSceneTransition))]
    public class SelectionPlayButtonAction : MonoBehaviour
    {
        [SerializeField] private AudioCue m_btnClickSFX = null;
        [SerializeField] private float m_sfxLength = 1f;
        private RequestSceneTransition m_requestSceneTransition;

        private void Awake()
        {
            m_requestSceneTransition = GetComponent<RequestSceneTransition>();
        }

        public void OnButtonClick()
        {
            StartCoroutine(DoButtonClick());
        }

        private IEnumerator DoButtonClick()
        {
            m_btnClickSFX.Play();
            yield return new WaitForSeconds(m_sfxLength);
            m_requestSceneTransition.RequestTransition();
        }
    }
}
