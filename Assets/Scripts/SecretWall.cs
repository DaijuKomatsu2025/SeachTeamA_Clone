using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    private int _num;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            _num++;
            //Debug.Log("hit:" + _num);
            if (_num >= 3) _wall.SetActive(false);
        }
    }
}
