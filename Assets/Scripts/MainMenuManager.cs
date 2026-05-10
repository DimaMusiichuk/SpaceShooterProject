using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI відображення")]
    [SerializeField] private TextMeshProUGUI coinsText;

    void Start()
    {
        UpdateCoinsDisplay();
    }

    public void UpdateCoinsDisplay()
    {
        if (DatabaseService.Instance != null && coinsText != null)
        {
            PlayerData data = DatabaseService.Instance.LoadData();
            coinsText.text = "Coins: " + data.coins;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void OpenShop()
    {
        Debug.Log("Відкриваємо меню магазину...");
    }

    public void QuitGame()
    {
        Debug.Log("Гра закрилася");
        Application.Quit(); 
    }
}