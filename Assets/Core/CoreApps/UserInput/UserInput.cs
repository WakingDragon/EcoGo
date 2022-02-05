﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BP.Core
{
    public class UserInput : MonoBehaviour
    {
        [Header("dependencies")]
        [SerializeField] private UserInputAsset m_inputAsset = null;

        private void Awake()
        {
            if (!m_inputAsset) { Debug.Log("User input library not attached"); }
        }

        public void ToggleInputEnabled(bool isInputEnabled)
        {
            m_inputAsset.InputEnabled(isInputEnabled);
        }
    }
}
