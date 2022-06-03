using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using Cinemachine;
using ArchimedesMiniGame;

namespace MainGame
{
    [RequireComponent(typeof(PlayerInputSystem))]
    public class GameManagerIN : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerIN _instance;
        public static GameManagerIN Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManagerIN>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        _instance = container.AddComponent<GameManagerIN>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        [Header("Camera Settings")]
        [SerializeField] CinemachineVirtualCamera m_CameraOnPlayer;
        [SerializeField] CinemachineVirtualCamera m_CameraOnModule;
        [SerializeField] CinemachineVirtualCamera m_CamerOnDialogue;
        

        [Header("Player Settings")]
        [SerializeField] Controllable m_Controllable;

        [Header("Button Interaction SO")]
        [SerializeField] List<ButtonInteractionScriptableObject> m_ButtonInteractionSO;


        [Header("Debug Settings")]
        [SerializeField] DebugText m_ElapsedTimeZeroG;

        private PlayerInputSystem m_PlayerInputs;
        private bool m_ZeroGActive;
        private PlayerMovementManager m_Player;
        public bool ZeroGActive
        {
            get => m_ZeroGActive;
        }
        private void Awake()
        {
            
            m_PlayerInputs = GetComponent<PlayerInputSystem>();
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(ChangeContinousMovementMessage));
            PubSub.PubSub.Subscribe(this, typeof(ZeroGMessage));
            PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Subscribe(this, typeof(StartDialogueMessage));
            PubSub.PubSub.Subscribe(this,typeof(EndDialogueMessage));
            PubSub.PubSub.Subscribe(this, typeof(CurrentDialogueFinishedMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));


            m_Player = m_Controllable.GetComponent<PlayerMovementManager>();
            SetNewControllable(m_Controllable);
        }

        public void SetContinousMovement(bool active)
        {
            m_PlayerInputs.ChangeContinousMovement(active);
        }

        public void OnPublish(IMessage message)
        {
            if (message is StartDialogueMessage)
            {
                if (m_Controllable.GetComponent<PlayerMovementManager>() != null)
                {
                    //m_CamerOnDialogue.Priority = 1; // nope. bisogna usare la current camera, perch� se passo al modulo e parte il dialogo, allora la telecamera deve andare al modulo
                    //m_CameraOnPlayer.Priority = 0;
                }

            }
            else if (message is EndDialogueMessage || message is CurrentDialogueFinishedMessage)
            {
                if (m_Controllable.GetComponent<PlayerMovementManager>() != null)
                {
                    //m_CamerOnDialogue.Priority = 0;
                    //m_CameraOnPlayer.Priority = 1;
                }
            }
            else if (message is ChangeContinousMovementMessage)
            {
                ChangeContinousMovementMessage changeContinousMovement = (ChangeContinousMovementMessage)message;
                m_PlayerInputs.ChangeContinousMovement(changeContinousMovement.Active);
            }
            else if(message is ZeroGMessage)
            {
                ZeroGMessage zeroG = (ZeroGMessage)message;
                if(zeroG.Active)
                {
                    m_ElapsedTimeZeroG.Active(true);
                    m_ElapsedTimeZeroG.SetMessage("ELAPSED TIME IN ZERO G: ");
                }
                else
                {
                    m_ElapsedTimeZeroG.Active(false);
                }
                m_ZeroGActive = zeroG.Active;
            }
            else if (message is PauseGameMessage)
            {
                Time.timeScale = 0;
            }
            else if (message is ResumeGameMessage)
            {
                Time.timeScale = 1;
            }
            else if(message is DockingCompleteMessage)
            {
                SetNewControllable(m_Player);
            }
            else if(message is StartEngineModuleMessage)
            {
                StartEngineModuleMessage startEngineModule = (StartEngineModuleMessage)message;
                SetNewControllable(startEngineModule.Module);
            }
            else if(message is ModuleDestroyedMessage)
            {
                SetNewControllable(m_Player);
            }
            else if(message is NoBatteryMessage)
            {
                SetNewControllable(m_Player);
            }
        }

        public void OnDisableSubscribe()
        {

            PubSub.PubSub.Unsubscribe(this, typeof(ChangeContinousMovementMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ZeroGMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ResumeGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(StartEngineModuleMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(StartDialogueMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(EndDialogueMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(CurrentDialogueFinishedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }

        public void SetNewControllable(Controllable newControllable)
        {
            m_Controllable = newControllable;
            m_PlayerInputs.SetControllable(m_Controllable);

            if(m_Controllable.GetComponent<PlayerMovementManager>() != null)
            {
                m_CameraOnPlayer.Priority = 1;
                m_CameraOnModule.Priority = 0;
            }
            else if(m_Controllable.GetComponent<Module>() != null)
            {
                m_CameraOnPlayer.Priority = 0;
                m_CameraOnModule.Priority = 1;
            }

        }

        public ButtonInteractionScriptableObject GetButtonInteractionSO(EInteractionType interactionType)
        {
            return m_ButtonInteractionSO.Find(x => x.InteractionType == interactionType);
        }
    }
}
