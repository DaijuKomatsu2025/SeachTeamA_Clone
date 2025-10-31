using UnityEngine;

public class InVisibleWallWhenNear : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private ParticleSystem _appearEffect;
    [SerializeField] private AudioClip _appearSound;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            OnEffect();
            _wall.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            OnEffect();
            _wall.SetActive(true);
        }
    }

    private void OnEffect()
    {
        if (_appearEffect != null)
        {
            var effect = Instantiate(_appearEffect, _wall.transform.position, Quaternion.identity);
            effect.transform.localScale = Vector3.one * 0.7f;
            effect.Play();
        }
        if (_appearSound != null)
        {
            AudioSource.PlayClipAtPoint(_appearSound, this.transform.position);
        }
    }
}
