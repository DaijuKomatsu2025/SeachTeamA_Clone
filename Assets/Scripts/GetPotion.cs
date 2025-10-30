using UnityEngine;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private int _healPoint = 10;
    [SerializeField] private AudioClip _getSound;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
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
