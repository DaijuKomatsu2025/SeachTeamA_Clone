using UnityEngine;

public class VisibleWallWhenAnnihilate : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private AudioClip _appearSound;
    private void Start()
    {
        _wall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            WallSetActive();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("Player")) WallSetActive();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            WallSetActive();
        }
    }

    private void WallSetActive()
    {
        if (GameModeManager.currentMode == GameModeManager.GameMode.Annihilate)
        {
            if (!_wall.activeSelf)
            {
                _wall.SetActive(true);
                OnEffect();
            }
        }
        else
        {
            if (_wall.activeSelf)
            {
                _wall.SetActive(false);
                OnEffect();
            }
        }
    }

    private void OnEffect()
    {
        if (_appearSound != null)
        {
            AudioSource.PlayClipAtPoint(_appearSound, this.transform.position);
        }
    }
}
