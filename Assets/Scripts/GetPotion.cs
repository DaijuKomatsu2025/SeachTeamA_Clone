using UnityEngine;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private float _healPoint = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            // HP回復処理

            Debug.Log("Heal!");
            Destroy(gameObject);
        }
    }
}   
