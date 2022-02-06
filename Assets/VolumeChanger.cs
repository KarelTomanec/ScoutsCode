using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeChanger : MonoBehaviour {
    public AudioMixer audioMixer;

    public void SetMusic(float soundLevel)
    {
        audioMixer.SetFloat("MusicVol", soundLevel);
    }

    public void SetSFX(float soundLevel)
    {
        audioMixer.SetFloat("SFXVol", soundLevel);
    }
}
