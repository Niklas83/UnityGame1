using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioTrack : MonoBehaviour
{
	public List<AudioClip> intros;
	public List<AudioClip> clips;
	public List<AudioClip> outros;
	
	public Vector2 nrLoopsRange;
	public Vector2 randomTimeRange;
	
	private AudioSource _introSource;
	private AudioSource _mainSource;
	private AudioSource _outroSource;
	
	void Start() {
		
	}

	public void Setup ()
	{
		_introSource = gameObject.AddComponent<AudioSource>();
		_mainSource = gameObject.AddComponent<AudioSource>();
		_outroSource = gameObject.AddComponent<AudioSource>();
	}
	
	public float Play()
	{
		float delay = 0;
		if (intros.Count > 0) {
			int index = Random.Range(0, intros.Count);
			AudioClip iclip = intros[index];
			_introSource.clip = iclip;
			_introSource.Play();
			delay = iclip.length;
		}
		
		AudioClip clip = clips[Random.Range(0, clips.Count)];
		_mainSource.clip = clip;
		_mainSource.PlayDelayed(delay);
		_mainSource.loop = true;
		_mainSource.volume = 1;
		
		float duration = 0;
		int nrLoops = Random.Range((int)nrLoopsRange.x, (int)nrLoopsRange.y);
		if (nrLoops > 0) {
			duration = (nrLoops * clip.length) + delay;
			StartCoroutine(TurnOffLooping(duration - 0.5f, _mainSource));
		} else {
			duration = Random.Range(randomTimeRange.x, randomTimeRange.y) + delay;
			StartCoroutine(FadeOut(duration - 0.5f, 0.5f, _mainSource));
		}
		
		if (outros.Count > 0) {
			AudioClip oclip = outros[Random.Range(0, outros.Count)];
			_outroSource.clip = oclip;
			_outroSource.PlayDelayed(duration);
			duration += oclip.length;
		}
		
		return duration;
	}
	
	public void FadeOut(float fadeDuration) {
		StartCoroutine(FadeOut(0, fadeDuration, _introSource));
		StartCoroutine(FadeOut(0, fadeDuration, _mainSource));
		StartCoroutine(FadeOut(0, fadeDuration, _outroSource));
	}
	
	private IEnumerator TurnOffLooping(float delay, AudioSource source) {
		yield return new WaitForSeconds(delay);
		source.loop = false;
	}
	
	
	private IEnumerator FadeOut(float delay, float fadeDuration, AudioSource source) {
		yield return new WaitForSeconds(delay);
		
		float startVolume = source.volume;
		float timer = 0;
		while (timer < fadeDuration) {
			float t = timer / fadeDuration;
			source.volume = Mathf.Lerp(startVolume, 0, t);
			timer += Time.deltaTime;
			yield return 0;
		}
		source.volume = 0;
	}
}
