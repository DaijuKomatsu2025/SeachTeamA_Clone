using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 30f;
    [SerializeField] private Timer _timer;

    private void Start()
    {
        if (_timer == null)
        {
            _timer = GameObject.Find("TimerText").GetComponent<Timer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            if (_timer != null) _timer.RecoverTimes(_extendTime);

            Destroy(gameObject);
        }
    }
}
