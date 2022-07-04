using System.Collections;
using UnityEngine;

public class CourutineHandle
{
    public bool IsDone
    {
        get;
        private set;
    }
    public object Current
    {
        get;
    }
    public bool MoveNext() => !IsDone;
    public void Reset()
    {
    }
    public CourutineHandle(MonoBehaviour owner, IEnumerator courotine)
    {
        Current = owner.StartCoroutine(courotine);
    }
    private IEnumerator Wrap(IEnumerator courutine)
    {
        yield return courutine;
        IsDone = true;
    }
}