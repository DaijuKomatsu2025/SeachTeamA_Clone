using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 30f;
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioClip _getSound;

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
            if (_getSound != null)
            {
                AudioSource.PlayClipAtPoint(_getSound, transform.position);
            }
            if (_timer != null) _timer.RecoverTimes(_extendTime);

            Destroy(gameObject);
        }
    }
}
