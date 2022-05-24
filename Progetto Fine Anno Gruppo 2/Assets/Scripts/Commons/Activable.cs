using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Commons
{
    public abstract class Activable : Gizmosable
    {


        public abstract void OnActive();
        public abstract void OnDeactive();
        /// <summary>
        /// This Method must be called on Start or Awake of the current Script. You should check if the variable m_Activator is null and place a LogError message in case.
        /// </summary>
        public abstract void OnStartVariablesReferredCheck();


    }
}
