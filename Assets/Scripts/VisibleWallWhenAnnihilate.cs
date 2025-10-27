using UnityEngine;

public class VisibleWallWhenAnnihilate : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    private void Start()
    {
        _wall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        WallSetActive();
    }

    private void OnTriggerStay(Collider other)
    {
        WallSetActive();
    }

    private void OnTriggerExit(Collider other)
    {
        WallSetActive();
    }

    private void WallSetActive()
    {
        if (GameModeManager.currentMode == GameModeManager.GameMode.Annihilate)
        {
            _wall.SetActive(true);
        }
        else
        {
            _wall.SetActive(false);
        }
    }
}
