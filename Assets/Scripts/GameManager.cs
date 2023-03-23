using System.Collections;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public event Action<bool> GameStarted;    

    [Header("ScriptableObjects")]
    [SerializeField]
    public PlayerDataScriptableObject playerDataSO;
    [SerializeField]
    public WeaponDataScriptableObject weaponDataSO;
    [SerializeField]
    public AudioFilesScriptableObject audioFilesSO;

    [Space(10)]
    [SerializeField]
    private AsteroidManager asteroidManager;
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject gameHolder;

    [SerializeField]
    private int startingLives = 3;

    private int level = 1;

    bool isPlaying = false;

    public int Lives
    {
        get;
        private set;
    }

    public int Points
    {
        get;
        private set;
    }

    void Awake()
    {
        #region - Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        gameHolder.SetActive(false);

        asteroidManager.OnAsteroidDestroyed += OnLevelPoints;
        player.OnPlayerDied += OnLevelLives;
    }

    void Start()
    {
        isPlaying = false;
        startingLives = playerDataSO.playerLives;
        GameStarted?.Invoke(isPlaying);
    }

    private void FixedUpdate()
    {
        if (isPlaying) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPressed();
        }
    }

    private void StartPressed()
    {
        AudioManager.Instance.PlaySFX(audioFilesSO.startPressed);
        isPlaying = true;
        uiManager.ShowPlayUI(true);
        StartGame();
    }

    public void StartGame()
    {
        gameHolder.SetActive(true);
        ResetGame();
        asteroidManager.Spawn(level);
        SpawnPlayer();
        GameStarted?.Invoke(isPlaying);
    }

    public void ResetGame()
    {
        Points = 0;
        Lives = startingLives;

        level = 1;
        asteroidManager.Reset();
        uiManager.UpdateLives(Lives);
        uiManager.UpdateScore(Points);
    }

    private void OnLevelPoints(int points)
    {
        Points += points;
        uiManager.UpdateScore(Points);

        if (asteroidManager.AsteroidsRemaining == 0)
        {
            level += 1;
            StartCoroutine(StartNextLevel());
        }
    }

    private void OnLevelLives()
    {
        Lives -= 1;
        if (Lives > 0)
        {
            uiManager.UpdateLives(Lives);
            SpawnPlayer();
        }
        else
        {
            uiManager.GameEnded(Points);
            isPlaying = false;
            GameStarted?.Invoke(isPlaying);
        }
    }

    public void SpawnPlayer()
    {
        player.Spawn();
    }

    IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(1);
        asteroidManager.Spawn(level);
    }
}
