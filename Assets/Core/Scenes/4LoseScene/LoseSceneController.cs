using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core.Audio;

namespace BP.Core
{
    [RequireComponent(typeof(RequestSceneTransition))]
    public class LoseSceneController : MonoBehaviour
    {
        [SerializeField] private AudioCue m_sfx = null;
        [SerializeField] private float m_interactionDelay = 1f;
        private bool m_blockInterations = true;
        private RequestSceneTransition m_sceneTransition;

        private void Awake()
        {
            m_sceneTransition = GetComponent<RequestSceneTransition>();
            m_blockInterations = true;
        }

        public void PlayLose()
        {
            m_sfx.Play();
            StartCoroutine(DelayInteractability());
        }

        public void OnAnyKeyPressed()
        {
            if(!m_blockInterations)
            {
                m_sceneTransition.RequestTransition();
            }
        }

        private IEnumerator DelayInteractability()
        {
            yield return new WaitForSeconds(m_interactionDelay);
            m_blockInterations = false;
        }
    }
}

