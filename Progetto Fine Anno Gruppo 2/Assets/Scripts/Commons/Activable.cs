using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activable : MonoBehaviour
{
    [SerializeField] public Activator m_Activator;

    public abstract void OnActive();
    public abstract void OnDeactive();
    /// <summary>
    /// This Method must be called on Start or Awake of the current Script. You should check if the variable m_Activator is null and place a LogError message in case.
    /// </summary>
    public abstract void OnStartVariablesReferredCheck();
}
