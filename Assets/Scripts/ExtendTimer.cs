using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 60f;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            // 時間延長処理

            Debug.Log("Time extend!");
            Destroy(gameObject);
        }
    }
}
