using UnityEngine;
using Photon.Pun;

public class ItemProperties : MonoBehaviourPunCallbacks, IPunObservable {

	public static ItemProperties Instance;
	[HideInInspector] public Rigidbody2D thisBody;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
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
		if(other.gameObject.tag == "Ground")
		{
			thisBody.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Ground")
		{
			thisBody.constraints = RigidbodyConstraints2D.None;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
//		throw new System.NotImplementedException();
	}
}
