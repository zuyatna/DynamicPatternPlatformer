using UnityEngine;
using Photon.Pun;

public class RoyalGuard_ThrowingItems : MonoBehaviourPunCallbacks, IPunObservable {

	public static RoyalGuard_ThrowingItems Instance;

	private Transform playerTransform;	
	public GameObject itemPrefab;

	newObjectPools itemPool;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Instance = this;

		itemPool = new newObjectPools(itemPrefab, 1);
		playerTransform = GetComponent<Transform>();		
	}

	// When to call this depends heavily on your game, but whenever you are
	// done with GameObject you should return it to the pool.
	void ReleaseItem(GameObject _item)
	{
		itemPool.ReturnInstance(_item);
	}

	[PunRPC]
	private void RPCDirection_ThrowingItems() {
		GameObject _item = itemPool.GetInstance();

		if(playerTransform.localScale.x > 0)
		{
			_item.transform.position = new Vector3(playerTransform.position.x + 2, transform.position.y, 0);
			_item.GetComponent<Rigidbody2D>().velocity = new Vector3(20, 0, 0);			
		}
		else
		{
			_item.transform.position = new Vector3(playerTransform.position.x - 2, transform.position.y, 0);
			_item.GetComponent<Rigidbody2D>().velocity = new Vector3(-20, 0, 0);
		}
		
		// ReleaseItem(itemPrefab);
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException();
    }
}
