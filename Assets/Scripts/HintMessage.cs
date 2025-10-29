using System;
using System.IO;
using UnityEngine;

public class HintMessage : MonoBehaviour
{
    private MessageWindow messageWindow;
    private string[] _hintTexts;

    private string _path = @"Assets\StreamingAssets\hints.csv";

    private string _currentMsg = "未設定";

    private void Awake()
    {
        ReadData();
    }

    private void ReadData()
    {
        try
        {
            _hintTexts = File.ReadAllLines(_path);
        }
        catch (Exception ex)
        {
            Debug.Log($"{_path} 読み込みエラー: {ex.Message}");
        }
    }

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
