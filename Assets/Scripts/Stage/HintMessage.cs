using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HintMessage : MonoBehaviour
{
    [SerializeField] private MessageWindow messageWindow;
    private static string[] _hintTexts;

    [SerializeField] private string _currentMsg = "未設定";

    private void Awake()
    {
        if (_hintTexts == null) ReadData();
    }

    private void ReadData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("hints"); // 拡張子不要
        _hintTexts = csvFile.text.Split('\n');
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            messageWindow.ShowMessageWindow(_currentMsg);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            messageWindow.HideMessageWindow();
        }
    }

    public void SetCurrentMessage(int num, MessageWindow messageCanvas)
    {
        this.messageWindow = messageCanvas;

        if (_hintTexts.Length >= num)
        {
            _currentMsg = _hintTexts[num];
        }
        else
        {
            _currentMsg = "？？？";
            Debug.Log("引数がメッセージ配列の要素数をオーバーしています");
        }
    }
}
