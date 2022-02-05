using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class TitleSplashUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_group = null;
        [SerializeField] private FloatVariable m_fadeInTime = null;
        [SerializeField] private FloatVariable m_fadeOutTime = null;

        private void Awake()
        {
            if(!m_group) { Debug.Log("no canvas group on " + gameObject.name); }
            if (!m_fadeInTime || !m_fadeOutTime) { Debug.Log("no duration floatvar on " + gameObject.name); }
            m_group.gameObject.SetActive(false);
        }

        public void ToggleSplash(bool show)
        {
            if(show)
            {
                StartCoroutine(ShowSplash());
            }
            else
            {
                StartCoroutine(HideSplash());
            }
        }

        private IEnumerator ShowSplash()
        {
            m_group.gameObject.SetActive(true);
            m_group.alpha = 0f;
            while(m_group.alpha <= 0.99f)
            {
                m_group.alpha += (1 / m_fadeInTime.Value) * Time.deltaTime;
                yield return null;
            }
            m_group.alpha = 1f;
        }
        private IEnumerator HideSplash()
        {
            m_group.alpha = 1f;
            while (m_group.alpha >= 0.01f)
            {
                m_group.alpha -= (1 / m_fadeOutTime.Value) * Time.deltaTime;
                yield return null;
            }
            m_group.alpha = 0f;
            m_group.gameObject.SetActive(false);
        }
    }
}
