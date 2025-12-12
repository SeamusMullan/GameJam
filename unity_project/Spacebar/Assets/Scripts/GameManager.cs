using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int score = 0;
    [SerializeField] private int tips = 0;
    [SerializeField] private bool isGameActive = false;

    [Header("Wave Settings")]
    [SerializeField] private float waveTimer = 60f;
    [SerializeField] private float currentWaveTime = 0f;
    [SerializeField] private int customersPerWave = 3;
    [SerializeField] private float customersPerWaveIncrease = 1.5f;

    [Header("Events")]
    public UnityEvent<int> OnWaveStart;
    public UnityEvent<int> OnWaveEnd;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<int> OnTipsChanged;
    public UnityEvent OnGameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        currentWaveTime += Time.deltaTime;
    }

    public void StartGame()
    {
        isGameActive = true;
        currentWave = 0;
        score = 0;
        tips = 0;
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;
        currentWaveTime = 0f;
        int customersThisWave = Mathf.RoundToInt(customersPerWave * Mathf.Pow(customersPerWaveIncrease, currentWave - 1));

        OnWaveStart?.Invoke(currentWave);

        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.SpawnWave(customersThisWave);
        }
    }

    public void EndWave()
    {
        OnWaveEnd?.Invoke(currentWave);
    }

    public void AddScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(score);
    }

    public void AddTips(int amount)
    {
        tips += amount;
        OnTipsChanged?.Invoke(tips);
    }

    public void GameOver()
    {
        isGameActive = false;
        OnGameOver?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public int GetCurrentWave() => currentWave;
    public int GetScore() => score;
    public int GetTips() => tips;
    public float GetWaveTime() => currentWaveTime;
    public bool IsGameActive() => isGameActive;
}
