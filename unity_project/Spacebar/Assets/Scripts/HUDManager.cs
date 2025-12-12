using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged.AddListener(UpdateScore);
            GameManager.Instance.OnTipsChanged.AddListener(UpdateTips);
            GameManager.Instance.OnWaveStart.AddListener(UpdateWave);
        }
    }

    void Update()
    {
        UpdateTimer();
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void UpdateTips(int tips)
    {
        if (tipsText != null)
        {
            tipsText.text = $"Tips: ${tips}";
        }
    }

    private void UpdateWave(int wave)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {wave}";
        }
    }

    private void UpdateTimer()
    {
        if (GameManager.Instance != null && timerText != null)
        {
            float time = GameManager.Instance.GetWaveTime();
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged.RemoveListener(UpdateScore);
            GameManager.Instance.OnTipsChanged.RemoveListener(UpdateTips);
            GameManager.Instance.OnWaveStart.RemoveListener(UpdateWave);
        }
    }
}
