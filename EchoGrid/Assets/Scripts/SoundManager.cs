﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource[] efxSource;					//Drag a reference to the audio source which will play the sound effects.
	public AudioSource voiceSource;
	public static SoundManager instance = null;		//Allows other scripts to call functions from SoundManager.				
	int max_sfx_playing = 5;

	void Awake ()
	{
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
		
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		//efxSource = new AudioSource[max_sfx_playing];
		//for (int i = 0; i < max_sfx_playing; ++i) {
		//	efxSource[i] = new AudioSource();
		//}
		//voiceSource = new AudioSource();
		//voiceSource = GetComponent<AudioSource>();
		DontDestroyOnLoad (gameObject);
	}
	
	
	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		for (int i = 0; i < max_sfx_playing; ++i) {
			if (!efxSource [i].isPlaying) {
				efxSource[i].clip = clip;
				//Play the clip.
				efxSource[i].Play ();
				return;
			}
		}
	}

	public bool PlayVoice(AudioClip clip, bool reset = false)
	{
		if ( (!voiceSource.isPlaying)||reset ) {
			voiceSource.clip = clip;
			//Play the clip.
			voiceSource.Play ();
			return true;
		}

		return false;
	}
	
}
