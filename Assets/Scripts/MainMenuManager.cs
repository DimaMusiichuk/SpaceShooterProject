using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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