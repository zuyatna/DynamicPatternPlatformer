using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public class dontdestroyobject : MonoBehaviour {
	private AudioSource[] _audio;    

	// Use this for initialization
	void Awake()
	{
		GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
			DontDestroyOnLoad(this.gameObject);
	}

	void Start(){
		
	}

	void Update () 
	{
		
	}
}
