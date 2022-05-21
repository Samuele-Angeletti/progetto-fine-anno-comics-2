using System;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{
    public abstract class Interactable : Gizmosable
    {
        [Header("Interaction Settings")]
        [SerializeField] public EInteractionType InteractionType;
        public abstract void Interact(Interacter interacter);
        public abstract void ShowUI(bool isVisible);
    }
}
