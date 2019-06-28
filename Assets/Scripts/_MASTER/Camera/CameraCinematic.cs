using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraCinematic : MonoBehaviourPunCallbacks, IPunObservable {

	public static CameraCinematic Instance;

	public float Zoom = 7;
    public float Normal = 5;
    public float Smooth = 5;

    //other object
    public Camera _camera;

    public bool isZoom = false;
    public bool camLeft;
    public bool camRight;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(isZoom) {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, Zoom, Smooth * Time.deltaTime);
            Vector3 desairPosition = new Vector3(0, _camera.transform.position.y, -10);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, desairPosition, 0.125f);
        }

        if(camLeft && camRight) {
            isZoom = true;
        }
        else
        {
            isZoom = false;
        }
              
        if(!isZoom) {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, Normal, Smooth * Time.deltaTime);
        }

        if(camLeft && _camera.transform.position.x > -2) {
            Vector3 desairPosition = new Vector3(_camera.transform.position.x - 1.3f, _camera.transform.position.y, -10);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, desairPosition, 0.125f);            
        }
        if(camRight && _camera.transform.position.x < 2) {
            Vector3 desairPosition = new Vector3(_camera.transform.position.x + 1.3f, _camera.transform.position.y, -10);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, desairPosition, 0.125f);
        }
    }    

    private void OnTriggerEnter2D(Collider2D coll)
    { 
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemies")
        {                     
            Debug.Log("Cinematic activated");            
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemies")
        {                      
            Debug.Log("Cinematic disactivated");
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // // We own this player: send the others our data

            stream.SendNext(camLeft);
            stream.SendNext(camRight);
            stream.SendNext(isZoom);
        }
        else
        {
            // // Network player, receive data

            this.camLeft = (bool)stream.ReceiveNext();
            this.camRight = (bool)stream.ReceiveNext();
            this.isZoom = (bool)stream.ReceiveNext();
        }
    }
}
