using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class MainMenuManagerTests
{
    private GameObject menuObj;
    private MainMenuManager menuManager;
    private GameObject shopPanelObj;

    [SetUp]
    public void Setup()
    {
        menuObj = new GameObject("MainMenuManager");
        menuManager = menuObj.AddComponent<MainMenuManager>();

        shopPanelObj = new GameObject("ShopPanel");
        SetPrivateField(menuManager, "shopPanel", shopPanelObj);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(menuObj);
        Object.DestroyImmediate(shopPanelObj);

        if (DatabaseService.Instance != null)
        {
            Object.DestroyImmediate(DatabaseService.Instance.gameObject);
            DatabaseService.Instance = null;
        }
    }

    [Test]
    public void Start_HidesShopPanel_OnInitialization()
    {
        shopPanelObj.SetActive(true);

        CallPrivateMethod(menuManager, "Start");

        Assert.IsFalse(shopPanelObj.activeSelf, "Метод Start має ховати панель магазину при запуску гри");
    }

    [Test]
    public void OpenShop_ActivatesShopPanel()
    {
        shopPanelObj.SetActive(false);

        menuManager.OpenShop();

        Assert.IsTrue(shopPanelObj.activeSelf, "Метод OpenShop має показувати панель магазину");
    }

    [Test]
    public void CloseShop_DeactivatesShopPanel()
    {
        shopPanelObj.SetActive(true);

        menuManager.CloseShop();

        Assert.IsFalse(shopPanelObj.activeSelf, "Метод CloseShop має ховати панель магазину");
    }

    [Test]
    public void UpdateCoinsDisplay_UpdatesTextFromDatabase()
    {
        GameObject dbObj = new GameObject("DatabaseService");
        DatabaseService db = dbObj.AddComponent<DatabaseService>();
        
        CallPrivateMethod(db, "Awake");

        string testFilePath = System.IO.Path.Combine(Application.persistentDataPath, "test_menu_save.json");
        SetPrivateField(db, "saveFilePath", testFilePath);

        PlayerData testData = new PlayerData { coins = 777 };
        db.SaveData(testData);

        GameObject textObj = new GameObject("CoinsText");
        TextMeshProUGUI mockText = textObj.AddComponent<TextMeshProUGUI>();
        SetPrivateField(menuManager, "coinsText", mockText);

        menuManager.UpdateCoinsDisplay();

        Assert.AreEqual("Coins: 777", mockText.text, "Текст монет має правильно підтягуватися з бази даних");

        if (System.IO.File.Exists(testFilePath)) 
        {
            System.IO.File.Delete(testFilePath);
        }
        Object.DestroyImmediate(textObj);
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