using System;
using UnityEngine;
using UnityEngine.UI;

public class DetectPlatformer : MonoBehaviour
{
    public CameraMove cameraMove;
    public Text timerToSpawnTxt;
    public GameObject items;

    private float _timerLeft = 15f;
    private bool _doSpawn = false;

    private void Update()
    {
        if (_doSpawn == false && cameraMove.isCameraMove == true)
        {
            _timerLeft -= Time.deltaTime;
            timerToSpawnTxt.text = "0:" +(_timerLeft).ToString("0");
            if (_timerLeft < 0)
            {
                _doSpawn = true;
                _timerLeft = 15f;
                timerToSpawnTxt.text = "0:" +_timerLeft.ToString("0");
            }   
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platformer") && _doSpawn)
        {
            var position = other.gameObject.transform.position;
            Debug.Log(position.ToString());
            _doSpawn = false;

            Instantiate(items,
                new Vector3(position.x, position.y + 2, 0), Quaternion.identity);
        }
    }
}
