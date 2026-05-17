using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class BackgroundChainManagerTests
{
    private BackgroundChainManager manager;
    private GameObject managerObj;

    [SetUp]
    public void Setup()
    {
        managerObj = new GameObject("BgManager");
        manager = managerObj.AddComponent<BackgroundChainManager>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(managerObj);
    }

    [Test]
    public void Start_EmptyNodesArray_DoesNotCrash()
    {
        SetPrivateField(manager, "backgroundNodes", new Transform[0]);

        Assert.DoesNotThrow(() => CallPrivateMethod(manager, "Start"));
    }

    [Test]
    public void Start_CalculatesHeight_And_PositionsNodesCorrectly()
    {
        GameObject bg1 = new GameObject("BG1");
        SpriteRenderer sr1 = bg1.AddComponent<SpriteRenderer>();
        sr1.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 10, 10), Vector2.zero);
        
        GameObject bg2 = new GameObject("BG2");
        SpriteRenderer sr2 = bg2.AddComponent<SpriteRenderer>();
        sr2.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 10, 10), Vector2.zero);

        Transform[] nodes = new Transform[] { bg1.transform, bg2.transform };
        SetPrivateField(manager, "backgroundNodes", nodes);

        CallPrivateMethod(manager, "Start");

        Assert.Greater(bg2.transform.position.y, bg1.transform.position.y, 
            "Другий елемент ланцюга має бути розташований вище за перший.");

        Object.DestroyImmediate(bg1);
        Object.DestroyImmediate(bg2);
    }

    [Test]
    public void Update_MovesBackgroundDownwards()
    {
        GameObject bg = new GameObject("BG1");
        bg.transform.position = Vector3.zero;
        
        Transform[] nodes = new Transform[] { bg.transform };
        SetPrivateField(manager, "backgroundNodes", nodes);
        SetPrivateField(manager, "scrollSpeed", 5f);
        
        CallPrivateMethod(manager, "Update");

        Assert.Less(bg.transform.position.y, 0f);

        Object.DestroyImmediate(bg);
    }


    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(target, value);
    }

    private void CallPrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(target, null);
    }
}