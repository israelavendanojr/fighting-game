using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour
{
    [SerializeField] SimpleAudioEvent theme;
    void Start()
    {
        if (GameObject.Find("Audio Event: " + theme.name) == null)
        {
            theme.Play();
            GameObject themeGO = GameObject.Find("Audio Event: " + theme.name);
            DontDestroyOnLoad(themeGO);
        }
        else
        {
            Destroy(gameObject);
            return;

        }
    }
}