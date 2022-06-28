using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using PubSub;
using UnityEngine.UI;
using Commons;
using System;
using MainGame;
using ArchimedesMiniGame;

public class DialoguePlayer : MonoBehaviour, ISubscriber
{

    public GameObject dialogueBox;
    public TextMeshProUGUI textToWrite;
    public Image spriteToChange;
    public GameObject ContinueMessage;

    [SerializeField] private PlayerInputSystem m_PlayerInputs;
    private Queue<string> m_dialogueLine;
    private Queue<ESpeaker> m_whoIsSpeakingRightNow;


    /* Risolvere problema con il cambio di controllable
     * Risolvere attivazione del dialogue box quando si entra nella modalità navicella
    */


    [HideInInspector] public Sprite spritePlayer;
    [HideInInspector] public Sprite spriteAdaPreIntegrazione;
    [HideInInspector] public Sprite spriteAdaPrimaIntegrazione;
    [HideInInspector] public Sprite spriteAdaSecondaIntegrazione;
    [HideInInspector] public Sprite spriteAdaFormaFinale;
    [HideInInspector] public Sprite vuotoSprite;
    [HideInInspector] public bool standardMessageIsPlaying;

    private Controllable m_controllable;
    [SerializeField] private float m_typeWriterSpeed;
    public bool dialogueIsPlaying = false;

    [Header("Messaggi predefiniti")]
    [ShowScriptableObject, SerializeField] DialogueHolderSO m_StandardMsgNoBattery;
    [ShowScriptableObject, SerializeField] DialogueHolderSO m_StandardMsgDockingComplete;
    [ShowScriptableObject, SerializeField] DialogueHolderSO m_StandardMsgStartEngine;
    [ShowScriptableObject, SerializeField] DialogueHolderSO m_StandardMsgModuleDestroyed;

    #region SINGLETONE PATTERN
    private static DialoguePlayer m_instance;

