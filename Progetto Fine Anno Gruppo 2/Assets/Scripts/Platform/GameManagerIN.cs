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
            m_Player = m_Controllable.GetComponent<PlayerMovementManager>();
            SetNewControllable(m_Controllable);
        }

        public void SetContinousMovement(bool active)
        {
            m_PlayerInputs.ChangeContinousMovement(active);
        }

        public void OnPublish(IMessage message)
        {
            if(message is ChangeContinousMovementMessage)
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
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(ChangeContinousMovementMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
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
