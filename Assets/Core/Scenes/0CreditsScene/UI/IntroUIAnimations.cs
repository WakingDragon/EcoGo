using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class IntroUIAnimations : MonoBehaviour
    {
        [SerializeField] private FloatVariable m_fadeTime = null;
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup.gameObject.SetActive(false);
        }

        public void OnNotifyShowIntroCanvas(bool enterCanvas)
        {
            if(enterCanvas)
            {
                StartCoroutine(EnterIntro());
            }
            else
            {
                StartCoroutine(ExitIntro());   
            }
        }

        private IEnumerator EnterIntro()
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0f;

            while(canvasGroup.alpha <= 0.99f)
            {
                canvasGroup.alpha += (1/m_fadeTime.Value) * Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private IEnumerator ExitIntro()
        {
            canvasGroup.alpha = 1f;

            while (canvasGroup.alpha >= 0.01f)
            {
                canvasGroup.alpha -= (1 / m_fadeTime.Value) * Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}


