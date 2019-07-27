using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class homeplaymanager : MonoBehaviour {
	public GameObject nowLoading;
	private AsyncOperation operation;

	public void Mulai(){
		nowLoading.gameObject.SetActive(true);
		StartCoroutine(waiting());
	}

	IEnumerator waiting()
	{
		yield return new WaitForSeconds(3f);
		operation = SceneManager.LoadSceneAsync(1);

		while(!operation.isDone) {
			yield return null;
		}
	}
}
