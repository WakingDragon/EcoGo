//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;
using BP.Core.Audio;

namespace BP.Core
{
    [CreateAssetMenu(fileName = "new_FXAsset", menuName = "FX/FX Asset")]
    public class FXAsset : ScriptableObject, IPoolableAsset
    {
        [SerializeField] private GameObject m_prefab = null;

        [Header("sfx")]
        [SerializeField] private AudioCue onStartSFX = null;
        [SerializeField] private bool onStartSFXIsLocal = false;

        [Header("Pooling settings")]
        [SerializeField] private int m_defaultNumberToPool = 5;
        [SerializeField] private float m_autoRepoolLife = 2f;
        private ObjectPoolAsset m_pool;

        #region FX
        public void Play(Vector3 worldPos, ObjectPoolAsset pool)
        {
            GameObject go = GenerateFXGO(worldPos, pool);
            if (!go) { Debug.Log("no go created "); return; }
            PlayOnStartSFX(go);
        }

        public void Play(Transform p, Vector3 localPos, ObjectPoolAsset pool)
        {
            GameObject go = GenerateFXGO(p.position + localPos, pool);
            if(!go) { Debug.Log("no go created "); return; }
            go.transform.parent = p;
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.Euler(p.forward);
            PlayOnStartSFX(go);
        }

        private GameObject GenerateFXGO(Vector3 worldPos, ObjectPoolAsset pool)
        {
            GameObject go;

            if (!pool)
            {
                go = CreateGO();
                go.transform.position = worldPos;

                if(m_autoRepoolLife > 0f)
                {
                    go.AddComponent<AutoDestroy>().SetLifespan(m_autoRepoolLife);
                }
            }
            else
            {
                go = pool.GetObjectFromPool(this, worldPos, Quaternion.identity);
            }

            return go;
        }

        private GameObject CreateGO()
        {
            if (m_prefab)
            {
                return Instantiate(m_prefab);
            }
            else
            {
                return new GameObject("fxGO");
            }
        }

        private void PlayOnStartSFX(GameObject go)
        {
            if(onStartSFX)
            { 
                if (onStartSFXIsLocal)
                {
                    var src = go.GetComponent<AudioSource>();
                    if (!src) { src = go.AddComponent<AudioSource>(); }
                    onStartSFX.Play(src);
                }
                else
                {
                    onStartSFX.Play();
                }
            }
        }
        #endregion

        #region IPoolable
        public IPoolableAsset GetIPoolableAsset() { return this; }
        public int NumberToPool() { return m_defaultNumberToPool; }

        public GameObject InstantiatePrefabToPool(Transform parent)
        {
            GameObject go = CreateGO();
            go.transform.parent = parent;
            return go;
        }

        public float AutoRepool() { return m_autoRepoolLife; }
        #endregion

    }
}

