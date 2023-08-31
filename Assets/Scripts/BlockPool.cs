using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField][Min(0)] private uint minBlockCount;
    [SerializeField] private GameObject blockPrefab;
    [HideInInspector] private List<PoolAwareBehaviour> poolAwareObjects = new List<PoolAwareBehaviour>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < minBlockCount; i++)
            poolAwareObjects.Add(AddObjectToPool());
    }

    private PoolAwareBehaviour AddObjectToPool()
    {
        GameObject obj = Instantiate(blockPrefab);
        PoolAwareBehaviour pool = obj.GetComponent<PoolAwareBehaviour>();
        if(pool == null)
        {
            pool = new PoolAwareBehaviour();
            obj.AddComponent<PoolAwareBehaviour>();
        }
        return pool;
    }

    public GameObject GetBlock()
    {
        foreach (PoolAwareBehaviour obj in poolAwareObjects)
            if (!obj.isLive) {
                obj.Alive();
                return obj.gameObject;
            }
        PoolAwareBehaviour obj1 = AddObjectToPool();
        obj1.Alive();
        return obj1.gameObject;
    }

    public T GetBlock<T>()
    {
        foreach (PoolAwareBehaviour obj in poolAwareObjects)
            if (!obj.isLive)
            {
                obj.Alive();
                return obj.GetComponent<T>();
            }
        PoolAwareBehaviour obj1 = AddObjectToPool();
        obj1.Alive();
        return obj1.GetComponent<T>();
    }
}
