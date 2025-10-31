using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    //ゲームオーバーボタンのプロパティ
    [SerializeField]private GameObject gameOverButton;
    [SerializeField]private GameObject gameOverText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲームオーバーボタンとテキストを非表示にする
        gameOverButton.SetActive(true);
        gameOverText.SetActive(true);
    }
    
    public void OnGameOver()
    {
        //ゲームオーバーボタンとテキストを表示する
        gameOverButton.SetActive(true);
        gameOverText.SetActive(true);
    }
    //ゲームオーバーボタンをオントリガーでタイトルに戻る
    public void OnGameOverButton()
    {
        //タイトルシーンに戻る
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
