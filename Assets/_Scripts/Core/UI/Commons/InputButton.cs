using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputButton : MonoBehaviour
{
    [SerializeField]
    protected RectTransform m_Button;
    
    [SerializeField]
    protected TextMeshProUGUI m_ButtonText;

    private object m_TweenID;
    protected TweenerCore<Vector3, Vector3, VectorOptions> GetTween()
    {
        DOTween.Kill(m_TweenID);
        var tween = m_Button?.DOScale(Vector3.one, 0.2f)
            .From(0.5f * Vector3.one)
            .SetEase(Ease.InOutBack);
        m_TweenID = tween.id;
        return tween;
    }

    protected virtual void OnInteraction(InputAction.CallbackContext context)
    {
        GetTween();
    }
}
