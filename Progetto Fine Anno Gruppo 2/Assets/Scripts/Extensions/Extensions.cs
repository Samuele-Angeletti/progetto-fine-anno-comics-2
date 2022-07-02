using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static CourutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine)
    {
        return new CourutineHandle(owner, coroutine);
    }
}
