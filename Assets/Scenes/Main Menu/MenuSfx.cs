using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalClickSfx : MonoBehaviour
{
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioClip clickSfx;

    private bool isMouseDown = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isMouseDown)
            {
                isMouseDown = true;
                PlaySfx();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
    }

    private void PlaySfx()
    {
        if (sfxPlayer != null && clickSfx != null)
        {
            sfxPlayer.PlayOneShot(clickSfx);
        }
    }
}
