using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "SO/Quest")]
    public class QuestSO : ScriptableObject
    {
        public string MessageOnScreen;
        [HideInInspector]
        public bool QuestAccomplished = false;
    }
}
