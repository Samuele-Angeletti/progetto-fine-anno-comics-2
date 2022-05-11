using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using PubSub;

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

        [Header("Player Settings")]
        [SerializeField] Controllable m_Controllable;

        [Header("Debug Settings")]
        [SerializeField] DebugText m_ElapsedTimeZeroG;

        private PlayerInputSystem m_PlayerInputs;

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
            m_PlayerInputs.SetControllable(m_Controllable);
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
            }
            else if (message is PauseGameMessage)
            {
                Time.timeScale = 0;
            }
            else if (message is ResumeGameMessage)
            {
                Time.timeScale = 1;
            }
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(ChangeContinousMovementMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}
