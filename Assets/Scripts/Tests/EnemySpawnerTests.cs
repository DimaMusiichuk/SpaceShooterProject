using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnerTests
{
    private GameObject spawnerObj;
    private EnemySpawner spawner;

    [SetUp]
    public void Setup()
    {
        spawnerObj = new GameObject("TestEnemySpawner");
        spawner = spawnerObj.AddComponent<EnemySpawner>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(spawnerObj);
    }

    [Test]
    public void EnemySpawner_DefaultSpawnBounds_AreValid()
    {
        float minX = (float)GetPrivateField(spawner, "minX");
        float maxX = (float)GetPrivateField(spawner, "maxX");
        float spawnY = (float)GetPrivateField(spawner, "spawnY");

        Assert.Less(minX, maxX);
        Assert.Greater(spawnY, 0f, "Точка спавну має бути вище центру екрану, щоб вороги падали зверху");
    }

    [Test]
    public void EnemySpawner_SpawnRateRange_IsLogical()
    {
        Vector2 rateRange = (Vector2)GetPrivateField(spawner, "spawnRateRange");

        Assert.Greater(rateRange.x, 0f, "Мінімальний час між спавнами має бути більшим за 0, щоб гра не зависла від нескінченного циклу.");
        Assert.LessOrEqual(rateRange.x, rateRange.y, "Мінімальний час спавну не може бути більшим за максимальний.");
    }

    [Test]
    public void SpawnRoutine_InitializesCoroutineSuccessfully()
    {
        MethodInfo routineMethod = spawner.GetType().GetMethod("SpawnRoutine", BindingFlags.NonPublic | BindingFlags.Instance);
        IEnumerator coroutine = (IEnumerator)routineMethod.Invoke(spawner, null);

        Assert.IsNotNull(coroutine, "Метод SpawnRoutine має успішно повертати об'єкт корутини для запуску.");
    }

    private object GetPrivateField(object target, string fieldName)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return field.GetValue(target);
    }
}