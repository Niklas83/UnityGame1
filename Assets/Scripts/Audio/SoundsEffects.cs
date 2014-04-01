using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundsEffects : MonoBehaviour
{
    public bool IceSoundMode;

    //This is a list of audio clips that will play on diffefrent occations and its volume
    public List<AudioClip> RegularMovementAudio;
    public List<AudioClip> IceMovementAudio;
    public float MovmentVol0To1;

    public List<AudioClip> RegularCharacterSelectedAudio;
    public List<AudioClip> IceCharacterSelectedAudio;
    public float CharacterSelectVol0To1;

    public List<AudioClip> RegularIdleAudio;
    public List<AudioClip> IceIdleAudio;
    public float IdleVol0To1;
    //private List<AudioClip> TheIdleAudioClips = new List<AudioClip>();

    public List<AudioClip> RegularDeathAudio;
    public List<AudioClip> IceDeathAudio;
    public float DeathVol0To1;

    private float TimeUntilIdleSound;       //Countdown untill the player will make a idle sound for not being moved in 1 minute + 0
    private bool IdleTimerActive;



    //Audio sources
    private AudioSource TheAudioSource;        //The audioSource that play steps

	// Use this for initialization
	void Start ()
	{
	    TheAudioSource = this.gameObject.GetComponent<AudioSource>();

	    IdleTimerActive = true;

	    TimeUntilIdleSound = 10 + Random.Range(0, 20);

	}

    void Update()
    {
        if (IdleTimerActive)
        {
            IdleSoundTimer();
        }
    }


    public void IdleSoundTimer()
    {
        TimeUntilIdleSound -= Time.deltaTime;

        if (TimeUntilIdleSound <= 0)
        {
            PlayIdleSound();
            TimeUntilIdleSound = 10 + Random.Range(0, 20);
        }
    }
    

    public void PlayWalkingSound()
    {
        if (IceSoundMode == false)
        {
            if (RegularMovementAudio != null && RegularMovementAudio.Count > 0)
            {
                int SizeOfMovmentAudioList = RegularMovementAudio.Count;
                int IndexOfRandomAudio = Random.Range(0, SizeOfMovmentAudioList);

                TheAudioSource.PlayOneShot(RegularMovementAudio[IndexOfRandomAudio], MovmentVol0To1);
            }
        }
        else if (IceSoundMode == true)
        {
            if (IceMovementAudio != null && IceMovementAudio.Count > 0)
            {
                int SizeOfIceMovmentAudioList = IceMovementAudio.Count;
                int IndexOfRandomAudio = Random.Range(0, SizeOfIceMovmentAudioList);

                TheAudioSource.PlayOneShot(IceMovementAudio[IndexOfRandomAudio], MovmentVol0To1);
            }
        }
    }

    public void PlaySelectedCharacterSound()
    {
        if (IceSoundMode == false)
        {
            if (RegularCharacterSelectedAudio != null && RegularCharacterSelectedAudio.Count > 0)
            {
                int sizeOfCharacterSelectedAudioList = RegularCharacterSelectedAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfCharacterSelectedAudioList);

                TheAudioSource.PlayOneShot(RegularCharacterSelectedAudio[indexOfRandomAudio], CharacterSelectVol0To1);
            }
        }
        else if (IceSoundMode == true)
        {
            if (IceCharacterSelectedAudio != null && IceCharacterSelectedAudio.Count > 0)
            {
                int sizeOfIceCharacterSelectedAudioList = IceCharacterSelectedAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceCharacterSelectedAudioList);

                TheAudioSource.PlayOneShot(IceCharacterSelectedAudio[indexOfRandomAudio], CharacterSelectVol0To1);
            }
        }
    }

    //Currently not in use, as the death sound gets droped on the floor due to player gameobuject being removed on death
    //public void PlayDeathSound()      
    //{
    //    if (RegularDeathAudio != null && RegularDeathAudio.Count > 0)
    //    {
    //        int sizeOfDeathAudioList = RegularDeathAudio.Count;
    //        int indexOfRandomAudio = Random.Range(0, sizeOfDeathAudioList);

    //        TheAudioSource.PlayOneShot(RegularDeathAudio[indexOfRandomAudio], DeathVol0To1);
    //    }
    //}

    public AudioClip GetRandomDeathAudioClip()
    {
        AudioClip randomDeathAudioClip = null;

        if (IceSoundMode == false)
        {
            if (RegularDeathAudio != null && RegularDeathAudio.Count > 0)
            {
                int sizeOfDeathAudioList = RegularDeathAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfDeathAudioList);
                randomDeathAudioClip = RegularDeathAudio[indexOfRandomAudio];
            }
        }
        else if (IceSoundMode == true)
        {
            if (IceDeathAudio != null && IceDeathAudio.Count > 0)
            {
                int sizeOfIceDeathAudioList = IceDeathAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceDeathAudioList);
                randomDeathAudioClip = IceDeathAudio[indexOfRandomAudio];
            }
        }

        return randomDeathAudioClip;
    }

    public void PlayIdleSound()
    {
        if (IceSoundMode == false)
        {
            if (RegularIdleAudio != null && RegularIdleAudio.Count > 0)
            {
                int sizeOfIdleAudioList = RegularIdleAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIdleAudioList);

                TheAudioSource.PlayOneShot(RegularIdleAudio[indexOfRandomAudio], IdleVol0To1);
            }
        }
        else if (IceSoundMode == true)
        {
            if (IceIdleAudio != null && IceIdleAudio.Count > 0)
            {
                int sizeOfIceIdleAudioList = IceIdleAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceIdleAudioList);

                TheAudioSource.PlayOneShot(IceIdleAudio[indexOfRandomAudio], IdleVol0To1);
            }
        }
    }

    public void SetIdleTimeBool(bool ActiveOrDisabled)
    {
        if (IdleTimerActive == false && ActiveOrDisabled == true)
        {
            TimeUntilIdleSound = 60 + Random.Range(0, 60);
            IdleTimerActive = ActiveOrDisabled;
        }
        else if (IdleTimerActive == true && ActiveOrDisabled == false)
        {
            IdleTimerActive = ActiveOrDisabled;
        }
    }

}
