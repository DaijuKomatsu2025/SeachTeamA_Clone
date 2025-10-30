using UnityEngine;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private int _healPoint = 10;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            var status = other.gameObject.GetComponent<PlayerController>();
            if (status != null) status.Heal(_healPoint);

            Destroy(gameObject);
        }
    }
}
