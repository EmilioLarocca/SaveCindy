using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManagerUI : MonoBehaviour
{
    public static AudioManagerUI instance;
    
    [SerializeField]
    private EventReference uiClick;

    [SerializeField]
    private EventReference uiPause;

    void Awake()
    {
        instance = this;
    }

    public void PlayUiClick()
    {
        RuntimeManager.PlayOneShot(uiClick);
    }

    public void PlayUiPause()
    {
        RuntimeManager.PlayOneShot(uiPause);
    }
}
