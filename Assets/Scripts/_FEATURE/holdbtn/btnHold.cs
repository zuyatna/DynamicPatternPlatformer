using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class btnHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool pointerDown;
	private float pointerDownTimer;

	[SerializeField]
	private float requireHoldTime;

	public UnityEvent onLongClick;
	
	[SerializeField]
	private Image fillImage;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		pointerDown = true;
		Debug.Log("OnPointerDown");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Reset();
		Debug.Log("OnPointerUp");
	}
	void Update () 
	{
		if(pointerDown)
		{
			pointerDownTimer += Time.deltaTime;
			if(pointerDownTimer >= requireHoldTime)
			{
				if(onLongClick != null)
					onLongClick.Invoke();

				Reset();
			}
			fillImage.fillAmount = pointerDownTimer / requireHoldTime;
		}
	}

	void Reset() {
		pointerDown = false;
		pointerDownTimer = 0;
		fillImage.fillAmount = pointerDownTimer / requireHoldTime;
	}
}
