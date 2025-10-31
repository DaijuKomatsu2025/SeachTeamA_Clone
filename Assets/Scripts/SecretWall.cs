using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _breakSound;
    [SerializeField] private ParticleSystem _breakEffect;
    private int _num;
    private bool _isBroken;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isBroken && other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            var pos = transform.position - Vector3.left * 1.3f;

            if (_hitSound != null)
            {
                AudioSource.PlayClipAtPoint(_hitSound, pos);
            }

            if (_hitEffect != null)
            {
                var effect = Instantiate(_hitEffect, pos, Quaternion.Euler(-90f, 0f, 0f));
                effect.Play();
            }

            _num++;

            //Debug.Log("hit:" + _num);
            if (_num >= 3)
            {
                if (_breakEffect != null)
                {
                    var eff = Instantiate(_breakEffect, _wall.transform.position, Quaternion.identity);
                    eff.transform.localScale = Vector3.one * 0.5f;
                    eff.Play();
                }
                if (_breakSound != null)
                {
                    AudioSource.PlayClipAtPoint(_breakSound, transform.position);
                }
                _wall.SetActive(false);
                _isBroken = true;
            }
        }
    }
}
