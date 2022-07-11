using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static CoroutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine)
    {
        return new CoroutineHandle(owner, coroutine);
    }
}
