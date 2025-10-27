using UnityEngine;

public class HintMessage : MonoBehaviour
{
    [SerializeField] private Canvas messageWindowCanvas;
    [SerializeField] private string[] _hintText;
    private string _currentMsg = "未設定";

    private void Start()
    {
        if (messageWindowCanvas != null)//初期値でメッセージウィンドウキャンバスの初期値をFalseに
        {
            messageWindowCanvas.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(_currentMsg);
        ShowMessageWindow(_currentMsg);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ヒント表示オフ");
        HideMessageWindow(_currentMsg);
    }

    public void SetCurrentMessage(int num)
    {
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

    public void ShowMessageWindow(string _currentMsg)
    {
        if (messageWindowCanvas != null)
        {
            messageWindowCanvas.gameObject.SetActive(true);
        }
    }
    public void HideMessageWindow(string _currentMsg)
    {
        if (messageWindowCanvas != null)
        {
            messageWindowCanvas.gameObject.SetActive(false);
        }
    }

}
