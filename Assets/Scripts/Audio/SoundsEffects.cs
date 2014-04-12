using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundsEffects : MonoBehaviour
{
    public bool iceSoundMode;

    //This is a list of audio clips that will play on diffefrent occations and its volume
    public List<AudioClip> regularMovementAudio;
    public List<AudioClip> iceMovementAudio;
    public float movmentVol0To1;

    public List<AudioClip> regularCharacterSelectedAudio;
    public List<AudioClip> iceCharacterSelectedAudio;
    public float characterSelectVol0To1;

    public List<AudioClip> regularIdleAudio;
    public List<AudioClip> iceIdleAudio;
    public float idleVol0To1;
    //private List<AudioClip> TheIdleAudioClips = new List<AudioClip>();

    public List<AudioClip> regularDeathAudio;
    public List<AudioClip> iceDeathAudio;
    public float deathVol0To1;

    private float _timeUntilIdleSound;       //Countdown untill the player will make a idle sound for not being moved in 1 minute + 0
    private bool _idleTimerActive;
    
    private AudioSource _audioSource;        //The audioSource that play steps

	// Use this for initialization
	void Start ()
	{
	    _audioSource = this.gameObject.GetComponent<AudioSource>();
	    _idleTimerActive = true;
	    _timeUntilIdleSound = 10 + Random.Range(0, 20);
	}

    void Update()
    {
        if (_idleTimerActive)
            IdleSoundTimer();
    }
    
    public void IdleSoundTimer()
    {
        _timeUntilIdleSound -= Time.deltaTime;

        if (_timeUntilIdleSound <= 0)
        {
            PlayIdleSound();
            _timeUntilIdleSound = 10 + Random.Range(0, 20);
        }
    }
    
    public void PlayWalkingSound()
    {
        if (iceSoundMode == false)
        {
            if (regularMovementAudio != null && regularMovementAudio.Count > 0)
            {
                int SizeOfMovmentAudioList = regularMovementAudio.Count;
                int IndexOfRandomAudio = Random.Range(0, SizeOfMovmentAudioList);

                _audioSource.PlayOneShot(regularMovementAudio[IndexOfRandomAudio], movmentVol0To1);
            }
        }
        else if (iceSoundMode == true)
        {
            if (iceMovementAudio != null && iceMovementAudio.Count > 0)
            {
                int SizeOfIceMovmentAudioList = iceMovementAudio.Count;
                int IndexOfRandomAudio = Random.Range(0, SizeOfIceMovmentAudioList);

                _audioSource.PlayOneShot(iceMovementAudio[IndexOfRandomAudio], movmentVol0To1);
            }
        }
    }

    public void PlaySelectedCharacterSound()
    {
        if (iceSoundMode == false)
        {
            if (regularCharacterSelectedAudio != null && regularCharacterSelectedAudio.Count > 0)
            {
                int sizeOfCharacterSelectedAudioList = regularCharacterSelectedAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfCharacterSelectedAudioList);

                _audioSource.PlayOneShot(regularCharacterSelectedAudio[indexOfRandomAudio], characterSelectVol0To1);
            }
        }
        else if (iceSoundMode == true)
        {
            if (iceCharacterSelectedAudio != null && iceCharacterSelectedAudio.Count > 0)
            {
                int sizeOfIceCharacterSelectedAudioList = iceCharacterSelectedAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceCharacterSelectedAudioList);

                _audioSource.PlayOneShot(iceCharacterSelectedAudio[indexOfRandomAudio], characterSelectVol0To1);
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

        if (iceSoundMode == false)
        {
            if (regularDeathAudio != null && regularDeathAudio.Count > 0)
            {
                int sizeOfDeathAudioList = regularDeathAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfDeathAudioList);
                randomDeathAudioClip = regularDeathAudio[indexOfRandomAudio];
            }
        }
        else if (iceSoundMode == true)
        {
            if (iceDeathAudio != null && iceDeathAudio.Count > 0)
            {
                int sizeOfIceDeathAudioList = iceDeathAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceDeathAudioList);
                randomDeathAudioClip = iceDeathAudio[indexOfRandomAudio];
            }
        }

        return randomDeathAudioClip;
    }

    public void PlayIdleSound()
    {
        if (iceSoundMode == false)
        {
            if (regularIdleAudio != null && regularIdleAudio.Count > 0)
            {
                int sizeOfIdleAudioList = regularIdleAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIdleAudioList);

                _audioSource.PlayOneShot(regularIdleAudio[indexOfRandomAudio], idleVol0To1);
            }
        }
        else if (iceSoundMode == true)
        {
            if (iceIdleAudio != null && iceIdleAudio.Count > 0)
            {
                int sizeOfIceIdleAudioList = iceIdleAudio.Count;
                int indexOfRandomAudio = Random.Range(0, sizeOfIceIdleAudioList);

                _audioSource.PlayOneShot(iceIdleAudio[indexOfRandomAudio], idleVol0To1);
            }
        }
    }

    public void SetIdleTimeBool(bool ActiveOrDisabled)
    {
        if (_idleTimerActive == false && ActiveOrDisabled == true)
        {
            _timeUntilIdleSound = 60 + Random.Range(0, 60);
            _idleTimerActive = ActiveOrDisabled;
        }
        else if (_idleTimerActive == true && ActiveOrDisabled == false)
        {
            _idleTimerActive = ActiveOrDisabled;
        }
    }

}
