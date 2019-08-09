using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Generat : MonoBehaviour {

    public Rigidbody2D Melee_Item_Rigid;    //use rigidbody2d game object
    public Transform SpawnPoint;
    [HideInInspector] public bool Item_Loot;                 //is item generator disactivated? <- see GameManager

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        Melee_Item_Rigid = GetComponent<Rigidbody2D>();
    }	

    public void Melee_Spawn()
    {
        transform.position = new Vector3(transform.position.x, SpawnPoint.position.y, 0);
        this.gameObject.transform.position = transform.position;
        this.gameObject.SetActive(true);
        Melee_Item_Rigid.constraints = RigidbodyConstraints2D.None;
    }
}
