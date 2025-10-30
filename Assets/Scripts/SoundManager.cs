using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    int maxSoundsCount = 10;
    int currentSoundCount = 0;

    void Awake()
    {
        if (instance is null) 
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

     public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (currentSoundCount >= maxSoundsCount) return;

        AudioSource.PlayClipAtPoint(clip, position);
        currentSoundCount++;

        StartCoroutine(DecreaseSoundCount(clip.length));
    }

    IEnumerator DecreaseSoundCount(float deray)
    {
        yield return new WaitForSeconds(deray);
        currentSoundCount--;
    }
}
