using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class RequestSceneTransition : MonoBehaviour
    {
        [SerializeField] private SceneTransition m_transitionAsset = null;
        [SerializeField] private SceneTransitionGameEvent m_notifyLoader = null;

        public void RequestTransition()
        {
            m_notifyLoader.Raise(m_transitionAsset);
        }
    }
}

