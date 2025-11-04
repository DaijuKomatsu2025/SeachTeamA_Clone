using UnityEngine;

public class VirtualPadController : MonoBehaviour
{
    public GameObject virtualPadUI;

    void Start()
    {
        // 実行プラットフォームを判定
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android)
        {
            // モバイルなら表示
            virtualPadUI.SetActive(true);
        }
        else
        {
            // PCやエディタなら非表示
            virtualPadUI.SetActive(false);
        }
    }
}
