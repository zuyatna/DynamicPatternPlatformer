using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class DetectPlatformerOnline : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject items;
    
    private bool _doSpawn = false;
    private float _timerLeft = 15f;
    
    private void Update()
    {
        if (_doSpawn == false)
        {
            _timerLeft -= Time.deltaTime;
            if (_timerLeft < 0)
            {
                _doSpawn = true;
                _timerLeft = 15f;
            }   
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && _doSpawn)
        {
            var position = other.gameObject.transform.position;
            Debug.Log(position.ToString());
            _doSpawn = false;

            PhotonNetwork.Instantiate(this.items.name, new Vector3(position.x, position.y + 2, 0), Quaternion.identity);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
//        throw new System.NotImplementedException();
    }
}
