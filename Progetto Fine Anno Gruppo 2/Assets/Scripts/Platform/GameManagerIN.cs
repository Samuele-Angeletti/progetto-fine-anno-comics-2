using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;

namespace MainGame
{
    [RequireComponent(typeof(PlayerInputSystem))]
    public class GameManagerIN : MonoBehaviour
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
            m_PlayerInputs.SetControllable(m_Controllable);
        }
    }
}
