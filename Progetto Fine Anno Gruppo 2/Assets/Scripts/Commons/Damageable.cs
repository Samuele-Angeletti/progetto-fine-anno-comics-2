using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float m_MaxLife;

    [Header("Save File")]
    [SerializeField] string m_FileName;

    private float m_CurrentLife;

    private void Awake()
    {
        m_CurrentLife = m_MaxLife;
    }

    public void SaveData()
    {
        SaveAndLoadSystem.Save(new DamageableInfos(m_MaxLife, m_CurrentLife), m_FileName);
    }

    internal void TakeDamage(float m_DamageAmount)
    {
        m_CurrentLife -= m_DamageAmount;
        if(m_CurrentLife <= 0)
        {
            m_CurrentLife = 0;
            // send message
        }
    }
}
