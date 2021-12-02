using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{

    public AudioSource sfx;
    public AudioClip sound;

    public void PlaySound()
    {
        if (sfx.isPlaying)
            return;
        sfx.PlayOneShot(sound);
    }
}
