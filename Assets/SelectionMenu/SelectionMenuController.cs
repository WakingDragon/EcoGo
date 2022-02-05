using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.SelectionMenu
{
    public class SelectionMenuController : MonoBehaviour
    {
        [SerializeField] private SelectionLibraryAsset m_selectionLib = null;
        [SerializeField] private GameObject m_canvasPrefab = null;
        private SelectionCanvas m_canvas;
        [SerializeField] private bool m_activateOnStart = false;

        private void Start()
        {
            if (m_activateOnStart) { SetupMenu(); }
        }

        public void SetupMenu()
        {
            SetupCanvasGO();
            CreateMenu();
        }

        private void SetupCanvasGO()
        {
            var go = Instantiate(m_canvasPrefab, Vector3.zero, Quaternion.identity);
            m_canvas = go.GetComponent<SelectionCanvas>();
            go.name = "SelectionCanvas";

            m_canvas.SetLibrary(m_selectionLib);
        }

        private void CreateMenu()
        {
            m_canvas.BuildMenu();
        }
    }
}


