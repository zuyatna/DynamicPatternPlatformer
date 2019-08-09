using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerat : MonoBehaviour {

	private Rigidbody2D thisBody;
	public Transform itemLootSpawn;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		thisBody = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Ground") {
			thisBody.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	public void SpawnLoot() {
		transform.position = new Vector3(transform.position.x, itemLootSpawn.position.y, 0);
        this.gameObject.transform.position = transform.position;
        this.gameObject.SetActive(true); 
		thisBody.constraints = RigidbodyConstraints2D.None;       
	}
}
