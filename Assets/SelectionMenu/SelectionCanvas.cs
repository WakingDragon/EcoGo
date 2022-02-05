using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BP.SelectionMenu
{
    public class SelectionCanvas : MonoBehaviour
    {
        [SerializeField] private string m_title = "Item Menu Title";
        [SerializeField] private TextMeshProUGUI m_titleComponent = null;
        private string m_selectionText;
        [SerializeField] private TextMeshProUGUI m_selectionTextComponent = null;
        [SerializeField] private GameObject m_contentFitter = null;
        [SerializeField] private GameObject m_buttonPrefab = null;
        private SelectionLibraryAsset m_selectionLib;
        private List<SelectionButtonController> m_buttons = new List<SelectionButtonController>();

        public void SetLibrary(SelectionLibraryAsset newLib)
        {
            m_buttons.Clear();
            m_selectionLib = newLib;
            m_selectionLib.SetDefaultAsSelectedItem();
        }

        public void BuildMenu()
        {
            SetTitle();
            SetSelectionText("Selection: " + m_selectionLib.SelectedItem().Name());

            var lib = m_selectionLib.Library();
            for(int i = 0; i < lib.Length; i++)
            {
                SetupButton(lib[i]);
            }
        }

        private void SetTitle()
        {
            m_titleComponent.text = m_title;
        }

        public void UpdateSelectionTextWithSelection()
        {
            SetSelectionText("Selection: " + m_selectionLib.SelectedItem().Name());
        }

        private void SetSelectionText(string text)
        {
            m_selectionTextComponent.text = text;
        }

        private void SetupButton(SelectableItemAsset item)
        {
            var controller = CreateButton(item);
            m_buttons.Add(controller);
            controller.SetupButton(m_selectionLib, item);
        }

        private SelectionButtonController CreateButton(SelectableItemAsset item)
        {
            var go = Instantiate(m_buttonPrefab, m_contentFitter.transform);
            go.name = item.Name();
            var controller = go.GetComponent<SelectionButtonController>();
            return controller;
        }
    }
}
