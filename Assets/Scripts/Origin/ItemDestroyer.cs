﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    public GameObject destroyPoint;

    // Use this for initialization
    void Start () {
        destroyPoint = GameObject.Find ("DestroyPoint");
    }
	
    // Update is called once per frame
    void Update () {
        if (transform.position.y < destroyPoint.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }
}
