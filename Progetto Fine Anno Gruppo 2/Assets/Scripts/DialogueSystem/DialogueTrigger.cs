using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]private DialogueHolderSO m_dialogueToShow;
    public bool isFinished = false;

    private void Update()
    {
        if (isFinished) Destroy(gameObject);
    }

}
