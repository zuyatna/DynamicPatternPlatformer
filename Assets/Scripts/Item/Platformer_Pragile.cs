using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer_Pragile : MonoBehaviour {

    private bool Pragile;
    private float timer;

    private void FixedUpdate()
    {
        if (Pragile == true)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                this.gameObject.SetActive(false);
                Pragile = false;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Pragile = true;
            timer = 0.5f;
            Debug.Log("Pragile was " + Pragile);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Pragile = true;
            timer = 0.8f;
            Debug.Log("Pragile was " + Pragile);
        }
    }
}
