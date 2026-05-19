using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemyTests
{
    private GameObject enemyObj;
    private Enemy enemy;

    [SetUp]
    public void Setup()
    {
        enemyObj = new GameObject("TestEnemy");
        enemy = enemyObj.AddComponent<Enemy>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
        
        if (GameManager.Instance != null)
        {
            Object.DestroyImmediate(GameManager.Instance.gameObject);
        }
    }

    [Test]
    public void Enemy_DefaultFallSpeed_IsCorrect()
    {
        Assert.AreEqual(3f, enemy.fallSpeed, "Стандартна швидкість падіння астероїда має бути 3");
    }

    [Test]
    public void Update_MovesEnemyDownwards()
    {
        enemyObj.transform.position = Vector3.zero;

        CallPrivateMethod(enemy, "Update");

        Assert.Less(enemyObj.transform.position.y, 0f, 
            "Ворог повинен падати вниз по осі Y під час виклику Update");
    }

    [Test]
    public void OnTriggerEnter2D_WithBullet_DoesNotCrash()
    {
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();
        GameManager.Instance = gm;

        GameObject bulletObj = new GameObject("TestBullet");
        bulletObj.tag = "Bullet";
        BoxCollider2D bulletCollider = bulletObj.AddComponent<BoxCollider2D>();

        MethodInfo triggerMethod = enemy.GetType().GetMethod("OnTriggerEnter2D", BindingFlags.NonPublic | BindingFlags.Instance);
        
        Assert.DoesNotThrow(() => triggerMethod.Invoke(enemy, new object[] { bulletCollider }), 
            "Зіткнення з кулею має оброблятися без помилок");

        Object.DestroyImmediate(bulletObj);
    }

    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }
}