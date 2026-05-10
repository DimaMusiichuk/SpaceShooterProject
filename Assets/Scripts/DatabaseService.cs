using UnityEngine;
using System.IO;

public class DatabaseService : MonoBehaviour
{
    public static DatabaseService Instance;
    
    private string saveFilePath;

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
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "player_save.json");
    }

    public PlayerData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        
        return new PlayerData { coins = 0 }; 
    }

    public void AddCoinsAndSave(int coinsToAdd)
    {
        PlayerData data = LoadData();
        data.coins += coinsToAdd;

        string json = JsonUtility.ToJson(data, true); 
        
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log($"<color=green>Прогрес збережено!</color> Всього монет: {data.coins}. Файл тут: {saveFilePath}");
    }

    public void SaveData(PlayerData updatedData)
    {
        string json = JsonUtility.ToJson(updatedData, true); 
        File.WriteAllText(saveFilePath, json);
        Debug.Log("<color=yellow>Дані магазину успішно збережено!</color>");
    }
}