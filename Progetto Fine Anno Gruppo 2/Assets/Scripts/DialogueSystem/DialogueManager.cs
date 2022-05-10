using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager m_instance;
    public static DialogueManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DialogueManager>();
                if (m_instance == null)
                {
                    m_instance = new GameObject().AddComponent<DialogueManager>();
                }
            }
            return m_instance;
        }
       
    }
    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(this);
            DontDestroyOnLoad(m_instance);
        }
    }
    

}
