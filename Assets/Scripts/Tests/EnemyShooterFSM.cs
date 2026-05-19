using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EnemyShooterFSMTests
{
    private GameObject fsmObj;
    private EnemyShooterFSM fsm;

    [SetUp]
    public void Setup()
    {
        fsmObj = new GameObject("TestShooterFSM");
        fsm = fsmObj.AddComponent<EnemyShooterFSM>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(fsmObj);
    }

    [Test]
    public void FSM_StartsInMovingState()
    {
        object state = GetPrivateField(fsm, "currentState");
        
        Assert.AreEqual(EnemyShooterFSM.EnemyState.MovingToPosition, state);
    }

    [Test]
    public void MoveDown_SwitchesStateToShooting_WhenTargetYReached()
    {
        float targetY = (float)GetPrivateField(fsm, "targetStopY");
        fsmObj.transform.position = new Vector3(0, targetY - 1f, 0);

        CallPrivateMethod(fsm, "MoveDown");

        object state = GetPrivateField(fsm, "currentState");
        Assert.AreEqual(EnemyShooterFSM.EnemyState.Shooting, state, 
            "Коли ворог долітає до потрібної точки, він має перейти в стан 'Shooting'.");
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
}