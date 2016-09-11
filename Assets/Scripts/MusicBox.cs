using UnityEngine;
using System.Collections;

public class MusicBox : MonoBehaviour {
	public enum MusicTone {Title = 0,
				Calm = 1,
				Storm = 2};
	private FMOD.Studio.EventInstance songEvt;

	public static MusicBox instance;

	public MusicTone weatherGameplaySong;

	void Awake() {
		instance = this;
		songEvt = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
	}

	// Use this for initialization
	void Start () {
		songEvt.start();
		SetMusicTone(weatherGameplaySong);
	}

	public void ResumeGameplayWeatherMusic() {
		SetMusicTone(weatherGameplaySong);
	}

	public void SetMusicTone(MusicTone newTone) {
		// Debug.Log("Changing music to: " + newTone);

		if(newTone != MusicTone.Title) {
			weatherGameplaySong = newTone;
		}

		switch(newTone) {
		case MusicTone.Title:
			songEvt.setParameterValue("Start", 0.0f);
			break;
		case MusicTone.Calm:
			songEvt.setParameterValue("Start", 1.0f);
			break;
		case MusicTone.Storm:
			songEvt.setParameterValue("Start", 2.0f);
			break;
		}
	}
}
