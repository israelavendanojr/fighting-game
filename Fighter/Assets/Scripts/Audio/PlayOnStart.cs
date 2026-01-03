using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnStart : MonoBehaviour
{
    [SerializeField] SimpleAudioEvent _sound;
    void Start()
    {
        _sound.Play();
    }

}
