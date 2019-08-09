using UnityEngine;
using Photon.Pun;

public class RoyalGuard_ThrowingShield : MonoBehaviourPunCallbacks, IPunObservable
{
	
	private Transform playerTransform;	
	public GameObject itemPrefab;

	newObjectPools itemPool;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
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
	private void RPCDirection_ThrowingShield() {
		GameObject _item = itemPool.GetInstance();

		if(playerTransform.localScale.x > 0)
		{
			_item.transform.position = new Vector3(playerTransform.position.x + 2, transform.position.y, 0);			
			_item.GetComponent<Rigidbody2D>().velocity = new Vector3(10, 0, 0);
			_item.GetComponent<Rigidbody2D>().AddTorque(-300, ForceMode2D.Force);
		}
		else
		{			
			_item.transform.localScale = new Vector3(_item.transform.localScale.x, -_item.transform.localScale.y, _item.transform.localScale.z);						
			_item.transform.position = new Vector3(playerTransform.position.x - 2, transform.position.y, 0);			
			_item.GetComponent<Rigidbody2D>().velocity = new Vector3(-10, 0, 0);
			_item.GetComponent<Rigidbody2D>().AddTorque(300, ForceMode2D.Force);			
		}			
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
