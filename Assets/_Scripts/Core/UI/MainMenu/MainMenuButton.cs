using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButton : MenuItemBase
{
    readonly Vector2 m_DefaultWidth = new Vector2(250, 120);
    readonly Vector2 m_DefaultHighlightWidth = new Vector2(300, 120);
    readonly Color m_FocusColor = new Color(1.0f, 0.707154f, 0.409434f);

    Button m_Button;
    Image m_BG;

    public override void OnBlur()
    {
        var rectTransform = transform as RectTransform;
        rectTransform.DOSizeDelta(m_DefaultWidth, 0.3f).SetEase(Ease.InOutCubic);
        m_BG.DOColor(Color.white, 0.25f).SetEase(Ease.InOutCubic);
    }

    public override void OnFocus()
    {
        var rectTransform = transform as RectTransform;
        rectTransform.DOSizeDelta(m_DefaultHighlightWidth, 0.3f).SetEase(Ease.InOutCubic);
        m_BG.DOColor(m_FocusColor, 0.25f).SetEase(Ease.InOutCubic);
    }

    public override void OnSelect()
    {
        var rectTransform = transform as RectTransform;
        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOSizeDelta(m_DefaultWidth, 0.3f).SetEase(Ease.InOutCubic));
        sequence.Join(m_BG.DOColor(Color.white, 0.25f).SetEase(Ease.InOutCubic));
        sequence.AppendCallback(() => { m_Button.onClick.Invoke(); });
    }

    private void EnforceNormalState()
    {
        var rectTransform = transform as RectTransform;
        rectTransform.sizeDelta = m_DefaultWidth;
        m_BG.color = Color.white;
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_Button = GetComponent<Button>();
        m_BG = GetComponent<Image>();
    }

}
