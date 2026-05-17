using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class DatabaseServiceTests
{
    private GameObject dbObj;
    private DatabaseService dbService;
    private string testFilePath;

    [SetUp]
    public void Setup()
    {
        dbObj = new GameObject("DatabaseService");
        dbService = dbObj.AddComponent<DatabaseService>();
        
        CallPrivateMethod(dbService, "Awake");

        testFilePath = Path.Combine(Application.persistentDataPath, "test_save.json");
        SetPrivateField(dbService, "saveFilePath", testFilePath);
        
        if (File.Exists(testFilePath)) 
        {
            File.Delete(testFilePath);
        }
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(testFilePath)) 
        {
            File.Delete(testFilePath);
        }
        Object.DestroyImmediate(dbObj);
        
        DatabaseService.Instance = null; 
    }

    [Test]
    public void Awake_SetsSingletonInstance()
    {
        Assert.IsNotNull(DatabaseService.Instance, "Синглтон бази даних має успішно ініціалізуватися.");
        Assert.AreEqual(dbService, DatabaseService.Instance, "Змінна Instance має вказувати на поточний скрипт.");
    }

    [Test]
    public void LoadData_WhenNoFileExists_ReturnsDefaultPlayerData()
    {
        PlayerData data = dbService.LoadData();

        Assert.AreEqual(0, data.coins, "Якщо файлу немає, гравцю має видаватися 0 монет.");
        Assert.IsFalse(data.hasDoubleShot, "Без збережень подвійний постріл має бути вимкнений.");
    }

    [Test]
    public void SaveAndLoadData_WorksCorrectly()
    {
        PlayerData testData = new PlayerData();
        testData.coins = 999;
        testData.hasPremiumSkin = true;

        dbService.SaveData(testData);
        PlayerData loadedData = dbService.LoadData();

        Assert.AreEqual(999, loadedData.coins, "Кількість монет має коректно зберігатися у файл.");
        Assert.IsTrue(loadedData.hasPremiumSkin, "Куплені скіни мають зберігатися.");
    }

    [Test]
    public void AddCoinsAndSave_IncreasesCoinAmount()
    {
        PlayerData startData = new PlayerData { coins = 10 };
        dbService.SaveData(startData); 

        dbService.AddCoinsAndSave(50);
        PlayerData loadedData = dbService.LoadData();

        Assert.AreEqual(60, loadedData.coins, "Метод AddCoinsAndSave має додавати монети до вже існуючих.");
    }


    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(target, value);
    }
}