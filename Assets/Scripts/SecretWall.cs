using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private ParticleSystem _hitEffect;
    private int _num;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            var pos = transform.position - Vector3.left * 1.3f;
            var effect = Instantiate(_hitEffect, pos, Quaternion.identity);
            effect.Play();
            _num++;
            //Debug.Log("hit:" + _num);
            if (_num >= 3) _wall.SetActive(false);
        }
    }
}
