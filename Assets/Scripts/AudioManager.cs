using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool muted;
    [SerializeField] AudioSource mainmenumusic;

    // Start is called before the first frame update
    void Start()
    {
        muted = false;
        mainmenumusic.Play();
    }
}