using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Music : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EventInstance gameStatusInstance;

    public EventReference lvlMusic;
    public EventReference gameStatus;

    [Range(0f, 3f)] public int toLevel;
    [Range(0f, 2f)] public int completed;
    [Range(0f, 1f)] public int toStart;
    [Range(0f, 1f)] public int toDeath;

    public int gamePaused; 

    public void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(lvlMusic);
        musicInstance.start();

        gameStatusInstance = RuntimeManager.CreateInstance(gameStatus);
        gameStatusInstance.start();
    }

    public void Update ()
    {
        musicInstance.setParameterByName("LVL", toLevel);
        musicInstance.setParameterByName("Completed", completed);
        musicInstance.setParameterByName("Start", toStart);
        musicInstance.setParameterByName("Death", toDeath);

        gameStatusInstance.setParameterByName("GamePaused", gamePaused);
    }
}
