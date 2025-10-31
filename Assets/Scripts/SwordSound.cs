using UnityEngine;

public class SwordSound : MonoBehaviour
{
    [SerializeField] private AudioClip swordSwingClip;

    public void PlaySwordSwingSound()
    {
        AudioSource.PlayClipAtPoint(swordSwingClip, transform.position);
    }
}
