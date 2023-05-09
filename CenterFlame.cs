using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    bool musicStart = false;

    public string bgmName = "";
    
    public void ResetMusic()
    {
        musicStart = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            {
                AudioManager.instance.PlayBGM(bgmName);
            if (collision.CompareTag("Note"))
                musicStart = true;
            }
        }
    }
}
