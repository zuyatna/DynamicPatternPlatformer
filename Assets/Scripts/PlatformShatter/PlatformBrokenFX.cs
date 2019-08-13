using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBrokenFX : MonoBehaviour
{
    public float alphaLevel = 1f;
    public GameObject platNormal;

    Dictionary<Transform, Vector3> pos = new Dictionary<Transform, Vector3>();

    void Start()
    {
        SetPositions();
    }

    void Update()
    {
        if(alphaLevel <= 0.1f)
        {
            Debug.Log("Position Restored!");
            RestorePositions();
            alphaLevel = 1;
            gameObject.SetActive(false);
            platNormal.SetActive(true);
        }

        alphaLevel -= 0.4f * Time.deltaTime;

        for(int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaLevel);
        
        //Debug.Log(alphaLevel);
    }

    void SetPositions()
    {
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            pos.Add(t, t.localPosition);
            Debug.Log(t.name + " : " + t.localPosition.x);
        }
    }

    void RestorePositions()
    {
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if(pos.ContainsKey(t) == true)
            {
                t.localPosition = pos[t];
            }
            else
            {
                Debug.Log("Transform not found!");
            }
        }
    }
}
