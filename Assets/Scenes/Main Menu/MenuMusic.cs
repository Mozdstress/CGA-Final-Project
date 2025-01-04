using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMusic : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    private bool isMusicPlaying = true;

    private void Start()
    {
        UpdateButton();
    }

    public void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            music.Stop();
            isMusicPlaying = false;
        }
        else
        {
            music.Play();
            isMusicPlaying = true;
        }

        UpdateButton();
    }

    private void UpdateButton()
    {
        if (isMusicPlaying)
        {
            toggleButton.image.sprite = musicOnSprite;
        }
        else
        {
            toggleButton.image.sprite = musicOffSprite;
        }
    }
}
