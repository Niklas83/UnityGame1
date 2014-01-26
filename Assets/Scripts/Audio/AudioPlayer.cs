using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour 
{
	private AudioSource mAudio;

	public void Start() {
		mAudio = this.GetComponent<AudioSource>();
	}
}
