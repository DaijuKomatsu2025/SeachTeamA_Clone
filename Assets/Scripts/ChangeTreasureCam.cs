using UnityEngine;
using Unity.Cinemachine;

public class ChangeTreasureCam : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _treasureCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_treasureCam != null)
            {
                _treasureCam.Priority = 24;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_treasureCam != null)
            {
                _treasureCam.Priority = 1;
            }
        }
    }
}
