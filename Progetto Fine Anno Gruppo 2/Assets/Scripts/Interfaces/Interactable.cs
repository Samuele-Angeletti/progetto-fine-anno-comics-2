using System;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] public EInteractionType InteractionType;
        public abstract void Interact();
        public abstract void ShowUI(bool isVisible);
    }
}
