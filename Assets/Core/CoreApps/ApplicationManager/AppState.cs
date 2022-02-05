using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    [RequireComponent(typeof(RequestSceneTransition))]
    public class AppState : MonoBehaviour
    {
        private enum AppCurrentState
        {
            initialising,
            ready,
            live
        }

        [Header("prefabs")]
        [SerializeField] private GameObject m_sceneLoaderPrefab = null;
        [SerializeField] private GameObject m_userInputPrefab = null;
        [SerializeField] private GameObject m_audioPrefab = null;

        private RequestSceneTransition m_requestSceneTransition;
        private AppCurrentState m_state;

        private void Awake()
        {
            m_state = AppCurrentState.initialising;
            m_requestSceneTransition = GetComponent<RequestSceneTransition>();
        }

        private void Start()
        {
            LoadCoreSystems();
            StartCoroutine(LoadIntro());
        }

        private void LoadCoreSystems()
        {
            Instantiate(m_audioPrefab);
            Instantiate(m_userInputPrefab);
            var go = Instantiate(m_sceneLoaderPrefab);
            m_state = AppCurrentState.ready;
        }

        private IEnumerator LoadIntro()
        {
            while (m_state != AppCurrentState.ready) { yield return null; }
            m_requestSceneTransition.RequestTransition();
            yield return new WaitForSeconds(0.1f);
            m_state = AppCurrentState.live;
        }
    }
}

