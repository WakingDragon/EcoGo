using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Core/Generic Poolable Asset", fileName ="generic_asset")]
public class GenericPoolableAsset : ScriptableObject, IPoolableAsset
{
    [SerializeField] private GameObject m_prefab = null;

    [Header("Pooling settings")]
    [SerializeField] private int m_numberToPool = 5;
    [SerializeField] private float m_autoRepoolLife = 2f;

    public IPoolableAsset GetIPoolableAsset() { return this; }
    public int NumberToPool() { return m_numberToPool; }

    public GameObject InstantiatePrefabToPool(Transform parent)
    {
        GameObject go = Instantiate(m_prefab, parent);
        return go;
    }

    public float AutoRepool()
    {
        return m_autoRepoolLife;
    }
}
