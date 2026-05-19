using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class GameManagerTests
{
    private GameObject gmObj;
    private GameManager gm;

    [SetUp]
    public void Setup()
    {
        gmObj = new GameObject("GameManager");
        gm = gmObj.AddComponent<GameManager>();
        
        CallPrivateMethod(gm, "Awake");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gmObj);
        GameManager.Instance = null;
    }

    [Test]
    public void Awake_SetsSingletonInstanceCorrectly()
    {
        Assert.IsNotNull(GameManager.Instance);
        Assert.AreEqual(gm, GameManager.Instance);
    }

    [Test]
    public void AddScore_IncreasesInternalScore()
    {
        gm.AddScore(100);
        gm.AddScore(50);

        int currentScore = (int)GetPrivateField(gm, "currentScore");
        Assert.AreEqual(150, currentScore, "Метод AddScore має правильно підсумовувати очки.");
    }

    [Test]
    public void AddScore_UpdatesTextMeshProUI()
    {
        GameObject textObj = new GameObject("ScoreText");
        TextMeshProUGUI mockText = textObj.AddComponent<TextMeshProUGUI>();
        
        SetPrivateField(gm, "scoreText", mockText);

        gm.AddScore(250);

        Assert.AreEqual("Score: 250", mockText.text, "UI текст має оновлюватися при додаванні очок.");

        Object.DestroyImmediate(textObj);
    }


    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }

    private object GetPrivateField(object target, string fieldName)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return field.GetValue(target);
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(target, value);
    }
}