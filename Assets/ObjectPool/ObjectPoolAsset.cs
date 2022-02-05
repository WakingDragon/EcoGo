using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BP.ObjectPooling
{
    [CreateAssetMenu(fileName ="objectPoolAsset",menuName ="ObjectPooling/(single) Object Pool")]
    public class ObjectPoolAsset : ScriptableObject
    {
        private Dictionary<IPoolableAsset, List<GameObject>> m_poolMaster = new Dictionary<IPoolableAsset, List<GameObject>>();
        private Transform m_poolFolder;

        #region pool folder
        private void CreatePoolFolder()
        {
            var folder = new GameObject();
            folder.name = "ObjectPoolFolder";
            var pool = folder.AddComponent<ObjectPool>();

            m_poolFolder = folder.transform;
            m_poolFolder.position = Vector3.zero;
            m_poolFolder.rotation = Quaternion.identity;
        }
        #endregion

        #region creating a new pool
        public void TryCreateNewPool(IPoolableAsset poolableAsset)
        {
            if (!m_poolFolder) { CreatePoolFolder(); }

            if (m_poolMaster.ContainsKey(poolableAsset)) { return; }

            var pool = new List<GameObject>();
            m_poolMaster.Add(poolableAsset, pool);
            PopulateNewPool(poolableAsset,pool);
        }

        private void PopulateNewPool(IPoolableAsset poolableAsset, List<GameObject> pool)
        {
            for (int i = 0; i < poolableAsset.NumberToPool(); i++)
            {
                AddNewGameObjectToPool(poolableAsset, pool);
            }
        }
        #endregion

        #region add objects to pool
        private GameObject AddNewGameObjectToPool(IPoolableAsset poolableAsset, List<GameObject> pool)
        {
            GameObject go = poolableAsset.InstantiatePrefabToPool(m_poolFolder.transform);
            ObjectRepool repooler = go.AddComponent<ObjectRepool>();
            repooler.Initialise(poolableAsset, this);
            go.SetActive(false);

            int prefix = pool.Count + 1;
            go.name = prefix + " " + poolableAsset.ToString();

            if (go)
            {
                pool.Add(go);
            }
            return go;
        }
        #endregion

        #region get object from pool
        public GameObject GetObjectFromPool(IPoolableAsset asset, Vector3 position, Quaternion rotation)
        {
            if (position == null) { position = Vector3.zero; }
            if (rotation == null) { rotation = Quaternion.identity; }

            if (!m_poolMaster.ContainsKey(asset))
            {
                TryCreateNewPool(asset);
            }

            var objectPool = m_poolMaster[asset];

            var go = FindInactiveObjectInPool(objectPool);

            if (go == null)
            {
                go = AddNewGameObjectToPool(asset, objectPool);
                go.SetActive(true);
            }

            if (go != null)
            {
                go.transform.position = position;
                go.transform.rotation = rotation;
                ObjectRepool repoolComponent = go.GetComponent<ObjectRepool>();
                repoolComponent.Activate();
            }
            return go;
        }

        private GameObject FindInactiveObjectInPool(List<GameObject> pool)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i] && !pool[i].activeSelf)
                {
                    GameObject obj = pool[i].gameObject;
                    obj.SetActive(true);
                    return obj;
                }
            }
            return null;
        }
        #endregion

        #region repool
        public void Repool(GameObject go)
        {
            go.transform.parent = null;
            go.SetActive(false);
            go.transform.parent = m_poolFolder.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }
        #endregion

        #region delete pool
        public void DeletePool(IPoolableAsset asset)
        {
            if(m_poolMaster.ContainsKey(asset))
            {
                foreach (var item in m_poolMaster[asset])
                {
                    Destroy(item);
                }
                m_poolMaster.Remove(asset);
            }
        }
        #endregion
    }
}

