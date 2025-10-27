using UnityEngine;

public class HintMessage : MonoBehaviour
{
    [SerializeField] private string[] _hintText;
    private string _currentMsg = "未設定";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(_currentMsg);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ヒント表示オフ");
    }

    public void SetCurrentMessage(int num)
    {
        if(_hintText.Length >= num)
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
