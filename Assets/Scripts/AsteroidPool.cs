using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : ObjectPool
{
    [SerializeField]
    private GameObject asteroidPrefab;

    [SerializeField]
    private int numOfAsteroidsInPool = 10;

    [SerializeField]
    private GameObject asteroidsPoolParent;

    public void InitializePool()
    {
        objectPrefab = asteroidPrefab;
        initialObjectCount = numOfAsteroidsInPool;
        poolParent = asteroidsPoolParent;
        Initialize();
    }
}
