using UnityEngine;

public class VisibleWallWhenNear : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    private void Start()
    {
        _wall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player"))) _wall.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player"))) _wall.SetActive(false);
    }
}
