using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPooler : MonoBehaviour {

    private GameObject[] Pooler;
    private int index;

    //timer
    private float _timer = 30f;

    public Transform GenerationPoint;

    //temp
    private int _temp;

    // Use this for initialization
    void Start () {
        Pooler = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Pooler[i] = transform.GetChild(i).gameObject;
        }

        foreach (GameObject go in Pooler)
        {
            go.SetActive(false);
        }

        //random obj
        var random_obj = Random.Range(0, Pooler.Length);
        _temp = random_obj;

        if (Pooler[_temp])
        {
            Pooler[_temp].SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            Pooler[_temp].SetActive(false);

            //transform.position = new Vector3(transform.position.x, GenerationPoint.position.y, 0);

            //random obj
            var random_obj = Random.Range(0, Pooler.Length);
            _temp = random_obj;

            Pooler[_temp].SetActive(true);
            //Pooler[_temp].transform.position = transform.position;

            //transform.position = new Vector3(transform.position.x, GenerationPoint.position.y, 0);

            _timer = 30f;
        }
	}
}
