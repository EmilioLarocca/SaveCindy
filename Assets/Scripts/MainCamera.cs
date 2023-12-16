using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    private StudioEventEmitter gameMusic;

    public EventReference musicEvent;

    // The parameter names for controlling music sections
    public string sectionParameter = "LVL";
    public string startParameter = "Start";
    public string deathParameter = "Death";

    void Start()
    {
        gameMusic = GetComponent<StudioEventEmitter>();

        PlayMusic();
    }

    void PlayMusic()
    {
        // Stop any existing music
        StopMusic();

        // Play the music
        gameMusic.EventReference = musicEvent;
        gameMusic.Play();
    }

    void StopMusic()
    {
        // Stop the current music
        if (gameMusic.IsPlaying())
        {
            gameMusic.Stop();
        }
    }

    // Example: Call this method when changing music sections
    public void ChangeMusicSection(float section, float start, float death)
    {
        // Update the parameters for dynamic music transitions
        gameMusic.SetParameter(sectionParameter, section);
        gameMusic.SetParameter(startParameter, start);
        gameMusic.SetParameter(deathParameter, death);

        // Play the updated music event with the new parameters
        PlayMusic();
    }
}
