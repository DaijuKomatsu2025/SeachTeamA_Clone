using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    private int _num;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            _num++;
            Debug.Log("hit:" + _num);
            if (_num >= 5) _wall.SetActive(false);
        }
    }
}
