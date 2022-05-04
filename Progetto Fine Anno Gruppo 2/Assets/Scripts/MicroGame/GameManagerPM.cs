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

        internal void UIUpdateLife(int m_CurrentLifes)
        {
            m_LifeText.text = "VITE: " + m_CurrentLifes;
        }

        private void Start()
        {
            PubSub.PubSub.Subscribe(this, typeof(PickablePickedMessage));
            PubSub.PubSub.Subscribe(this, typeof(GameOverMicroGameMessage));
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
