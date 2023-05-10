using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InitialVelocity : MonoBehaviour
{
    Rigidbody2D m_RigidBody = null;

    [SerializeField]
    Vector2 m_InitialVelocity = Vector2.zero;

    [SerializeField, Range(1, 10)]
    float m_VelocityMultiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();

        m_RigidBody.AddForce(m_VelocityMultiplier * m_InitialVelocity, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, m_VelocityMultiplier * m_InitialVelocity);
        }
    }

}
