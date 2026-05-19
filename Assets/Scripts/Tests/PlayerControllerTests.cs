using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class PlayerControllerTests
{
    private GameObject playerObj;
    private PlayerController player;
    private GameObject textObj;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("TestPlayer");
        player = playerObj.AddComponent<PlayerController>();

        textObj = new GameObject("HpText");
        player.hpDisplay = textObj.AddComponent<TextMeshProUGUI>();

        player.hp = 3;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(textObj);

        if (GameManager.Instance != null)
        {
            Object.DestroyImmediate(GameManager.Instance.gameObject);
        }
    }

    [Test]
    public void Player_DefaultValues_AreSetCorrectly()
    {
        Assert.AreEqual(3, player.hp, "Початкове здоров'я має бути рівним 3");
        Assert.AreEqual(7f, player.speed, "Початкова швидкість має бути 7");
    }

    [Test]
    public void Update_ClampsPlayerPosition_WithinScreenBounds()
    {
        playerObj.transform.position = new Vector3(20f, -20f, 0f);

        CallPrivateMethod(player, "Update");

        Assert.AreEqual(player.maxX, playerObj.transform.position.x, "Гравець не повинен вилітати за праву межу екрану");
        Assert.AreEqual(player.minY, playerObj.transform.position.y, "Гравець не повинен вилітати за нижню межу екрану");
    }

    [Test]
    public void OnTriggerEnter2D_WithEnemy_DecreasesHealth()
    {
        GameObject enemyObj = new GameObject("TestEnemy");
        enemyObj.tag = "Enemy";
        BoxCollider2D enemyCollider = enemyObj.AddComponent<BoxCollider2D>();

        GameObject gmObj = new GameObject("GameManager");
        GameManager.Instance = gmObj.AddComponent<GameManager>();

        MethodInfo triggerMethod = player.GetType().GetMethod("OnTriggerEnter2D", BindingFlags.NonPublic | BindingFlags.Instance);
        triggerMethod.Invoke(player, new object[] { enemyCollider });

        Assert.AreEqual(2, player.hp, "Зіткнення з ворогом має віднімати рівно 1 ХП");

        Object.DestroyImmediate(enemyObj);
    }

    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }
}