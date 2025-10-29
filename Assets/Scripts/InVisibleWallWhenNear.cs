using UnityEngine;

public class InVisibleWallWhenNear : MonoBehaviour
{
    [SerializeField] private GameObject _wall;


    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player"))) _wall.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player"))) _wall.SetActive(true);
    }
}
