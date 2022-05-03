using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnEnd();
    public abstract void MyOnCollisionEnter2D(Collision2D collision);
    public abstract void OnFixedUpdate();
}
