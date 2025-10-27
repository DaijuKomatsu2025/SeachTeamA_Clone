using TMPro;
using UnityEngine;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private GameObject _messageWindow;

    private void Start()
    {
        if (_messageWindow != null)
        {
            _messageWindow.gameObject.SetActive(false);
        }
    }

    public void ShowMessageWindow(string _currentMsg)
    {
        if (_messageWindow != null)
        {
            _textMeshProUGUI.text = _currentMsg;
            _messageWindow.gameObject.SetActive(true);
        }
    }
    public void HideMessageWindow()
    {
        if (_messageWindow != null)
        {
            _textMeshProUGUI.text = "";
            _messageWindow.gameObject.SetActive(false);
        }
    }
}
