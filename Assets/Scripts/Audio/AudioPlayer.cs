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

	public void FadeOut(float duration) {
		StartCoroutine(Fade(duration));
	}

	private IEnumerator Fade(float duration) {
		float timer = 0;
		while (timer < duration) {
			float t = 1 - (timer / duration);
			mAudio.volume = t;
			timer += Time.deltaTime;
			yield return 0;
		}
	}
}
