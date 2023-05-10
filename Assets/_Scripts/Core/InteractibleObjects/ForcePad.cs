using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class ForcePad : MonoBehaviour
{
    [SerializeField]
    private float m_ForceMultiplier = 1.0f;

    [SerializeField]
    private AudioClip m_Audio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.soundManager.PlaySound(m_Audio);
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(m_ForceMultiplier * rb.velocity);
    }
}
