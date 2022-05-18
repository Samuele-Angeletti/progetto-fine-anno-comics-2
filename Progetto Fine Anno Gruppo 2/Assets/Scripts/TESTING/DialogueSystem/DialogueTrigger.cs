using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : MonoBehaviour
{
    
    [ShowScriptableObject]
    public DialogueHolderSO m_dialogueToShow;


    private void Update()
    {
       
    }

}
