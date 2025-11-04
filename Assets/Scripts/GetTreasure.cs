using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GetTreasure : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private CinemachineCamera _gameclearCamera;
    [SerializeField] private AudioClip _getSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            _timer.enabled = false;

            //Debug.Log("お宝ゲット！！！！！！");
            if (_getSound != null)
            {
                AudioSource.PlayClipAtPoint(_getSound, transform.position);
            }
            _gameclearCamera.Priority.Value = 20;
            other.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
            other.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(GameClear());
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("ゲームクリアシーンを呼ぶ");
        SceneReloader.Instance.ReloadSceneClean("GameClearScene");
    }
}   
