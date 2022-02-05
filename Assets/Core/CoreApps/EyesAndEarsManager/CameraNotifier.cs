using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class CameraNotifier : MonoBehaviour
    {
        [SerializeField] private EyesAndEarsLibrary m_camLibrary = null;
        private Camera m_cam;

        private void OnEnable()
        {
            if(!m_camLibrary) 
            { Debug.Log("eyes and ears library missing from " + gameObject.name); }

            m_cam = GetComponent<Camera>();
            if (!m_cam) { Debug.Log("MainCamera attached to go without camera"); }

            m_camLibrary.AddCamera(m_cam);
        }
    }
}

