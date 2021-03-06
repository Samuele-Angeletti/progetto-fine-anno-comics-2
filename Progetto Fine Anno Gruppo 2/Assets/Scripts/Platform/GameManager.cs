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
        [SerializeField] CinemachineVirtualCamera m_CameraOnTransition;
        [SerializeField] CinemachineVirtualCamera m_CameraOnPlayerFar;
        [SerializeField] CinemachineVirtualCamera m_CameraOnPlayerClose;
        [SerializeField] BackgroundAxe m_Background;

        [Header("Player Settings")]
        public Controllable m_Controllable;

        [Header("Button Interaction SO")]
        [SerializeField] List<ButtonInteractionScriptableObject> m_ButtonInteractionSO;

        [Header("Archimedes")]
        [SerializeField] Archimedes m_Archimedes;

        [Header("Light Manager")]
        [SerializeField] LightManager m_LightManager;

        [Header("Debug Settings")]
        [SerializeField] DebugText m_ElapsedTimeZeroG;

        private QuestSystem m_QuestSystem;
        private PlayerInputSystem m_PlayerInputs;
        private bool m_ZeroGActive;
        private PlayerMovementManager m_Player;
        private ECameras m_ActiveCamera;
        public Archimedes Archimedes => m_Archimedes;
        
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
            m_QuestSystem = GetComponent<QuestSystem>();
            
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
            m_QuestSystem.SetNextQuest(0);
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

                DockingCompleteMessage dockingCompleteMessage = (DockingCompleteMessage)message;
                if(dockingCompleteMessage.Module.GetComponent<Capsula>() == null)
                    SetActiveCamera(ECameras.PlayerClose);
            }
            else if(message is StartEngineModuleMessage)
            {
                StartEngineModuleMessage startEngineModule = (StartEngineModuleMessage)message;
                SetNewControllable(startEngineModule.Module);
                SetActiveCamera(ECameras.Module);
            }
            else if(message is ModuleDestroyedMessage)
            {
                SetNewControllable(m_Player);
                SetActiveCamera(ECameras.PlayerClose);
            }
            else if(message is NoBatteryMessage)
            {
                SetNewControllable(m_Player);
                SetActiveCamera(ECameras.PlayerClose);
            }
        }

        internal IEnumerator SetTargetForCamera(Transform newFollow)
        {
            yield return new WaitForSeconds(3);
            m_CameraOnModule.Follow = newFollow;
            m_CameraOnModuleFocused.Follow = newFollow.gameObject.GetComponent<Module>().DockingPoint.transform;
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
                m_CameraOnPlayerFar.Follow = m_Player.transform;
                m_CameraOnPlayerClose.Follow = m_Player.transform;
                m_CameraOnTransition.Follow = m_Player.transform;
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

        public void ForcePlayerAsControllable()
        {
            SetNewControllable(m_Player);
        }

        public void SetActiveCamera(ECameras camera)
        {
            if (m_ActiveCamera == camera) return;

            switch (camera)
            {
                case ECameras.Player:
                    m_CameraOnPlayer.Priority = 1;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 0;
                    m_CameraOnPlayerFar.Priority = 0;
                    m_CameraOnPlayerClose.Priority = 0;
                    m_CameraOnTransition.Priority = 0;
                    m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, m_CameraOnPlayer.m_Lens.OrthographicSize);
                    break;
                case ECameras.Module:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 1;
                    m_CameraOnModuleFocused.Priority = 0;
                    m_CameraOnPlayerFar.Priority = 0;
                    m_CameraOnPlayerClose.Priority = 0;
                    m_CameraOnTransition.Priority = 0;
                    m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, m_CameraOnModule.m_Lens.OrthographicSize);
                    break;
                case ECameras.ModuleFocused:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 1;
                    m_CameraOnPlayerFar.Priority = 0;
                    m_CameraOnPlayerClose.Priority = 0;
                    m_CameraOnTransition.Priority = 0;
                    m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, m_CameraOnModuleFocused.m_Lens.OrthographicSize);
                    break;
                case ECameras.PlayerFar:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 0;
                    m_CameraOnPlayerFar.Priority = 1;
                    m_CameraOnPlayerClose.Priority = 0;
                    m_CameraOnTransition.Priority = 0;
                    m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, m_CameraOnPlayerFar.m_Lens.OrthographicSize);
                    break;
                case ECameras.PlayerClose:
                    m_CameraOnPlayer.Priority = 0;
                    m_CameraOnModule.Priority = 0;
                    m_CameraOnModuleFocused.Priority = 0;
                    m_CameraOnPlayerFar.Priority = 0;
                    m_CameraOnPlayerClose.Priority = 1;
                    m_CameraOnTransition.Priority = 0;
                    m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, m_CameraOnPlayerClose.m_Lens.OrthographicSize);
                    break;
            }
            m_ActiveCamera = camera;
        }

        public ButtonInteractionScriptableObject GetButtonInteractionSO(EInteractionType interactionType)
        {
            return m_ButtonInteractionSO.Find(x => x.InteractionType == interactionType);
        }

        public void SetBackgroundToCameraSize(CinemachineVirtualCamera camera)
        {
            m_Background.ChangeScale(m_CameraOnPlayer.m_Lens.OrthographicSize, camera.m_Lens.OrthographicSize);
        }

        public void Save()
        {
            PubSub.PubSub.Publish(new SaveMessage());

            Invoke("InvokeSave", 1);
        }

        private void InvokeSave()
        {
            SaveAndLoadSystem.Save();
        }

        public void Load()
        {
            PubSub.PubSub.Publish(new LoadMessage(SaveAndLoadSystem.Load<Dictionary<string, SavableInfos>>()));
        }

        internal void SaveAndChangeScene(string v)
        {
            InvokeSave();
            SceneManager.LoadScene(v);
        }

        public void SetCameraOnPlayer(CinemachineVirtualCamera newCamera)
        {
            m_CameraOnPlayer = newCamera;
            if (m_Player != null)
            {
                m_CameraOnPlayer.Follow = m_Player.transform;
                m_CameraOnPlayerFar.Follow = m_Player.transform;
                m_CameraOnPlayerClose.Follow = m_Player.transform;
                m_CameraOnTransition.Follow = m_Player.transform;
            }
        }

        public void SetInternalLight(float newAmount)
        {
            m_LightManager.InternalLight = newAmount;
            m_LightManager.SetGlobalLight(newAmount);
        }    

        internal void GameOver()
        {
            Debug.Log("GAME OVER");
        }
    }
}
