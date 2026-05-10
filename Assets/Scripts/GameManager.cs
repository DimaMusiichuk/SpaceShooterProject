using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Елементи")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Налаштування економіки")]
    [SerializeField] private int pointsPerCoin = 100;
    
    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void ProcessGameOverLogic()
    {
        int earnedCoins = currentScore / pointsPerCoin; 
        Debug.Log($"Гра закінчена! Набрано очок: {currentScore}. Зароблено монет: {earnedCoins}");
        
        if (DatabaseService.Instance != null && earnedCoins > 0)
        {
            DatabaseService.Instance.AddCoinsAndSave(earnedCoins);
        }
    }
}