using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class ShopManagerTests
{
    private GameObject shopObj;
    private ShopManager shop;
    private GameObject dbObj;
    private DatabaseService db;
    private string testFilePath;

    [SetUp]
    public void Setup()
    {
        shopObj = new GameObject("ShopManager");
        shop = shopObj.AddComponent<ShopManager>();

        dbObj = new GameObject("DatabaseService");
        db = dbObj.AddComponent<DatabaseService>();
        CallPrivateMethod(db, "Awake");

        testFilePath = System.IO.Path.Combine(Application.persistentDataPath, "test_shop_save.json");
        SetPrivateField(db, "saveFilePath", testFilePath);

        GameObject textObj = new GameObject("CoinsText");
        shop.coinsText = textObj.AddComponent<TextMeshProUGUI>();
        
        shop.healthPrice = 50;
        shop.doubleShotPrice = 150;
        shop.skinPrice = 300;
    }

    [TearDown]
    public void Teardown()
    {
        if (System.IO.File.Exists(testFilePath)) 
        {
            System.IO.File.Delete(testFilePath);
        }
        
        Object.DestroyImmediate(shopObj);
        Object.DestroyImmediate(dbObj);
        
        if (shop.coinsText != null)
        {
            Object.DestroyImmediate(shop.coinsText.gameObject);
        }
        
        DatabaseService.Instance = null;
    }

    [Test]
    public void BuyExtraHealth_WithEnoughCoins_DeductsCoinsAndAddsHealth()
    {
        db.SaveData(new PlayerData { coins = 100, extraHealth = 0 });

        shop.BuyExtraHealth();

        PlayerData data = db.LoadData();
        Assert.AreEqual(50, data.coins, "Після покупки ХП має знятися 50 монет");
        Assert.AreEqual(1, data.extraHealth, "Лічильник додаткового ХП має збільшитися на 1");
    }

    [Test]
    public void BuyExtraHealth_WithoutEnoughCoins_DoesNothing()
    {
        db.SaveData(new PlayerData { coins = 10, extraHealth = 0 });

        shop.BuyExtraHealth();

        PlayerData data = db.LoadData();
        Assert.AreEqual(10, data.coins, "Монети не мають списуватися, якщо їх недостатньо");
        Assert.AreEqual(0, data.extraHealth, "ХП не має додаватися без успішної оплати");
    }

    [Test]
    public void BuyDoubleShot_WithEnoughCoins_UnlocksFeature()
    {
        db.SaveData(new PlayerData { coins = 200, hasDoubleShot = false });
        
        shop.BuyDoubleShot();
        
        PlayerData data = db.LoadData();
        Assert.AreEqual(50, data.coins, "Має знятися 150 монет за подвійний постріл");
        Assert.IsTrue(data.hasDoubleShot, "Подвійний постріл має бути розблокований");
    }

    [Test]
    public void BuyPremiumSkin_AlreadyBought_DoesNotDeductCoins()
    {
        db.SaveData(new PlayerData { coins = 1000, hasPremiumSkin = true });
        
        shop.BuyPremiumSkin();
        
        PlayerData data = db.LoadData();
        Assert.AreEqual(1000, data.coins, "Монети не мають відніматися, якщо преміум скін вже був куплений раніше");
    }

    [Test]
    public void UpdateShopUI_DisplaysCorrectCoinsAmount()
    {
        db.SaveData(new PlayerData { coins = 888 });
        
        shop.UpdateShopUI();

        Assert.AreEqual("Coins: 888", shop.coinsText.text, "Текст UI має коректно відображати монети, витягнуті з бази даних.");
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