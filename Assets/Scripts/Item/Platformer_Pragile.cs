using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer_Pragile : MonoBehaviour {

    private bool Pragile;
    private float timer;
    public GameObject obj;

    private void FixedUpdate()
    {
        if (Pragile == true)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                obj.SetActive(false);
                Pragile = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Armature")
        {
            Pragile = true;
            timer = 0.5f;
            Debug.Log("Pragile was " + Pragile);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Armature")
        {
            Pragile = true;
            timer = 0.8f;
            Debug.Log("Pragile was " + Pragile);
        }
    }
}
