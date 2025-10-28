using UnityEngine;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private float _healPoint = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Heal!");
        Destroy(gameObject);
    }
}   
