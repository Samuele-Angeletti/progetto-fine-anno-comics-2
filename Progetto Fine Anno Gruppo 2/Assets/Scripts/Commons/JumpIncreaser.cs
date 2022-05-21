using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class JumpIncreaser : MonoBehaviour
{
    [SerializeField] float m_NewJumpHeight;

    private float m_OldJumpHeight;

    public void StoreJumpHegiht(float jumpHeght)
    {
        m_OldJumpHeight = jumpHeght;
    }

    public float GetOldJumpHeght()
    {
        return m_OldJumpHeight;
    }

    public float GetNewJumpHeght()
    {
        return m_NewJumpHeight;
    }
}
