using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlowingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] float m_Speed;
    Color m_Original;
    EDirection m_Direction = EDirection.Down;
    private void Start()
    {
        m_Original = m_Text.color;
    }

    private void Update()
    {
        switch(m_Direction)
        {
            case EDirection.Up:
                ChangeColor(Mathf.Abs(m_Speed));
                break;
            case EDirection.Down:
                ChangeColor(-m_Speed);
                break;
        }
    }

    private void ChangeColor(float speed)
    {
        m_Original.a += speed * Time.deltaTime;
        m_Text.color = m_Original;
        if(m_Original.a >= 1 && m_Direction == EDirection.Up)
        {
            m_Direction = EDirection.Down;
        }
        else if(m_Original.a <= 0 && m_Direction == EDirection.Down)
        {
            m_Direction = EDirection.Up;
        }
    }
}
