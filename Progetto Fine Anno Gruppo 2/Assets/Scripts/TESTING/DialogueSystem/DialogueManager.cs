using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using PubSub;
using UnityEngine.UI;
using Commons;

public class DialogueManager : MonoBehaviour, ISubscriber
{
    
    public GameObject dialogueBox;
    public TextMeshProUGUI textToWrite;
    public Image spriteToChange;
    

    private PlayerInputSystem m_PlayerInputs;
    private Queue<string> m_dialogueLine;
    private Queue<ESpeaker> m_whoIsSpeakingRightNow;


    [HideInInspector]public Sprite spriteAda;
    [HideInInspector]public Sprite spritePlayer;
    [SerializeField] private Controllable m_controllable;
    [SerializeField] private float m_typeWriterSpeed;
    public bool dialogueIsPlaying = false;
    
    #region SINGLETONE PATTERN
    private static DialogueManager m_instance;

    public static DialogueManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DialogueManager>();
                if (m_instance == null)
                {
                    m_instance = new GameObject().AddComponent<DialogueManager>();
                }
            }
            return m_instance;
        }
       
    }
    #endregion

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(this);
            DontDestroyOnLoad(gameObject);
        }
        m_PlayerInputs = GetComponent<PlayerInputSystem>();
        m_whoIsSpeakingRightNow = new Queue<ESpeaker>();
        
    }
    
    
    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(StartDialogueMessage));
        PubSub.PubSub.Subscribe(this, typeof(EndDialogueMessage));
        m_dialogueLine = new Queue<string>();
        m_PlayerInputs.SetControllable(m_controllable);

    }
 
    public void OnPublish(IMessage message)
    {
        if (message is StartDialogueMessage)
        {
            StartDialogueMessage dialogueMessage = (StartDialogueMessage)message;
            StartCoroutine(Startdialogue(dialogueMessage.dialogue));
            
        }
        if (message is EndDialogueMessage)
        {
            dialogueBox.SetActive(false);

        }
    }


    public IEnumerator Startdialogue(DialogueHolderSO dialogueToEnqueue)
    {
        
        if (dialogueToEnqueue == null) yield return null;
        m_dialogueLine.Clear();
        Debug.Log("cancellata");
        dialogueBox.SetActive(true);

        for (int i = 0; i < dialogueToEnqueue.Dialogo.Count; i++)
        {
            m_whoIsSpeakingRightNow.Enqueue(dialogueToEnqueue.Dialogo[i].WhoIsSpeaking);
            m_dialogueLine.Enqueue($"{dialogueToEnqueue.Dialogo[i].WhoIsSpeaking}: " + dialogueToEnqueue.Dialogo[i].DialougueLine);
        }
        yield return DisplayNextDialogueLine(dialogueToEnqueue);


    }

    public IEnumerator DisplayNextDialogueLine(DialogueHolderSO dialogue)
    {
       
        while (m_dialogueLine.Count >0)
        {
            string temp = m_dialogueLine.Dequeue();
            Debug.Log($"{m_dialogueLine.Count }");
            ChangeSpeakerImage();
            yield return TypeWriteEffect(temp, textToWrite);
           
        }
        if (m_dialogueLine.Count == 0)
        {
            PubSub.PubSub.Publish(new EndDialogueMessage());
            Debug.Log("dialogo finito");
            
            
            yield return null;
        }


    }

    private void ChangeSpeakerImage()
    {
        ESpeaker intPersonaggio = m_whoIsSpeakingRightNow.Dequeue();
        if (intPersonaggio == ESpeaker.Reimann)
        {
            spriteToChange.sprite = spritePlayer;
            
        }
        if (intPersonaggio == ESpeaker.Ada)
        {
            spriteToChange.sprite = spriteAda;
            
        }
        
    }

    private IEnumerator TypeWriteEffect(string lineaDiDialogo, TMP_Text textLabel)
    {
        dialogueIsPlaying = true;
        float t = 0f;
        int charIndex = 0;
        while (charIndex < lineaDiDialogo.Length)
        {
            t+= Time.deltaTime * m_typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0,lineaDiDialogo.Length);

            textLabel.text = lineaDiDialogo.Substring(0,charIndex);
            yield return null;
        }
        yield return new WaitUntil(() => m_PlayerInputs.inputControls.Player.Interaction.phase == UnityEngine.InputSystem.InputActionPhase.Performed);
        dialogueIsPlaying = false;

    }
    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(StartDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));
    } 
}
