using UnityEngine;
using Photon.Pun;

public class ItemsBehaviour : MonoBehaviourPunCallbacks {

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Items")
        {
            Destroy(gameObject);
        }
    }
}
