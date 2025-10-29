using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 60f;
    [SerializeField] private Timer _timer;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            _timer.RecoverTimes();
            Destroy(gameObject);
        }
    }
}
