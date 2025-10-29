using UnityEngine;

public class GetPotion : MonoBehaviour
{
    [SerializeField] private float _healPoint = 10f;
    [SerializeField] private CommonStatus _status;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            // HP回復処理
            //_status.Heal(_healPoint);
            Destroy(gameObject);
        }
    }
}   
