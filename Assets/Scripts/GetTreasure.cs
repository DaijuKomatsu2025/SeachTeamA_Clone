using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetTreasure : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("お宝ゲット！！！！！！");
        StartCoroutine(GameClear());
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("GameClearScene");
    }
}   
