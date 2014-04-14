using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
DONE ------- 1. Bestämma vilka "musikvarianter" som ska vara med, t.ex endast snippets. Direkt i advanced audioplayer
DONE ------- 2. Volymregel under varje track (inte en generell för tex snippets) 
DONE ------- 3. Bestämma enviroments fade out tid
DONE ------- 4. Bestämma enviroments fade in tid
DONE ------- 5. Bestämma när den ska börja fade in. Enklast skulle vara om jag bara kan säga tex -13 s för att enviroments ska börja sin fade in i slutet på loopen/outro
DONE ------- 6. Kan byta ut enviroments mot total tystnad?? Eventuellt
DONE ------- 7. Bocka i en ruta om jag vill att enviroments ska låta hela tiden...även bakom musiken (Det innebär att även den måste kunna loopa).
8. Game Over musik som bryter all musik direkt
*/

public class AudioPlayerAdvanced : MonoBehaviour 
{
	public List<TrackInfo> tracks;
		
	void Start() 
	{
		DebugAux.Assert(tracks.Count > 0, "[AudioPlayerAdvanced] There is no tracks specified!");
		
		foreach (TrackInfo info in tracks) {
			AudioTrack track = Helper.Instansiate<AudioTrack>(info.audioTrackPrefab.gameObject, this.gameObject);
			info.track = track;
			track.Setup();
		}
		
		NextStep(tracks[0]); // Always start with the first track
	}
	
	public void NextStep(TrackInfo trackInfo) 
	{
		float duration = trackInfo.track.Play();
		TrackInfo nextTrack = PickATrack(trackInfo);
		duration += nextTrack.startTime;
		Debug.Log("[AudioPlayerAdvanced] Starting track, " + duration + " seconds until next.");
		StartCoroutine(MakeNextDecisionIn(duration, nextTrack));
	}
	
	public IEnumerator MakeNextDecisionIn(float delay, TrackInfo nextTrack) 
	{
		yield return new WaitForSeconds(delay);
		NextStep(nextTrack);
	}
	
	public TrackInfo PickATrack(TrackInfo excludedInfo) 
	{
		float r = Random.value;
		float p = 0;
		float totalWeight = 0;
		foreach (TrackInfo info in tracks) {
			if (info != excludedInfo)
				totalWeight += info.weight;
		}
		
		if (totalWeight == 0)
			return PickATrack(null);
		
		foreach (TrackInfo info in tracks) {
			if (info == excludedInfo)
				continue;
				
			float probability = info.weight / totalWeight;
			p += probability;
			// Debug.Log(r + " "+ p +" "+ info + " " + info.weight + " " + totalWeight);
			if (r < p) {
				return info;
			}
		}
		DebugAux.Assert(false, "Found no track to play!?");
		return null;
	}
	
	public void FadeOut(float fadeDuration)
	{
		foreach (TrackInfo info in tracks) {
			info.track.FadeOut(fadeDuration);
		}
	}
}

[System.Serializable]
public class TrackInfo
{
	public AudioTrack audioTrackPrefab;
	public float weight = 1;
	public float startTime = 0;
	
	public AudioTrack track { get; set; }
}