using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public event System.Action<int> OnAsteroidDestroyed;
    public static event System.Action OnAstriodHitPlayer;

    public int AsteroidsRemaining
    {
        get { return asteroids; }
    }

    [SerializeField]
    private AsteroidPool asteroidPool;

    [SerializeField]
    private float offscreenPadding;

    [SerializeField]
    private int startingAsteroidCount = 1;

    private int asteroids;

    private void Start()
    {
        asteroidPool.InitializePool();
    }

    public void Spawn(int level)
    {
        int numAsteroids = startingAsteroidCount + level;
        for (int i = 0; i < numAsteroids; i++)
        {
            Vector3 pos = GetOffScreenPosition(); Quaternion rot = GetOffScreenRotation();
            CreateAsteroid(pos, rot, Random.Range(1, 4));
        }
    }

    public void Reset()
    {
        asteroidPool.ReleaseAll();
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

    private Asteroid CreateAsteroid(Vector3 position, Quaternion rotation, int category = 1)
    {
        Asteroid asteroid = GetAsteroidFromPool(category);
        Transform asteroidTranform = asteroid.gameObject.transform;
        asteroidTranform.position = position;
        asteroidTranform.rotation = rotation;
        asteroidTranform.SetParent(gameObject.transform);
        //Change the scale of the asteroid based on the category.
        asteroidTranform.localScale = category / 3.0f * Vector3.one;
        return asteroid;
    }

    private void OnAsteroidDie(Asteroid asteroid, int points, Vector3 position, int asteroidCategory, bool didHitPlayer)
    {
        ReturnAsteroidToPool(asteroid);
        OnAsteroidDestroyed?.Invoke(points);

        if (didHitPlayer)
        {
            OnAstriodHitPlayer?.Invoke();
            return;
        }

        // create children asteroids
        for (int i = 1; i < asteroidCategory; i++)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Floor(Random.Range(0.0f, 360.0f))));
            CreateAsteroid(position, rotation, (asteroidCategory - 1));
        }
    }

    Asteroid GetAsteroidFromPool(int asteroidLevel)
    {
        GameObject asteroidGO = asteroidPool.GetGameObject();
        Asteroid asteroid = asteroidGO.GetComponent<Asteroid>();
        //Determines the speed AND health.[TODO] Change it when adding improvements
        asteroid.asteroidLevel = asteroidLevel;
        asteroid.EventDie += OnAsteroidDie;
        asteroids++;
        return asteroid;
    }

    void ReturnAsteroidToPool(Asteroid asteroid)
    {
        asteroids--;
        asteroidPool.ReleaseObject(asteroid.gameObject);
        asteroid.EventDie -= OnAsteroidDie;
    }
}
