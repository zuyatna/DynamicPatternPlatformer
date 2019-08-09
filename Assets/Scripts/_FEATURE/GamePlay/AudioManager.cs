using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private AudioSource[] audioSource;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayGameplayAudio(){
		audioSource = GameObject.Find("MusicManager").GetComponents<AudioSource>();
		audioSource[0].Stop();
		audioSource[1].Play();
	}

	public void PlayHomeSceneAudio(){
		audioSource = GameObject.Find("MusicManager").GetComponents<AudioSource>();
		audioSource[1].Stop();
		audioSource[0].Play();
	}
}
