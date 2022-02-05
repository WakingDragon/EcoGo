using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    //TODO - deprecate?
    public class EyesAndEarsInterface : MonoBehaviour
    {
        [SerializeField] private EyesAndEarsLibrary m_eyesAndEars = null;

        public Camera GetActiveCam()
        {
            var cam = m_eyesAndEars.GetActiveCamera();
            if (cam)
            {
                return cam;
            }
            
            return Camera.main;
        }
    }
}

