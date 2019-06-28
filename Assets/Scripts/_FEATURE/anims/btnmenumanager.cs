using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class btnmenumanager : MonoBehaviour {

	public Animator myAnim;
	public GameObject soon;

	private AsyncOperation operation;

	public void ShakeShop()
	{
		myAnim.SetBool("ShakeShopBtn", true);
		soon.gameObject.SetActive(true);
		StartCoroutine(waiting());
	}

	public void ShakeArmory()
	{
		myAnim.SetBool("ShakeArmoryBtn", true);
		soon.gameObject.SetActive(true);
		StartCoroutine(waiting());
	}

	IEnumerator waiting()
	{
		yield return new WaitForSeconds(1f);
		soon.gameObject.SetActive(false);
		// while(!operation.isDone) {
		// 	yield return null;
		// }
	}
}
