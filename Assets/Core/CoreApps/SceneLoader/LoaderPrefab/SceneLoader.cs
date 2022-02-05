using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BP.Core
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("dependencies")]
        [SerializeField] private ScenesLibraryAsset m_sceneLibrary = null;
        [SerializeField] private BoolGameEvent m_pausedForLoadingEvent = null;

        private SceneAsset m_currentMainScene;  //scene that will be swapped out, tends to have main cam

        private void Awake()
        {
            if(!m_sceneLibrary) { Debug.Log("scene loader missing assets"); }
            m_sceneLibrary.Initialise();
        }

        public void ProcessTransitionRequest(SceneTransition transition)
        {
            //if unload && setactive > transition
            //if !unload && setactive > LoadAdditive
            //if !unload && !setactive > LoadNewNonFocus
            if(transition.UnloadThisScene() && transition.SetNewSceneActive())
            {
                StartCoroutine(TransitionBetweenScenes(transition.NewScene(), m_currentMainScene));
            }
            else if(!transition.UnloadThisScene() && transition.SetNewSceneActive())
            {
                StartCoroutine(LoadAdditive(transition.NewScene(), true, true));
            }
            else if (!transition.UnloadThisScene() && !transition.SetNewSceneActive())
            {
                StartCoroutine(LoadAdditive(transition.NewScene(), true, false));
            }
        }

        #region loading/unloading
        public void LoadNewActiveScene(SceneAsset scene)
        {
            StartCoroutine(TransitionBetweenScenes(scene, m_currentMainScene));
        }

        public void LoadNewNonfocusScene(SceneAsset scene)
        {
            StartCoroutine(LoadAdditive(scene, true, false));
        }

        private IEnumerator TransitionBetweenScenes(SceneAsset newScene, SceneAsset oldScene)
        {
            yield return StartCoroutine(LoadAdditive(newScene, true, true));
            while (!newScene.IsLoaded())
            {
                yield return null;
            }
            m_currentMainScene = newScene;
            UnloadScene(oldScene);
        }

        public IEnumerator LoadAdditive(SceneAsset scene, bool setActiveOnLoad, bool setAsFocusScene)
        {
            m_sceneLibrary.AddSceneToLibrary(scene);

            m_pausedForLoadingEvent.Raise(true);

            AsyncOperation asyncOp;

            asyncOp = SceneManager.LoadSceneAsync(scene.SceneName(), LoadSceneMode.Additive);

            asyncOp.allowSceneActivation = false;

            yield return MonitorLoadingOfScene(asyncOp);    //change status @90%

            asyncOp.allowSceneActivation = setActiveOnLoad;

            yield return CheckIfLoadIsDone(asyncOp);

            if (setAsFocusScene) { m_currentMainScene = scene; }

            //update the scene asset
            m_sceneLibrary.SetAsLoaded(scene);
            var sceneRef = SceneManager.GetSceneByName(scene.SceneName());
            scene.SetSceneRef(sceneRef);
            if(setActiveOnLoad)
            {
                m_sceneLibrary.SetActiveScene(scene);
            }
            if(setAsFocusScene)
            {
                SceneManager.SetActiveScene(sceneRef);
            }
            scene.OnLoad();

            m_pausedForLoadingEvent.Raise(false);
        }

        private IEnumerator MonitorLoadingOfScene(AsyncOperation asyncScene)
        {
            while (asyncScene.progress < 0.9f)
            {
                yield return null;
            }
        }

        private IEnumerator CheckIfLoadIsDone(AsyncOperation asyncScene)
        {
            while (!asyncScene.isDone)
            {
                yield return null;
            }
        }

        public void UnloadScene(SceneAsset scene)
        {
            scene.OnUnload();
            SceneManager.UnloadSceneAsync(scene.SceneName());
            scene.SetActive(false);
            m_sceneLibrary.SetAsUnloaded(scene);
        }
        #endregion 
    }
}

