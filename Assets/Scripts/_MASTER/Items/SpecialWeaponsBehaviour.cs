using Photon.Pun;
using UnityEngine;

public class SpecialWeaponsBehaviour : MonoBehaviour, IPunObservable
{

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "RGShield")
        {
            Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
//        throw new System.NotImplementedException();
    }
}
