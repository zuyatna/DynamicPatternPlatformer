using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class rooms : MonoBehaviour {
	public GameObject readygame;
	public GameObject gameisready; 

	public GameObject readytogame;	
	public GameObject cancelready;
	public void BacktoMenu()
	{
		SceneManager.LoadSceneAsync("GameMenu");
	}

	public void ReadyToGame() {
		gameisready.gameObject.SetActive(true);
		readygame.gameObject.SetActive(false);
	}

	public void ReadyGame()
	{
		readytogame.gameObject.SetActive(false);
		cancelready.gameObject.SetActive(true);
	}

	public void CancelGame()
	{
		cancelready.gameObject.SetActive(false);
		readytogame.gameObject.SetActive(true);
	}
}
