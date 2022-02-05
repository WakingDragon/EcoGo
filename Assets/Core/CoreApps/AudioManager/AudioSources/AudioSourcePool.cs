using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class AudioSourcePool : MonoBehaviour
    {
        public List<AudioSource> m_pool = new List<AudioSource>();
        private GameObject m_poolGO;

        public void Assemble(int poolSize)
        {
            m_poolGO = new GameObject();
            m_poolGO.transform.parent = transform;
            m_poolGO.name = "AudioSourcePool";

            for (int i = 0; i < poolSize; i++)
            {
                var src = AddNewSourceToPool();
                src.playOnAwake = false;
                src.enabled = false;
            }
        }

        private AudioSource AddNewSourceToPool()
        {
            var src = m_poolGO.AddComponent<AudioSource>();
            if (!m_pool.Contains(src)) { m_pool.Add(src); }
            return src;
        }

        public AudioSource GetSourceFromPool()
        {
            for(int i = 0; i<m_pool.Count;i++)
            {
                if(!m_pool[i].enabled)
                {
                    return m_pool[i];
                }
            }
            return AddNewSourceToPool();
        }

        public void ReturnSourceToPool(AudioSource src)
        {
            src.enabled = false;       //shouldn't be needed as any disabled src is available
        }
    }
}


