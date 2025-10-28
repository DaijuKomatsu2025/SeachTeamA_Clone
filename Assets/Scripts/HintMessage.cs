using UnityEngine;

public class HintMessage : MonoBehaviour
{
    private MessageWindow messageWindow;
    [SerializeField] private string[] _hintText;
    private string _currentMsg = "未設定";

    private void OnTriggerEnter(Collider other)
    {
        messageWindow.ShowMessageWindow (_currentMsg);
    }
    private void OnTriggerExit(Collider other)
    {
        messageWindow.HideMessageWindow();
    }

    public void SetCurrentMessage(int num, MessageWindow messageCanvas)
    {
        this.messageWindow = messageCanvas;

        if (_hintText.Length >= num)
        {
            _currentMsg = _hintText[num];
        }
        else
        {
            _currentMsg = "？？？";
            Debug.Log("引数がメッセージ配列の要素数をオーバーしています");
        }
    }
}
