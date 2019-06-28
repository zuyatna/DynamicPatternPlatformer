using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class menumanager : MonoBehaviour {
	public GameObject customegame;
	public GameObject findgame;

	public GameObject roomgame;
	
	public GameObject createcustomegame;
	public GameObject settings;

	//setting audio music in settings
	public AudioMixer audioMixer;

	//animator
	public Animator myAnim;

	//ready game
	public GameObject readytogame;
	public GameObject readygame;

	public RectTransform objRect;

	public void customeGame() {
		customegame.gameObject.SetActive(true);
		myAnim.SetBool("IsMoveBtn", true);
		// myAnim.SetTrigger("CustomeIsMove");
		// myAnim.SetBool("CustomeIsMoveBtn", true);
	}

	public void findGame() {
		findgame.gameObject.SetActive(true);
	}

	public void findroom(){
		roomgame.gameObject.SetActive(true);
	}
	public void createcustomeGame() {
		createcustomegame.gameObject.SetActive(true);
		customegame.gameObject.SetActive(false);
	}

	public void settinggame() {
		settings.gameObject.SetActive(true);
	}

	public void setVolume(float volume) {
		audioMixer.SetFloat("volume", volume);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) {
			myAnim.SetBool("IsMoveBtn", false);
			myAnim.SetBool("CustomeIsMoveBtn", false);
			customegame.gameObject.SetActive(false);
			createcustomegame.gameObject.SetActive(false);
			roomgame.gameObject.SetActive(false);
			settings.gameObject.SetActive(false);			
		}
	}
	public void backmenu() {		
		myAnim.SetBool("CustomeIsMoveBtn", false);
		myAnim.SetBool("IsMoveBtn", false);
		objRect.localPosition = new Vector2(-525, 197);
		customegame.gameObject.SetActive(false);
		findgame.gameObject.SetActive(false);
		createcustomegame.gameObject.SetActive(false);
		roomgame.gameObject.SetActive(false);
		settings.gameObject.SetActive(false);		
	}

	public void ready(){
		readytogame.gameObject.SetActive(true);
		readygame.gameObject.SetActive(false);
	}

	public void creategame() {
		SceneManager.LoadSceneAsync("Room");
		
	}

	public void unready(){
		readygame.gameObject.SetActive(true);
		readytogame.gameObject.SetActive(false);
	}
}
