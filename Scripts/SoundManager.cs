using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[Header("Audio Clips")]
	public AudioClip mainMenuSound;
	public AudioClip gamePlaySound,buttonClick,levelFailed,levelComplete;
	[Header("Audio Sources")]
	[SerializeField]
	private AudioSource musicSource;
	[SerializeField]
	private AudioSource sfxSource,levelFail_CompleteSource;
	[Header("Audio Listener")]
	[SerializeField]
	AudioListener CurrentAudioListener;

	public static SoundManager _SoundManager;

	void Awake()
	{
		if (_SoundManager == null) {
			_SoundManager = this;
		} 
		else {
			Destroy (this.gameObject);	
		}
		DontDestroyOnLoad (this.gameObject);
		verifyAudioSources ();
	}

	public void sfxVolume()
	{
		sfxSource.volume = PlayerPrefs.GetFloat ("SFXVol",1);
	}

	public void changeVolume()
	{
		AudioListener.volume = PlayerPrefs.GetFloat ("Vol",1);
	}


	void verifyAudioSources()
	{
		musicSource.playOnAwake = false;
		musicSource.loop = true;
		sfxSource.playOnAwake = false;
		sfxSource.loop = false;
		levelFail_CompleteSource.playOnAwake = false;
		levelFail_CompleteSource.loop = false;
	}

	public void playMainMenuSounds()
	{
		musicSource.clip = mainMenuSound;
		musicSource.Play ();
	}

	public void playMainMenuSounds(float temp)
	{
		musicSource.clip = mainMenuSound;
		musicSource.Play ();
		musicSource.volume = temp;
	}

	public void playGameplaySounds()
	{
		musicSource.clip = gamePlaySound;
		musicSource.Play ();
	}

	public void playGameplaySounds(float temp)
	{
		musicSource.clip = gamePlaySound;
		musicSource.Play ();
		musicSource.volume = temp;
	}

	public void playButtonClickSound()
	{
		sfxSource.clip = buttonClick;
		sfxSource.Play ();
	}

	public void playLevelFailedSound()
	{
		levelFail_CompleteSource.clip = levelFailed;
		levelFail_CompleteSource.Play ();
	}
	public void playLevelCompleteSound()
	{
		levelFail_CompleteSource.clip = levelComplete;
		levelFail_CompleteSource.Play ();
	}
}
