using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int numOfBulletsInPool = 10;

    [SerializeField]
    private GameObject bulletsPoolParent;

    private List<GameObject> pool;
    public bool isInitialised;

    public void Initialize()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("[ObjectPool] 'bulletPrefab' has not been set.", this);
        }

        pool = new List<GameObject>(numOfBulletsInPool);
        for (int i = 0; i < numOfBulletsInPool; i++)
        {
            AddGameObject();
        }
        isInitialised = true;
    }

    public GameObject GetGameObject()
    {
        if (!isInitialised)
        {
            Debug.LogError("[ObjectPool] has been initialised. Call 'Init' first.", this);
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
        GameObject go = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(bulletsPoolParent.transform, true);
        go.SetActive(false);
        pool.Add(go);
        return go;
    }
}
