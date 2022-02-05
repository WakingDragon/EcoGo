using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BP.Core.Audio;

namespace BP.SelectionMenu
{
    public class SelectionButtonController : MonoBehaviour
    {
        [SerializeField] private Image m_imageComponent = null;
        [SerializeField] private TextMeshProUGUI m_textComponent = null;
        [SerializeField] private AudioCue m_btnClickSFX = null;
        [SerializeField] private Sprite m_lockedImg = null;
        [SerializeField] private string m_lockedText = "Locked!";
        private SelectableItemAsset m_itemAsset;
        private SelectionLibraryAsset m_selectionLib;

        #region setup
        public void SetupButton(SelectionLibraryAsset lib, SelectableItemAsset asset)
        {
            m_itemAsset = asset;
            m_selectionLib = lib;

            SetImageAndName();
            //TODO - if no unlocked then disable colour changes
        }

        private void SetImageAndName()
        {
            if(m_itemAsset.IsUnlocked())
            {
                m_imageComponent.sprite = m_itemAsset.MenuImage();
                m_textComponent.text = m_itemAsset.Name();
            }
            else
            {
                m_imageComponent.sprite = m_lockedImg;
                m_textComponent.text = m_lockedText;
            }
        }
        #endregion

        public void OnButtonClick()
        {
            if (m_itemAsset.IsUnlocked())
            {
                m_btnClickSFX.Play();
                m_selectionLib.SetSelectedItem(m_itemAsset);
            }
            else
            {
                //TODO ugly beeping noise if incorrectly clicked
            }
        }
    }
}
