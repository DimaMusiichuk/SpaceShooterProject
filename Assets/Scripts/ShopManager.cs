using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Налаштування цін")]
    public int healthPrice = 50;
    public int doubleShotPrice = 150;
    public int skinPrice = 300;

    [Header("UI Елементи")]
    public TextMeshProUGUI coinsText;
    void OnEnable()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        if (DatabaseService.Instance != null && coinsText != null)
        {
            PlayerData data = DatabaseService.Instance.LoadData();
            coinsText.text = "Coins: " + data.coins;
        }
    }

    public void BuyExtraHealth()
    {
        PlayerData data = DatabaseService.Instance.LoadData();
        
        if (data.coins >= healthPrice)
        {
            data.coins -= healthPrice;
            data.extraHealth += 1;
            
            DatabaseService.Instance.SaveData(data);
            UpdateShopUI();
            
            Debug.Log("<color=green>Куплено +1 HP!</color> Тепер додаткових життів: " + data.extraHealth);
        }
        else
        {
            Debug.Log("<color=red>Недостатньо монет для купівлі HP!</color>");
        }
    }

    public void BuyDoubleShot()
    {
        PlayerData data = DatabaseService.Instance.LoadData();
        
        if (data.hasDoubleShot) 
        {
            Debug.Log("Подвійний постріл вже куплено раніше!");
            return;
        }

        if (data.coins >= doubleShotPrice)
        {
            data.coins -= doubleShotPrice;
            data.hasDoubleShot = true;
            
            DatabaseService.Instance.SaveData(data);
            UpdateShopUI();
            
            Debug.Log("<color=green>Куплено Подвійний постріл!</color>");
        }
        else
        {
            Debug.Log("<color=red>Недостатньо монет!</color>");
        }
    }

    public void BuyPremiumSkin()
    {
        PlayerData data = DatabaseService.Instance.LoadData();
        
        if (data.hasPremiumSkin) 
        {
            Debug.Log("Скін вже куплено раніше!");
            return;
        }

        if (data.coins >= skinPrice)
        {
            data.coins -= skinPrice;
            data.hasPremiumSkin = true;
            
            DatabaseService.Instance.SaveData(data);
            UpdateShopUI();
            
            Debug.Log("<color=green>Куплено Premium Скін!</color>");
        }
        else
        {
            Debug.Log("<color=red>Недостатньо монет!</color>");
        }
    }
}