using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetTreasure : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("お宝ゲット！！！！！！");
        _camera.Priority.Value = 20;
        StartCoroutine(GameClear());
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("GameClearScene");
    }
}   
