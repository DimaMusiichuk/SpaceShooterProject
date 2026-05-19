using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemyBulletTests
{
    private GameObject bulletObj;
    private EnemyBullet bullet;

    [SetUp]
    public void Setup()
    {
        bulletObj = new GameObject("TestEnemyBullet");
        bullet = bulletObj.AddComponent<EnemyBullet>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(bulletObj);
    }

    [Test]
    public void EnemyBullet_DefaultValues_AreCorrect()
    {
        Assert.AreEqual(8f, bullet.speed, "Швидкість ворожої кулі має бути 8");
        Assert.AreEqual(4f, bullet.lifetime, "Час життя ворожої кулі має бути 4 секунди");
    }

    [Test]
    public void Start_SetsRotationDownwards()
    {
        MethodInfo startMethod = bullet.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        startMethod.Invoke(bullet, null);

        Assert.AreEqual(270f, bulletObj.transform.rotation.eulerAngles.z, 0.01f, 
            "Ворожа куля повинна бути повернута носом донизу");
    }

    [Test]
    public void Update_MovesBulletDownwards_InWorldSpace()
    {
        bulletObj.transform.position = Vector3.zero;

        MethodInfo updateMethod = bullet.GetType().GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
        updateMethod.Invoke(bullet, null);

        Assert.Less(bulletObj.transform.position.y, 0f, 
            "Ворожа куля має летіти вниз до гравця");
    }
}