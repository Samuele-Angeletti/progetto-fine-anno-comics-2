using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;
using Cinemachine;
using ArchimedesMiniGame;
using System;
using UnityEngine.SceneManagement;

namespace MainGame
{
    [RequireComponent(typeof(PlayerInputSystem))]
    public class GameManager : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManager m_instance;
        public static GameManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<GameManager>();

                    if (m_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        m_instance = container.AddComponent<GameManager>();
                    }
                }

                return m_instance;
            }
        }
        #endregion

        [Header("Camera Settings")]
        [SerializeField] CinemachineVirtualCamera m_CameraOnPlayer;
        [SerializeField] CinemachineVirtualCamera m_CameraOnModule;
        [SerializeField] CinemachineVirtualCamera m_CameraOnModuleFocused;

        [Header("BackGround Music")]
        [SerializeField,ShowScriptableObject] AudioAssetSO currentBackGroundMusic;


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
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
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
            AudioManager.Instance.PlayBackGroundMusic(currentBackGroundMusic);
        }

        public void SetContinousMovement(bool active)
        {
            m_PlayerInputs.ChangeContinousMovement(active);
        }

        public void OnPublish(IMessage message)
        {
            if (message is ChangeContinousMovementMessage)
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
                if(m_Player != null)
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

        internal void SetTargetForCamera(Transform newFollow)
        {
            m_CameraOnModule.Follow = newFollow;
            m_CameraOnModuleFocused.Follow = newFollow;
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

        public void SetNewPlayer(PlayerMovementManager player)
        {
            m_Player = player;
            if (m_Controllable.GetComponent<PlayerMovementManager>() == null)
            {
                m_CameraOnPlayer.Follow = m_Player.transform;
                SetNewControllable(m_Player);
            }
        }

        public void SetNewControllable(Controllable newControllable)
        {
            m_Controllable = newControllable;
            m_PlayerInputs.SetControllable(m_Controllable);

            if(m_Controllable.GetComponent<PlayerMovementManager>() != null)
            {
                SetActiveCamera(ECameras.Player);
            }
            else if(m_Controllable.GetComponent<Module>() != null)
            {
                SetActiveCamera(ECameras.Module);
            }

        }

        public void SetActiveCamera(ECameras camera)
        {
            switch (camera)
            {
                case ECameras.Player:
                    m_CameraOnPlayer.Priority = 1;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 0;
                    break;
                case ECameras.Module:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 1;
                    m_CameraOnModuleFocused.Priority = 0;
                    break;
                case ECameras.ModuleFocused:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 1;
                    break;
            }
        }

        public ButtonInteractionScriptableObject GetButtonInteractionSO(EInteractionType interactionType)
        {
            return m_ButtonInteractionSO.Find(x => x.InteractionType == interactionType);
        }

        [ContextMenu("Debug Save")]
        public void Save()
        {
            PubSub.PubSub.Publish(new SaveMessage());

            Invoke("InvokeSave", 1);
        }

        private void InvokeSave()
        {
            SaveAndLoadSystem.Save();
        }

        internal void SaveAndChangeScene(string v)
        {
            InvokeSave();
            SceneManager.LoadScene(v);
        }
    }
}
