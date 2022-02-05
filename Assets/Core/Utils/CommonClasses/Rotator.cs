using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] bool m_rotateLocally = false;
        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        public void SetRotationsPerMin(Vector3 rotsPerMin)
        {
            xRotationsPerMinute = rotsPerMin.x;
            yRotationsPerMinute = rotsPerMin.y;
            zRotationsPerMinute = rotsPerMin.z;
        }

        private void Update()
        {
            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;

            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute;

            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;

            if(m_rotateLocally)
            {
                
                transform.localRotation = transform.localRotation * Quaternion.Euler(xDegreesPerFrame,yDegreesPerFrame,zDegreesPerFrame);
            }
            else
            {
                transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);
                transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);
                transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
            }
        }
    }
}
