using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private int _healPoint = 10;
    [SerializeField] private AudioClip _getSound;
    [SerializeField] private ParticleSystem _getEffect;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            if (_getEffect != null)
            {
                var effect = Instantiate(_getEffect, this.transform.position, Quaternion.Euler(-90f, 0f, 0f));
                effect.Play();
            }
            if (_getSound != null)
            {
                AudioSource.PlayClipAtPoint(_getSound, transform.position);
            }
            var status = other.gameObject.GetComponent<PlayerController>();
            if (status != null) status.Heal(_healPoint);

            Destroy(gameObject);
        }
    }
}
