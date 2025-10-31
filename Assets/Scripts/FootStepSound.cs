using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip footstepClip;

    public void PlayFootstepSound()
    {
        AudioSource.PlayClipAtPoint(footstepClip, transform.position);
    }
}
