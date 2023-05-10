using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class URLButton : MonoBehaviour
{
    Button m_Button;

    Button button
    {
        get
        {
            if(!m_Button)
            {
                m_Button= GetComponent<Button>();
            }
            return m_Button;
        }
    }

    public string LinkURL;

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Application.OpenURL(LinkURL);
    }
}
