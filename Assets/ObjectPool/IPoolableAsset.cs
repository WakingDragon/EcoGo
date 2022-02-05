using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableAsset
{
    GameObject InstantiatePrefabToPool(Transform parent);
    IPoolableAsset GetIPoolableAsset();
    int NumberToPool();
    float AutoRepool();
}
