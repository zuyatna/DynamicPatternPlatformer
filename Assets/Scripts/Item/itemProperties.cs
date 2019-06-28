using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemProperties : MonoBehaviour {

	public static itemProperties Instance;

	private Rigidbody2D thisBody;	

	public Animator m_anim;

	[HideInInspector] public bool anim;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Instance = this;
		thisBody = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player") {
			thisBody.velocity = new Vector3(0, 0, 0);
			Destroy(this.gameObject);
		}
	}

	// public void AlertBoomEnded(string _message) {
	// 	if(_message.Equals("BoomAnimEnded")) {
	// 		Destroy(this.gameObject);
	// 	}
	// }
}
