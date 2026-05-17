using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class BulletTests
{
    private GameObject bulletObj;
    private Bullet bullet;

    [SetUp]
    public void Setup()
    {
        bulletObj = new GameObject("TestBullet");
        bullet = bulletObj.AddComponent<Bullet>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(bulletObj);
    }

    [Test]
    public void Bullet_DefaultValues_AreSetCorrectly()
    {
        Assert.AreEqual(12f, bullet.speed, "Стандартна швидкість кулі має бути 12.");
        Assert.AreEqual(3f, bullet.lifetime, "Стандартний час життя кулі має бути 3 секунди.");
    }

    [Test]
    public void Start_SetsCorrectRotation_ForBullet()
    {
        CallPrivateMethod(bullet, "Start");

        Assert.AreEqual(90f, bulletObj.transform.rotation.eulerAngles.z, 0.01f, 
            "При старті куля повинна повернутися на 90 градусів (щоб летіти носом уперед).");
    }

    [Test]
    public void Update_MovesBulletUpwards_InWorldSpace()
    {
        bulletObj.transform.position = Vector3.zero;

        CallPrivateMethod(bullet, "Update");

        Assert.Greater(bulletObj.transform.position.y, 0f, 
            "Куля повинна рухатися вгору (Vector3.up) під час виклику Update.");
    }

    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }
}