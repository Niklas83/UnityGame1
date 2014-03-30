using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundsEffects : MonoBehaviour {

    //This is a list of audio clips that will play on diffefrent occations and its volume
    public AudioClip MovmentAudio1;
    public AudioClip MovmentAudio2;
    public AudioClip MovmentAudio3;
    public float MovmentVol0To1;
    private List<AudioClip> TheMovmentUAudioClips = new List<AudioClip>();

    public AudioClip CharacterSelectedAudio;
    public float CharacterSelectVol0To1;

    public AudioClip IdleAudio1;
    public AudioClip IdleAudio2;
    public AudioClip IdleAudio3;
    public float IdleVol0To1;
    private List<AudioClip> TheIdleAudioClips = new List<AudioClip>();

    public AudioClip DeathAudio;

    private float TimeUntilIdleSound;       //Countdown untill the player will make a idle sound for not being moved in 1 minute + 0
    private bool IdleTimerActive;

    //Audio sources
    private AudioSource TheAudioSource;        //The audioSource that play steps

	// Use this for initialization
	void Start ()
	{
	    TheAudioSource = this.gameObject.GetComponent<AudioSource>();

	    if (MovmentAudio1 != null)
	    {
	        TheMovmentUAudioClips.Add(MovmentAudio1);
	    }
        if (MovmentAudio2 != null)
        {
            TheMovmentUAudioClips.Add(MovmentAudio2);
        }
        if (MovmentAudio3 != null)
        {
            TheMovmentUAudioClips.Add(MovmentAudio3);
        }

        if (IdleAudio1 != null)
        {
            TheIdleAudioClips.Add(IdleAudio1);
        }
        if (IdleAudio2 != null)
        {
            TheIdleAudioClips.Add(IdleAudio2);
        }
        if (IdleAudio3 != null)
        {
            TheIdleAudioClips.Add(IdleAudio3);
        }


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
        if (TheMovmentUAudioClips != null && (MovmentAudio1 != null || MovmentAudio2 != null || MovmentAudio3 != null))
        {
            int SizeOfMovmentAudioList = TheMovmentUAudioClips.Count;
            int IndexOfRandomAudio = Random.Range(0, SizeOfMovmentAudioList);

            TheAudioSource.PlayOneShot(TheMovmentUAudioClips[IndexOfRandomAudio], MovmentVol0To1);
        }
    }

    public void PlaySelectedCharacterSound()
    {
        if (CharacterSelectedAudio != null)
        {
            TheAudioSource.PlayOneShot(CharacterSelectedAudio, CharacterSelectVol0To1);
        }
    }

    public void PlayIdleSound()
    {
        if (TheIdleAudioClips != null && (IdleAudio1 != null || IdleAudio2 != null || IdleAudio3 != null))
        {
            int SizeOfIdleAudioList = TheIdleAudioClips.Count;
            int IndexOfRandomAudio = Random.Range(0, SizeOfIdleAudioList);

            TheAudioSource.PlayOneShot(TheIdleAudioClips[IndexOfRandomAudio], IdleVol0To1);
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
