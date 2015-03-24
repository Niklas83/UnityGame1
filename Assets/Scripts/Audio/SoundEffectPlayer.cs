using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffectPlayer : MonoBehaviour
{
	public SoundEffects soundEffects;

    private float _timeUntilIdleSound;       //Countdown untill the player will make a idle sound for not being moved in 1 minute + 0
    private bool _idleTimerActive;
    
    private AudioSource _audioSource;        //The audioSource that play steps

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
    
    private void PlaySound(List<AudioClip> audioList, float volume)
    {
    	if (audioList.Count > 0) {
			int index = Random.Range(0, audioList.Count);
			_audioSource.PlayOneShot(audioList[index], volume);
    	}
    }
    
	public void PlayIdleSound()
	{
		PlaySound(soundEffects.idleAudio, soundEffects.idleVolume);
	}
    public void PlayWalkingSound(string groundTag)
    {
        if (groundTag == "Untagged")
        {
            PlaySound(soundEffects.movementAudio, soundEffects.movmentVolume);  //Currently pavement
        }
        else if (groundTag == "GroundGrass")
        {
            PlaySound(soundEffects.movementAudioGrass, soundEffects.movmentVolume);
        }
        else if (groundTag == "GroundSnow")
        {
            PlaySound(soundEffects.movementAudioSnow, soundEffects.movmentVolume);
        }
        else if (groundTag == "GroundSand")
        {
            PlaySound(soundEffects.movementAudioSand, soundEffects.movmentVolume);
        }
        else if (groundTag == "GroundSneakDirt")
        {
            PlaySound(soundEffects.movementAudioSneakDirt, soundEffects.movmentVolume);
        }
        else if (groundTag == "GroundDirt")
        {
            PlaySound(soundEffects.movementAudioDirt, soundEffects.movmentVolume);
        }
        else if (groundTag == "GroundPavemet")
        {
            PlaySound(soundEffects.movementAudio, soundEffects.movmentVolume);  //Currently pavement
        }
    }
    public void PlayAvatarSelectedSound()
    {
		PlaySound(soundEffects.avatarSelectedAudio, soundEffects.avatarSelectVolume);
    }
    public void PlayDeathSound()      
    {
		PlaySound(soundEffects.deathAudio, soundEffects.deathVolume);
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