    public static DialoguePlayer Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DialoguePlayer>();
                if (m_instance == null)
                {
                    m_instance = new GameObject().AddComponent<DialoguePlayer>();
                }
            }
            return m_instance;
        }

    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        m_whoIsSpeakingRightNow = new Queue<ESpeaker>();

    }


    private void Start()
    {
        PubSub.PubSub.Subscribe(this, typeof(StartDialogueMessage));
        PubSub.PubSub.Subscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Subscribe(this, typeof(CurrentDialogueFinishedMessage));

        // modulo
        PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
        PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
        PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));

        m_dialogueLine = new Queue<string>();

    }

    public void OnPublish(IMessage message)
    {
        if (message is StartDialogueMessage)
        {
            StartDialogueMessage dialogueMessage = (StartDialogueMessage)message;
            StartCoroutine(Startdialogue(dialogueMessage.dialogue));

        }
        else if (message is CurrentDialogueFinishedMessage)
        {
            StopAllCoroutines();
            dialogueBox.SetActive(false);
        }
        else if (message is ModuleDestroyedMessage)
        {
            standardMessageIsPlaying = true;
            PubSub.PubSub.Publish(new StartDialogueMessage(GetRandomMessage(m_StandardMsgModuleDestroyed.Dialogo)));

        }
        else if (message is NoBatteryMessage)
        {
            standardMessageIsPlaying = true;
            PubSub.PubSub.Publish(new StartDialogueMessage(GetRandomMessage(m_StandardMsgNoBattery.Dialogo)));

        }
        else if (message is DockingCompleteMessage)
        {
            standardMessageIsPlaying = true;
            PubSub.PubSub.Publish(new StartDialogueMessage(GetRandomMessage(m_StandardMsgDockingComplete.Dialogo)));

        }
        else if (message is StartEngineModuleMessage)
        {
            standardMessageIsPlaying = true;
            PubSub.PubSub.Publish(new StartDialogueMessage(GetRandomMessage(m_StandardMsgStartEngine.Dialogo)));
        }
    }


    public IEnumerator Startdialogue(List<DialogueLine> dialogueToEnqueue)
    {


        if (dialogueToEnqueue == null) yield return null;
        m_dialogueLine.Clear();

        dialogueBox.SetActive(true);
	    ContinueMessage.SetActive(false);
        for (int i = 0; i < dialogueToEnqueue.Count; i++)
        {
            m_whoIsSpeakingRightNow.Enqueue(dialogueToEnqueue[i].WhoIsSpeaking);
            if (m_whoIsSpeakingRightNow.Peek() != ESpeaker.Vuoto)
            {
                m_dialogueLine.Enqueue($"{ESpeakerTostring(dialogueToEnqueue[i].WhoIsSpeaking)}: " + dialogueToEnqueue[i].DialougueString);
            }
            else
            {
                m_dialogueLine.Enqueue(dialogueToEnqueue[i].DialougueString);
            }
        }
        yield return DisplayNextDialogueLine();


    }



    public IEnumerator DisplayNextDialogueLine()
    {
    

        while (m_dialogueLine.Count >0)
        {
            string temp = m_dialogueLine.Dequeue();

            ChangeSpeakerImage();

            yield return TypeWriteEffect(temp, textToWrite);
        }
        if (m_dialogueLine.Count == 0)
        {

            PubSub.PubSub.Publish(new CurrentDialogueFinishedMessage());
            standardMessageIsPlaying = false;
            yield return new WaitForSeconds(1);
            yield return null;
        }


    }
    private void ChangeSpeakerImage()
    {
        ESpeaker whoIsSpeakingRightNow = m_whoIsSpeakingRightNow.Dequeue();
        if (whoIsSpeakingRightNow == ESpeaker.Vuoto)
        {

        }
        if (whoIsSpeakingRightNow == ESpeaker.Riemann)
        {
            spriteToChange.sprite = spritePlayer;
        }
        if (whoIsSpeakingRightNow == ESpeaker.Ada_Pre_Integrazione)
        {
            spriteToChange.sprite = spriteAdaPreIntegrazione;
        }
        if (whoIsSpeakingRightNow == ESpeaker.Ada_Prima_Integrazione)
        {
            spriteToChange.sprite = spriteAdaPrimaIntegrazione;
        }
        if (whoIsSpeakingRightNow == ESpeaker.Ada_Seconda_Integrazione)
        {
            spriteToChange.sprite = spriteAdaSecondaIntegrazione;
        }
        if (whoIsSpeakingRightNow == ESpeaker.Ada_Forma_Finale)
        {
            spriteToChange.sprite = spriteAdaFormaFinale;
        }

    }
    private string ESpeakerTostring(ESpeaker whoIsSpeaking)
    {
        string currentSpeaker = string.Empty;
        if (whoIsSpeaking == ESpeaker.Riemann)
        {
            currentSpeaker = "Reimann";

        }
        if (whoIsSpeaking != ESpeaker.Riemann)
        {
            currentSpeaker = "Ada";
        }
        return currentSpeaker;

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
            charIndex = Mathf.Clamp(charIndex,  0,lineaDiDialogo.Length);

            textLabel.text = lineaDiDialogo.Substring (0,charIndex);
            yield return null;
        }

        ContinueMessage.SetActive(true);
        yield return new WaitUntil(() => m_PlayerInputs.inputControls.Player.Interaction.phase == UnityEngine.InputSystem.InputActionPhase.Performed);

        dialogueIsPlaying = false;

    }
    public void OnDisableSubscribe()
    {
        PubSub.PubSub.Unsubscribe(this, typeof(StartDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(CurrentDialogueFinishedMessage));
           // mod
        PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
        PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));

    }

    private List<DialogueLine> GetRandomMessage(List<DialogueLine> dialogueHolderSO)
    {
        if (dialogueHolderSO == null) return null;
        int indiceRandomico = UnityEngine.Random.Range(0, dialogueHolderSO.Count);
        List<DialogueLine> temp = new List<DialogueLine>();
        temp.Add(dialogueHolderSO[indiceRandomico]);
        return temp;
    }
    private void Update()
    {
        m_controllable = GameManager.Instance.m_Controllable;
    }
}

