using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.UI;

namespace ArchimedesMiniGame
{
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

        [Header("UI References")]
        [SerializeField] Slider m_BatterySlider;
        [SerializeField] Slider m_DamageSlider;
        [SerializeField] Button m_DockingAttemptButton;
        [SerializeField] Button m_StopSpeedButton;


        [Header("Save File")]
        [Tooltip("Nome del file da creare per salvare i dati di danneggiamento. Se questa stringa viene omessa, verrà usato il nome del GameObject del Modulo corrente")]
        [SerializeField] string m_FileName;

        public Module m_CurrentModule; // to fix

        private void Awake()
        {
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(DockingCompleteMessage));
            PubSub.PubSub.Subscribe(this, typeof(ModuleDestroyedMessage));
            PubSub.PubSub.Subscribe(this, typeof(NoBatteryMessage));
            PubSub.PubSub.Subscribe(this, typeof(PauseGameMessage));
            PubSub.PubSub.Subscribe(this, typeof(ResumeGameMessage));

            ActiveDockingAttemptButton(false);
            ActiveStopSpeedButton(false);
        }

        public void SaveData()
        {
            m_FileName = m_FileName == "" ? m_CurrentModule.gameObject.name : m_FileName;
            SaveAndLoadSystem.Save(m_CurrentModule.GetDamageableInfo(), m_FileName);
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

        public void ActiveStopSpeedButton(bool active)
        {
            m_StopSpeedButton.interactable = active;
        }

        public void ActiveDockingAttemptButton(bool active)
        {
            m_DockingAttemptButton.interactable = active;
        }
    }
}
