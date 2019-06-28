using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations;

public class animsbtn : MonoBehaviour {
	public Animator myAnim;	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BtnCustome(string animscustome) {
		if(animscustome.Equals("animationsEnded"))
		{
			myAnim.SetBool("CustomeIsMoveBtn", true);
			// myAnim.SetTrigger("CustomeIsMove");
		}
	}	

	public void btncustomeismove(string animscustome) {
		if(animscustome.Equals("animationsisEnded"))
		{
			myAnim.SetTrigger("moveidle");
			myAnim.SetBool("ShakeShopBtn", false);
			myAnim.SetBool("ShakeArmoryBtn", false);
			// myAnim.SetTrigger("CustomeIsMove");
		}
	}	
}
