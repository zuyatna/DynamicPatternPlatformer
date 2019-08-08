using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject slicedPlatform;
    public float shakeAmount = 0.025f;
    public float speed = 5f;

    private bool isShake = false;

    void Start()
    {
        slicedPlatform.SetActive(false);
    }

    void OnMouseDown()
    {
        if(!isShake)
            isShake = true;
        else
        {
            isShake = false;
            gameObject.SetActive(false);
            slicedPlatform.SetActive(true);
        }
    }

    private void Update()
    {
        if(isShake)
            shakeAnim();
    }

    void shakeAnim()
    {
        Vector3 tempPos = transform.position;
        Vector3 startingPos = transform.position;

        tempPos.x = startingPos.x + Mathf.Sin(Time.time * speed) * shakeAmount;
        transform.position = tempPos;
        //Debug.Log(transform.position.x + ", " + tempPos.x);
    }
}
