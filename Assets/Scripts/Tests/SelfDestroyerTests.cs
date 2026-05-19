using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class SelfDestroyerTests
{
    private GameObject obj;
    private SelfDestroyer destroyer;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject("ExplosionEffect");
        destroyer = obj.AddComponent<SelfDestroyer>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void SelfDestroyer_DefaultLifetime_IsCorrect()
    {
        Assert.AreEqual(1.0f, destroyer.lifetime, "Час життя ефекту за замовчуванням має бути рівно 1 секунда");
    }

    [Test]
    public void Start_ExecutesDestroyWithoutErrors()
    {
        MethodInfo startMethod = destroyer.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        
        Assert.DoesNotThrow(() => startMethod.Invoke(destroyer, null));
    }
}