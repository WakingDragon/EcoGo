using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BP.Core
{
    [CreateAssetMenu(fileName ="userInputLibrary",menuName ="Core/Input/(single) User Input Library")]
    public class UserInputAsset : ScriptableObject
    {
        [SerializeField] private bool m_inputEnabled = true;

        [Header("anykey")]
        [SerializeField] private VoidGameEvent m_anyKeyEvent = null;

        [Header("axes")]
        [SerializeField] private float m_axisSensitivity = 0.01f;
        [SerializeField] private FloatGameEvent m_xAxisEvent = null;
        [SerializeField] private FloatGameEvent m_yAxisEvent = null;
        private float m_lastX, m_lastY;

        [Header("fire and jump")]
        [SerializeField] private BoolGameEvent m_fire = null;
        [SerializeField] private VoidGameEvent m_fireBtnDownOnly = null;
        [SerializeField] private BoolGameEvent m_jump = null;
        [SerializeField] private VoidGameEvent m_specialPower = null;

        [Header("keyboard keys")]
        [SerializeField] private VoidGameEvent m_PkeyDown = null;
        [SerializeField] private VoidGameEvent m_ESCkeyDown = null;

        public void InputEnabled(bool inputEnabled) { m_inputEnabled = inputEnabled; }

        public void AnyKeyDown(InputAction.CallbackContext context)
        {
            if (m_inputEnabled && context.started)
            {
                m_anyKeyEvent.Raise();
            }
        }

        public void XAxisInput(InputAction.CallbackContext context)
        {
            if (!m_inputEnabled) { return; }

            if (context.started)
            {
                var x = context.ReadValue<float>();

                if (Mathf.Abs(x) > m_axisSensitivity)
                {
                    m_xAxisEvent.Raise(x);
                }
            }

            if (context.canceled)
            {
                var x = context.ReadValue<float>();

                if (Mathf.Abs(x) < m_axisSensitivity)
                {
                    m_xAxisEvent.Raise(0f);
                }
            }
        }

        public void YAxisInput(InputAction.CallbackContext context)
        {
            if (!m_inputEnabled) { return; }

            if (context.started)
            {
                var y = context.ReadValue<float>();

                if (Mathf.Abs(y) > m_axisSensitivity)
                {
                    m_yAxisEvent.Raise(y);
                }
            }

            if (context.canceled)
            {
                var y = context.ReadValue<float>();

                if (Mathf.Abs(y) < m_axisSensitivity)
                {
                    m_yAxisEvent.Raise(0f);
                }
            }

            //var yAxis = Input.GetAxis("Vertical");

            //if (Mathf.Abs(yAxis) > m_axisSensitivity)
            //{
            //    m_yAxisEvent.Raise(yAxis);
            //    m_lastY = yAxis;
            //}
            //else if (Mathf.Abs(m_lastY) > m_axisSensitivity)
            //{
            //    m_yAxisEvent.Raise(0f);
            //    m_lastY = 0f;
            //}
        }

        public void Fire1Input(InputAction.CallbackContext context)
        {
            if(!m_inputEnabled) { return; }

            if(context.started)
            {
                m_fire.Raise(true);
                m_fireBtnDownOnly.Raise();
            }

            if (context.canceled)
            {
               m_fire.Raise(false);
            }
        }

        public void SpecialPowerInput(InputAction.CallbackContext context)
        {
            if (m_inputEnabled && context.started)
            {
                m_specialPower.Raise();
            }
        }

        public void JumpInput(InputAction.CallbackContext context)
        {
            if (!m_inputEnabled) { return; }

            if (context.started)
            {
                m_jump.Raise(true);
            }

            if(context.canceled)
            {
                m_jump.Raise(false);
            }

            //if (Input.GetButtonDown("Jump"))
            //{
            //    m_jump.Raise(true);
            //}
            //if (Input.GetButtonUp("Jump"))
            //{
            //    m_jump.Raise(false); 
            //}
        }

        public void PauseInput(InputAction.CallbackContext context)
        {
            if (!m_inputEnabled) { return; }

            if (context.started)
            {
                m_PkeyDown.Raise();
            }
        }

        public void QuitInput(InputAction.CallbackContext context)
        {
            if (!m_inputEnabled) { return; }

            if (context.started)
            {
                m_ESCkeyDown.Raise();
            }
        }
    }
}

