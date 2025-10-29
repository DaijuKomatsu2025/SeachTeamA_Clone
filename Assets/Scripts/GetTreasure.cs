using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetTreasure : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _gameclearCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            Debug.Log("お宝ゲット！！！！！！");
            _gameclearCamera.Priority.Value = 20;
            StartCoroutine(GameClear());
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("GameClearScene");
    }
}   
