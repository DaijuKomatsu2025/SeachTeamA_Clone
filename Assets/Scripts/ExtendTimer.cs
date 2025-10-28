using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 60f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Time extend!");
        Destroy(gameObject);
    }
}
