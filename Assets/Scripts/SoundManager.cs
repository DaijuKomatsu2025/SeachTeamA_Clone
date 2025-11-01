using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource _audioSource;

    int maxSoundsCount = 10;
    int currentSoundCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (currentSoundCount >= maxSoundsCount) return;

        AudioSource.PlayClipAtPoint(clip, position);
        currentSoundCount++;

        StartCoroutine(DecreaseSoundCount(clip.length));
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);
    }

    IEnumerator DecreaseSoundCount(float deray)
    {
        yield return new WaitForSeconds(deray);
        currentSoundCount--;
    }
}
