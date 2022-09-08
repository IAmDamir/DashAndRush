using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance = null;

    public static BackgroundMusic Instance
    {
        get { return instance; }
    }

    public AudioSource audio;
    public AudioClip[] myMusic;

    private void Awake()
    {
        audio.clip = myMusic[0] as AudioClip;

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        audio.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!audio.isPlaying)
            playRandomMusic();
    }

    private void playRandomMusic()
    {
        audio.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        audio.Play();
    }
}