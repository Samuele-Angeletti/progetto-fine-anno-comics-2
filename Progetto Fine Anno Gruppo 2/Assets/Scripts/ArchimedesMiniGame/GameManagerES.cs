using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.UI;
using Commons;
namespace ArchimedesMiniGame
{
    [RequireComponent(typeof(PlayerInputSystem))]
    public class GameManagerES : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerES _instance;
        public static GameManagerES Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManagerES>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        _instance = container.AddComponent<GameManagerES>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        [Header("Player Settings")]
        [SerializeField] Controllable m_Controllable;

        [Header("UI References")]
        [SerializeField] Slider m_BatterySlider;
        [SerializeField] Slider m_DamageSlider;
        [SerializeField] Button m_DockingAttemptButton;


        [Header("Save File")]
        [Tooltip("Nome del file da creare per salvare i dati di danneggiamento. Se questa stringa viene omessa, verr� usato il nome del GameObject del Modulo corrente")]
        [SerializeField] string m_FileName;

        private PlayerInputSystem m_PlayerInputs;

        private void Awake()
        {
            m_PlayerInputs = GetComponent<PlayerInputSystem>();
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));

            m_PlayerInputs.SetControllable(m_Controllable);

            ActiveDockingAttemptButton(false);
        }

        public void SaveData()
        {
            m_FileName = m_FileName == "" ? m_Controllable.gameObject.name : m_FileName;
            SaveAndLoadSystem.Save(m_Controllable.GetComponent<Module>().GetDamageableInfo(), m_FileName);
        }

        public void OnPublish(IMessage message)
        {
            if(message is DockingCompleteMessage)
            {
                SaveData();
            }
            else if(message is ModuleDestroyedMessage)
            {
                SaveData();
            }
            else if(message is NoBatteryMessage)
            {
                SaveData();
            }
            else if(message is PauseGameMessage)
            {
                Time.timeScale = 0;
            }
            else if(message is ResumeGameMessage)
            {
                Time.timeScale = 1;
            }
        }

        public void UpdateBatterySlider(float current, float maxValue)
        {
            m_BatterySlider.value = current / maxValue;
        }

        public void UpdateLifeSlider(float current, float maxValue)
        {
            m_DamageSlider.value = current / maxValue;
        }

        public void ActiveDockingAttemptButton(bool active)
        {
            m_DockingAttemptButton.interactable = active;
        }

        public void OnDisableSubscribe()
        {
            PubSub.PubSub.Unsubscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Unsubscribe(this, typeof(ResumeGameMessage));
        }

        private void OnDestroy()
        {
            OnDisableSubscribe();
        }
    }
}