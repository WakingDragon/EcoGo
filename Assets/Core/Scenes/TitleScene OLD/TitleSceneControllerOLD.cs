using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core.Audio;

namespace BP.Core
    {
    public class TitleSceneControllerOLD : MonoBehaviour
    {
        [Header("settings")]
        [SerializeField] private bool m_playOnAwake = false;
        [SerializeField] private BoolGameEvent m_toggleMainMenu = null;
        [SerializeField] private FloatVariable m_fadeInTime = null;
        [SerializeField] private FloatVariable m_fadeOutTime = null;

        [Header("events")]
        [SerializeField] private BoolGameEvent m_toggleSplashEvent = null;
        [SerializeField] private BoolGameEvent m_toggleIntroEvent = null;
        [SerializeField] private BoolGameEvent m_enabledInput = null;
        [SerializeField] private SceneAsset m_playBtnTargetScene = null;
        [SerializeField] private SceneAssetGameEvent m_loadNewActiveSceneEvent = null;

        [Header("music")]
        [SerializeField] private AudioCue m_ambientPadsCue = null;
        [SerializeField] private AudioCue m_menuRhythmCue = null;
        [SerializeField] private AudioCue m_menuCricketsCue = null;

        private enum MenuSceneState { inactive, splash, intro, menu }
        private MenuSceneState m_state = MenuSceneState.inactive;

        private void Start()
        {
            if (m_playOnAwake) { Activate(); }
        }

        public void Activate()
        {
            StartCoroutine(DoSplash(true));

            if (m_ambientPadsCue) { StartCoroutine(AudioAmbience()); }
            if (m_menuRhythmCue) { m_menuRhythmCue.Play(); }
            if (m_menuCricketsCue) { m_menuCricketsCue.Play(); }
        }

        public void RespondToInput()
        {
            if (m_state == MenuSceneState.splash)
            {
                StartCoroutine(DoIntro(true));
            }
            else if (m_state == MenuSceneState.intro)
            {
                StartCoroutine(ShowMenu());
            }
            
        }


        private IEnumerator DoSplash(bool show)
        {
            if (show)
            {
                m_enabledInput.Raise(false);
                m_toggleSplashEvent.Raise(true);

                yield return new WaitForSeconds(m_fadeInTime.Value);

                m_state = MenuSceneState.splash;
                m_enabledInput.Raise(true);
            }
            else
            {
                m_toggleSplashEvent.Raise(false);
                yield return new WaitForSeconds(m_fadeOutTime.Value);
            }
        }

        private IEnumerator DoIntro(bool show)
        {
            m_enabledInput.Raise(false);
            yield return StartCoroutine(DoSplash(false));

            if (show)
            {
                m_toggleIntroEvent.Raise(true);
                yield return new WaitForSeconds(m_fadeInTime.Value);
                m_state = MenuSceneState.intro;
            }
            else
            {
                m_toggleIntroEvent.Raise(false);
                yield return new WaitForSeconds(m_fadeOutTime.Value);
            }
            m_enabledInput.Raise(true);
        }

        private IEnumerator ShowMenu()
        {
            //m_state = MenuSceneState.menu;
            m_enabledInput.Raise(false);
            //yield return new WaitForSeconds(m_fadeInTime.Value);
            yield return StartCoroutine(DoIntro(false));

            yield return new WaitForSeconds(m_fadeInTime.Value);
            m_toggleMainMenu.Raise(true);
            m_state = MenuSceneState.menu;
            m_enabledInput.Raise(true);
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

        public void OnPlayButtonClicked()
        {
            m_toggleSplashEvent.Raise(false);
            m_toggleIntroEvent.Raise(false);
            m_toggleMainMenu.Raise(false);
            m_loadNewActiveSceneEvent.Raise(m_playBtnTargetScene);
            m_state = MenuSceneState.inactive;
        }
    }
}

