using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private bool playerInside;
    private VideoPlayer vd;
    private AudioSource audi;

    private void Awake()
    {
        playerInside = false;
        vd = GetComponent<VideoPlayer>();
        audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        controlVideo();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInside = true;
        vd.Play();
        other.gameObject.GetComponent<AudioSource>().mute = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;
        vd.Stop();
        other.gameObject.GetComponent<AudioSource>().mute = false;
    }

    private void controlVideo()
    {
        if (!playerInside)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (vd.isPaused)
            {
                vd.Play();
            }
            else
            {
                vd.Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            audi.mute = !audi.mute;
        }

    }
}
