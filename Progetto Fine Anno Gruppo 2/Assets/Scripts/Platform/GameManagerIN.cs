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

        private PlayerInputSystem m_PlayerInputs;

        private void Awake()
        {
            
            m_PlayerInputs = GetComponent<PlayerInputSystem>();
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(ChangeContinousMovementMessage));
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
                Debug.Log(message);
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
