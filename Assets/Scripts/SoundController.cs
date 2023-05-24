using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Sprite spriteSound, spriteMute;
    private Image buttnImage;
    private bool isMuted;

    public void Awake()
    {
        isMuted = false;
        buttnImage = GetComponent<Image>();
    }

    public void ChangeMuteState()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            buttnImage.sprite = spriteMute;
            AudioListener.volume = 0;
        }
        else
        {
            buttnImage.sprite = spriteSound;
            AudioListener.volume = 1;
        }
    }
}
