using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    #region Inspector Varaibles

    [SerializeField] AudioSource[] audioSources;

    #endregion

    public void PlayGunshotSound()
    {
        audioSources[2].Play();
    }

    public void PlayIntroMusic()
    {

    }
}
