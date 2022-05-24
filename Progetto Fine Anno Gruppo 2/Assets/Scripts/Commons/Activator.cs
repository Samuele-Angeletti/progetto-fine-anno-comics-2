using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
[RequireComponent(typeof(PolygonCollider2D))]
public class Activator : MonoBehaviour
{
    PolygonCollider2D m_Collider2D;

    private void Awake()
    {
        m_Collider2D = GetComponent<PolygonCollider2D>();
        m_Collider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovementManager>() != null)
            ActiveObjects();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovementManager>() != null)
            DeactiveObjects();
    }

    public void ActiveObjects()
    {
        PubSub.PubSub.Publish(new ActivableMessage(true, this));
    }

    public void DeactiveObjects()
    {
        PubSub.PubSub.Publish(new ActivableMessage(false, this));
    }
}
