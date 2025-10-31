using UnityEngine;

public class ExtendTimer : MonoBehaviour
{
    [SerializeField] private float _extendTime = 30f;
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioClip _getSound;
    [SerializeField] private ParticleSystem _getEffect;

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
            if (_getEffect != null)
            {
                var effect = Instantiate(_getEffect, other.transform.position, Quaternion.Euler(-90f, 0f, 0f));
                effect.transform.SetParent(other.transform);
                effect.Play();
            }

            if (_getSound != null)
            {
                AudioSource.PlayClipAtPoint(_getSound, transform.position);
            }
            if (_timer != null) _timer.RecoverTimes(_extendTime);

            Destroy(gameObject);
        }
    }
}
