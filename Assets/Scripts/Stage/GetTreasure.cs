using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GetTreasure : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private CinemachineCamera _gameclearCamera;
    [SerializeField] private AudioClip _getSound;
    [SerializeField] private ImageAlphaLerper _fadeLerper;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            other.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
            other.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(GameClear());
        }
    }

    IEnumerator GameClear()
    {
        _timer.enabled = false;

        //Debug.Log("お宝ゲット！！！！！！");
        if (_getSound != null)
        {
            AudioSource.PlayClipAtPoint(_getSound, transform.position);
        }
        _gameclearCamera.Priority.Value = 20;
        yield return new WaitForSeconds(2f);

        _fadeLerper.FadeTo(2f, true);

        yield return new WaitForSeconds(1f);
        SceneReloader.Instance.loadGameScene("GameClearScene");
    }
}   
