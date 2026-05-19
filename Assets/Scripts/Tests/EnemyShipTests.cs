using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemyShipTests
{
    private GameObject shipObj;
    private EnemyShip ship;

    [SetUp]
    public void Setup()
    {
        shipObj = new GameObject("TestEnemyShip");
        ship = shipObj.AddComponent<EnemyShip>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(shipObj);
        
        if (GameManager.Instance != null)
        {
            Object.DestroyImmediate(GameManager.Instance.gameObject);
        }
    }

    [Test]
    public void EnemyShip_DefaultMovementValues_AreCorrect()
    {
        Assert.AreEqual(2f, ship.speed, "Швидкість падіння корабля має бути 2");
        Assert.AreEqual(2f, ship.tiltSpeed, "Швидкість хитання має бути 2");
        Assert.AreEqual(3f, ship.tiltAmount, "Амплітуда хитання по X має бути 3");
    }

    [Test]
    public void Update_MovesShipDownwards()
    {
        shipObj.transform.position = Vector3.zero;
        CallPrivateMethod(ship, "Start");
        
        CallPrivateMethod(ship, "Update");

        Assert.Less(shipObj.transform.position.y, 0f, "Корабель повинен летіти вниз");
    }

    [Test]
    public void OnTriggerEnter2D_WithBullet_DestroysShipAndAddsScore()
    {
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();
        GameManager.Instance = gm;

        GameObject bulletObj = new GameObject("TestBullet");
        bulletObj.tag = "Bullet";
        BoxCollider2D bulletCollider = bulletObj.AddComponent<BoxCollider2D>();

        MethodInfo triggerMethod = ship.GetType().GetMethod("OnTriggerEnter2D", BindingFlags.NonPublic | BindingFlags.Instance);
        
        Assert.DoesNotThrow(() => triggerMethod.Invoke(ship, new object[] { bulletCollider }), 
            "Корабель має успішно вибухнути та нарахувати 150 очок без помилок.");

        Object.DestroyImmediate(bulletObj);
    }

    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }
}