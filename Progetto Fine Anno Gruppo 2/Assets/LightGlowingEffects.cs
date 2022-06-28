using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightGlowingEffects : MonoBehaviour
{
    [SerializeField] Light2D m_Light;
    [SerializeField] float m_Speed;
    [Range(0, 10)]
    [SerializeField] float m_MaxIntensity;
    [Range(0, 10)]
    [SerializeField] float m_MinIntensity;
    EDirection direction = EDirection.Up;

    private void Update()
    {
        switch(direction)
        {
            case EDirection.Up:
                Glowing(Mathf.Abs(m_Speed));
                break;
            case EDirection.Down:
                Glowing(-m_Speed);
                break;
        }
    }

    private void Glowing(float increaser)
    {
        m_Light.intensity += increaser * Time.deltaTime;
        if(direction == EDirection.Up && m_Light.intensity >= m_MaxIntensity)
        {
            direction = EDirection.Down;
        }
        else if(direction == EDirection.Down && m_Light.intensity <= m_MinIntensity)
        {
            direction = EDirection.Up;
        }
    }
}
