using UnityEngine;
using Photon.Pun;

public class ItemsBehaviour : MonoBehaviourPunCallbacks, IPunObservable {

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Items")
        {
            Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
//        throw new System.NotImplementedException();
    }
}
