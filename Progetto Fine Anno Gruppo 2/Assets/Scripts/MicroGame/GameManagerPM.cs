using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using Commons;
using TMPro;
using UnityEngine.UI;

namespace MicroGame
{
    public class GameManagerPM : MonoBehaviour, ISubscriber
    {
        #region SINGLETON PATTERN
        public static GameManagerPM _instance;
        public static GameManagerPM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManagerPM>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GameManager");
                        _instance = container.AddComponent<GameManagerPM>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        [Header("UI Stuff")]
        [SerializeField] TextMeshProUGUI m_LifeText;
        [SerializeField] Slider m_BatterySlider;

        [Header("Load File")]
        [SerializeField] string m_FileName;

        private List<Pickable> m_PickableList = new List<Pickable>();
        private int m_PickablesInScene;
        private int m_PickablePicked = 0;
        private void Awake()
        {
            Pickable[] pickables = FindObjectsOfType<Pickable>();
            for (int i = 0; i < pickables.Length; i++)
            {
                m_PickableList.Add(pickables[i]);
            }
            m_PickablesInScene = m_PickableList.Count;
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PickablePickedMessage));
            PubSub.PubSub.Subscribe(this, typeof(GameOverMicroGameMessage));

            StartSettings();
        }

        private void StartSettings()
        {
            DamageableInfos d = SaveAndLoadSystem.Load<DamageableInfos>(m_FileName);
            float damagePercentage = d.DamagePercentage();

            if (damagePercentage == 0)
            {
                Debug.Log("Spawna 1");
            }
            else if (damagePercentage <= 0.2f)
            {
                Debug.Log("Spawna 2");
            }
            else if (damagePercentage <= 0.4f)
            {
                Debug.Log("Spawna 3");
            }
            else if (damagePercentage <= 0.6f)
            {
                Debug.Log("Spawna 4");
            }
            else if (damagePercentage <= 0.8f)
            {
                Debug.Log("Spawna 5");
            }
            else if (damagePercentage < 1f)
            {
                Debug.Log("Spawna 6");
            }
            else
            {
                Debug.Log("Spawna 7");
            }
        }

        internal void UIUpdateLife(int m_CurrentLifes)
        {
            m_LifeText.text = "VITE: " + m_CurrentLifes;
        }

        public void OnPublish(IMessage message)
        {
            if(message is PickablePickedMessage)
            {
                PickablePickedMessage pickablePicked = (PickablePickedMessage)message;
                m_PickableList.Remove(pickablePicked.Pickable);
                CheckPickableLeft();
            }
            else if(message is GameOverMicroGameMessage)
            {
                Debug.Log("Gioco finito");
            }
        }

        private void CheckPickableLeft()
        {
            m_PickablePicked++;

            m_BatterySlider.value = (float)m_PickablePicked / (float)m_PickablesInScene;

            if (m_PickablePicked >= m_PickablesInScene)
            {
                Debug.Log("Gioco finito");
            }
        }
    }

}
