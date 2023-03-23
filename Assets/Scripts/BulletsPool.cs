using UnityEngine;

public class BulletsPool : ObjectPool
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int numOfBulletsInPool = 10;

    [SerializeField]
    private GameObject bulletsPoolParent;

    public void InitializePool()
    {
        objectPrefab = bulletPrefab;
        initialObjectCount = numOfBulletsInPool;
        poolParent = bulletsPoolParent;
        Initialize();
    }
}
