using Unity.Cinemachine;
using UnityEngine;

public class DisableStopCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_cam != null)
            {
                _cam.Priority = 24;
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (_cam != null)
    //        {
    //            _cam.Priority = 24;
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_cam != null)
            {
                _cam.Priority = 10;
            }
        }
    }
}
