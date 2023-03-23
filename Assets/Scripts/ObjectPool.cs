using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    protected GameObject objectPrefab;

    protected int initialObjectCount = 10;

    protected GameObject poolParent;

    private List<GameObject> pool;
    public bool isInitialised;

    protected void Initialize()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("[ObjectPool] 'object prefab' has not been set.", this);
        }

        pool = new List<GameObject>(initialObjectCount);
        for (int i = 0; i < initialObjectCount; i++)
        {
            AddGameObject();
        }
        isInitialised = true;
    }

    public GameObject GetGameObject()
    {
        if (!isInitialised)
        {
            Debug.LogError("[ObjectPool] has been initialised. Call 'Initialize' first.", this);
            return null;
        }

        for (int i = 0; i < pool.Count; i++)
        {
            GameObject ob = pool[i];
            if (!ob.activeSelf)
            {
                ob.transform.Translate(Vector3.zero);
                ob.transform.rotation = Quaternion.identity;
                ob.SetActive(true);
                return ob;
            }
        }

        GameObject additionalGO = AddGameObject();
        additionalGO.SetActive(true);
        return additionalGO;
    }

    public void ReleaseObject(GameObject go)
    {
        go.SetActive(false);
    }

    public void ReleaseAll()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject ob = pool[i];
            ob.SetActive(false);
        }
    }

    private GameObject AddGameObject()
    {
        GameObject go = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(poolParent.transform, true);
        go.SetActive(false);
        pool.Add(go);
        return go;
    }
}
