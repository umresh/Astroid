using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public event System.Action<int> OnAsteroidDestroyed;
    public static event System.Action OnAstriodHitPlayer;


    public int AsteroidsRemaining
    {
        get { return asteroids.Count; }
    }

    [SerializeField]
    private GameObject asteroidPrefab;

    [SerializeField]
    private float offscreenPadding;

    [SerializeField]
    private int startingAsteroidCount = 1;

    public List<Asteroid> asteroids;

    void Awake()
    {
        Reset();
    }

    // private void Start()
    // {
    //     Spawn(1);
    // }

    public void Spawn(int level)
    {
        int numAsteroids = startingAsteroidCount + level;
        for (int i = 0; i < numAsteroids; i++)
        {
            Vector3 pos = GetOffScreenPosition(); Quaternion rot = GetOffScreenRotation();
            CreateAsteroid(asteroidPrefab, pos, rot, Random.Range(1, 4));
        }
    }

    public void Reset()
    {
        if (asteroids != null)
        {
            for (int i = 0; i < asteroids.Count; i++)
            {
                Destroy(asteroids[i].gameObject);
            }
        }
        asteroids = new List<Asteroid>();
    }

    private Vector3 GetOffScreenPosition()
    {
        float posX = 0.0f;
        float posY = 0.0f;
        int startingSide = Random.Range(0, 4);
        switch (startingSide)
        {
            // top
            case 0:
                posX = Random.value;
                posY = 0.0f;
                posY -= offscreenPadding;
                break;
            // bottom
            case 1:
                posX = Random.value;
                posY = 1.0f;
                posY += offscreenPadding;
                break;
            // left
            case 2:
                posX = 0.0f;
                posY = Random.value;
                posX -= offscreenPadding;
                break;
            // right
            case 3:
                posX = 1.0f;
                posY = Random.value;
                posX += offscreenPadding;
                break;
        }
        return Camera.main.ViewportToWorldPoint(new Vector3(posX, posY, 1.0f));
    }

    private Quaternion GetOffScreenRotation()
    {
        int angle = 0;
        int startingSide = Random.Range(0, 4);
        switch (startingSide)
        {
            case 0:
                angle = Random.Range(20, 70);
                break;
            case 1:
                angle = -Random.Range(20, 70);
                break;
            case 2:
                angle = Random.Range(110, 160);
                break;
            case 3:
                angle = -Random.Range(110, 160);
                break;
        }
        return Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }

    private Asteroid CreateAsteroid(GameObject prefab, Vector3 position, Quaternion rotation, int category = 1)
    {
        GameObject asteroidGO = Instantiate(prefab, position, rotation) as GameObject;
        asteroidGO.transform.SetParent(gameObject.transform);
        asteroidGO.transform.localScale = category / 3.0f * Vector3.one;

        Asteroid asteroid = asteroidGO.GetComponent<Asteroid>();
        asteroid.asteroidLevel = category;
        asteroid.EventDie += OnAsteroidDie;
        asteroid.OnPlayerHit += OnPlayerHit;

        asteroids.Add(asteroid);
        return asteroid;
    }

    private void OnPlayerHit(Asteroid asteroid)
    {
        asteroids.Remove(asteroid);
        OnAstriodHitPlayer?.Invoke();
    }

    private void OnAsteroidDie(Asteroid asteroid, int points, Vector3 position, int asteroidCategory)
    {
        asteroids.Remove(asteroid);

        // create children asteroids
        for (int i = 1; i < asteroidCategory; i++)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Floor(Random.Range(0.0f, 360.0f))));
            CreateAsteroid(asteroidPrefab, position, rotation, (asteroidCategory - 1));
        }

        // dispatch event
        if (OnAsteroidDestroyed != null)
        {
            OnAsteroidDestroyed(points);
        }
    }


}
