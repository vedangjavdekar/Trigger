using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    Transform m_OutPortal;
    [SerializeField]
    Transform m_OutputPosition;

    [SerializeField]
    float m_VelMultiplier = 1.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = (m_OutputPosition.position);
        Vector2 vel = m_VelMultiplier * m_OutPortal.right;
        collision.attachedRigidbody.velocity = vel;
    }
}
