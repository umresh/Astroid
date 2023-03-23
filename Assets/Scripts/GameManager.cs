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

    public event Action OnFireWeapon;

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
    bool enableInputs = false;

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
        enableInputs = true;
    }

    private void Update()
    {
        if (!enableInputs) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (isPlaying)
                OnFireWeapon?.Invoke();
            else
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
    }

    public void ResetGame()
    {
        Points = 0;
        Lives = startingLives;

        level = 1;
        asteroidManager.Reset();
        ResetUI();
    }
    private void ResetUI()
    {
        uiManager.UpdateLives(Lives);
        uiManager.UpdateScore(Points);
        UpdateLevel();
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
            AudioManager.Instance.PlaySFX(audioFilesSO.gameEnded);
            uiManager.GameEnded(Points);
            isPlaying = false;
            enableInputs = false;
            StartCoroutine(EnableInputs());
        }
    }

    public void SpawnPlayer()
    {
        player.Spawn();
    }

    private void UpdateLevel()
    {
        uiManager.UpdateLevels(level);
    }

    IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(1);
        UpdateLevel();
        asteroidManager.Spawn(level);
    }

    IEnumerator EnableInputs()
    {
        yield return new WaitForSeconds(2);
        enableInputs = true;
    }
}
