using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class EventTogglePSPlay : MonoBehaviour
    {
        [SerializeField] private ParticleSystem psToToggle = null;

        private void Awake()
        {
            if(!psToToggle) { Debug.Log("need a PS to toggle: " + gameObject.name); }
        }

        public void ToggleActiveStatus(bool play)
        {
            if(!psToToggle) { return; }

            if(play)
            {
                psToToggle.Play();
            }
            else
            {
                psToToggle.Stop();
            }
        }

        //public void MakeGameobjectActive()
        //{
        //    if (!psToToggle) { return; }

        //    psToToggle.SetActive(true);
        //}

        //public void MakeGameobjectInactive()
        //{
        //    if (!psToToggle) { return; }

        //    psToToggle.SetActive(false);
        //}
    }
}

