using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Video360 : MonoBehaviour
{
    private bool playerInside;
    private VideoPlayer vd;
    private AudioSource audi;
    public GameObject map;
    public GameObject vd360;

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
        map.SetActive(false);
        vd360.SetActive(true);
        vd.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;
        map.SetActive(true);
        vd360.SetActive(false);
        vd.Stop();
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
