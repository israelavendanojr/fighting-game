using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioSourceOnComplete : MonoBehaviour
{
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!source.isPlaying)
            Destroy(gameObject);
    }
}