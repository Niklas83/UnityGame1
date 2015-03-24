using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffects : MonoBehaviour
{
	public List<AudioClip> idleAudio;
	public float idleVolume = 1.0f;
	
    public List<AudioClip> movementAudio;
    public List<AudioClip> movementAudioGrass;
    public List<AudioClip> movementAudioSnow;
    public List<AudioClip> movementAudioSand;
    public List<AudioClip> movementAudioDirt;
    public List<AudioClip> movementAudioSneakDirt;
    public float movmentVolume = 1.0f;

    public List<AudioClip> avatarSelectedAudio;
	public float avatarSelectVolume = 1.0f;

    public List<AudioClip> deathAudio;
	public float deathVolume = 1.0f;
}
