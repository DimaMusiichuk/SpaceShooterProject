using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI відображення")]
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private GameObject shopPanel;

    void Start()
    {
        UpdateCoinsDisplay();

        if (shopPanel != null)
        {
            shopPanel.SetActive(false); 
        }
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
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Гра закрилася!");
        Application.Quit();
    }
}