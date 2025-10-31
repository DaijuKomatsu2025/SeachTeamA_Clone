using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetTreasure : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _gameclearCamera;
    [SerializeField] private AudioClip _getSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            //Debug.Log("お宝ゲット！！！！！！");
            if (_getSound != null)
            {
                AudioSource.PlayClipAtPoint(_getSound, transform.position);
            }
            _gameclearCamera.Priority.Value = 20;
            StartCoroutine(GameClear());
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(3f);
        SceneReloader.Instance.loadGameScene("GameClearScene");
    }
}   
