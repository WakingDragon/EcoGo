using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BP.Core
{
    [CreateAssetMenu(fileName = "new_sceneTransition", menuName = "Core/Scenes/Scene Transition Asset")]
    public class SceneTransition : ScriptableObject
    {
        [SerializeField] private SceneAsset m_thisScene = null;
        [SerializeField] private SceneAsset m_newScene = null;

        //NOTE: if you unload and don't set the next one active you will have problems if there are no other active viewable cameras
        [SerializeField] private bool m_unloadThisScene = true;
        [SerializeField] private bool m_setNewSceneActive = true;

        public SceneAsset ThisScene() { return m_thisScene; }
        public SceneAsset NewScene() { return m_newScene; }
        public bool UnloadThisScene() { return m_unloadThisScene; }
        public bool SetNewSceneActive() { return m_setNewSceneActive; }
    }
}


