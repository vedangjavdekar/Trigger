using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    Transform m_ReachTarget = null;
    [SerializeField]
    ParticleSystem m_Particles;

    [SerializeField]
    AudioClip m_Audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.gameStateManager.OverrideState(GameState.GoalReached);
            if (m_ReachTarget)
            {
                if(collision.gameObject.TryGetComponent(out PlayerBall playerBall))
                {
                    float tweenDuration = 0.5f;
                    DOVirtual.DelayedCall(tweenDuration, () =>
                    {
                        GameManager.Instance.soundManager.PlaySound(m_Audio);
                        m_Particles.Play();

                    });
                    playerBall.OnReachedGoal(m_ReachTarget,tweenDuration);
                }
            }
            else
            {
                Debug.LogError("Reach Target Not Set");
            }
        }
    }
}
