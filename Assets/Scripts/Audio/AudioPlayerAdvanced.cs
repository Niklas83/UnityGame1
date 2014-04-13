using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioState
{
	Environment,
	Music,
	Snippet,
}

public class AudioPlayerAdvanced : MonoBehaviour 
{	
	Dictionary<int, BaseState> _allStates;
	StateMachine _stateMachine;
	
	public AudioTrack enviromentTrack;
	public AudioTrack musicTrack;
	public AudioTrack snippetTrack;
		
	void Start() 
	{
		_stateMachine = new StateMachine();
		
		enviromentTrack = Helper.Instansiate<AudioTrack>(enviromentTrack.gameObject, this.gameObject);
		musicTrack = Helper.Instansiate<AudioTrack>(musicTrack.gameObject, this.gameObject);
		snippetTrack = Helper.Instansiate<AudioTrack>(snippetTrack.gameObject, this.gameObject);
		
		enviromentTrack.Setup();
		musicTrack.Setup();
		snippetTrack.Setup();
		
		_allStates = new Dictionary<int, BaseState>();
		_allStates[(int) AudioState.Environment] = new AudioEnvironment(this.gameObject, _stateMachine, enviromentTrack);
		_allStates[(int) AudioState.Music] = new AudioMusic(this.gameObject, _stateMachine, musicTrack);
		_allStates[(int) AudioState.Snippet] = new AudioSnippet(this.gameObject, _stateMachine, snippetTrack);
		
		_stateMachine.Setup((int) AudioState.Environment, _allStates);
	}
	
	public void DelayedStateChange(AudioBaseState state, float duration, AudioState nextState) 
	{
		StartCoroutine(state.DelayedStateChange(duration, nextState));
	}

	public void FadeOut(float fadeDuration)
	{
		enviromentTrack.FadeOut(fadeDuration);
		musicTrack.FadeOut(fadeDuration);
		snippetTrack.FadeOut(fadeDuration);
	}
}
	
public abstract class AudioBaseState : BaseState
{
	StateMachine _stateMachine;
	protected AudioPlayerAdvanced audioPlayer;
	protected AudioSource audioSource;
	protected AudioTrack track;
	
	public AudioBaseState(GameObject player, StateMachine stateMachine, AudioTrack track) {
		_stateMachine = stateMachine;
		audioPlayer = player.GetComponent<AudioPlayerAdvanced>();
		audioSource = player.AddComponent<AudioSource>();
		audioSource.loop = true;
		this.track = track;
	}
	
	public IEnumerator DelayedStateChange(float delay, AudioState state) 
	{
		yield return new WaitForSeconds(delay);
		ChangeState(state);
	}
	
	public void ChangeState(AudioState state) 
	{
		_stateMachine.ChangeState((int) state);
	}
	
	public abstract void OnEnter();
	public abstract void Update();
	public abstract void OnExit();
}

public class AudioEnvironment : AudioBaseState
{
	public AudioEnvironment(GameObject player, StateMachine stateMachine, AudioTrack track) : base(player, stateMachine, track) {
	}
	public override void OnEnter() {
		float duration = track.Play();
		AudioState nextState = Random.value < 0.5f ? AudioState.Music : AudioState.Snippet;
		Debug.Log("[Environment Track] Starting " + nextState + " in " + duration + " seconds.");
		audioPlayer.DelayedStateChange(this, duration, nextState);
	}
	public override void Update() {}
	public override void OnExit() {}
}
public class AudioMusic : AudioBaseState
{
	public AudioMusic(GameObject player, StateMachine stateMachine, AudioTrack track) : base(player, stateMachine, track) {
	}
	public override void OnEnter() {
		float duration = track.Play();
		Debug.Log("[Music Track] Starting Environment track in " + duration + " seconds.");
		audioPlayer.DelayedStateChange(this, duration, AudioState.Environment);
	}
	public override void Update() {}
	public override void OnExit() {}
}
public class AudioSnippet : AudioBaseState
{
	public AudioSnippet(GameObject player, StateMachine stateMachine, AudioTrack track) : base(player, stateMachine, track) {
	}
	public override void OnEnter() {
		float duration = track.Play();
		Debug.Log("[Snippet Track] Starting Environment track in " + duration + " seconds.");
		audioPlayer.DelayedStateChange(this, duration, AudioState.Environment);
	}
	public override void Update() {}
	public override void OnExit() {}
}
