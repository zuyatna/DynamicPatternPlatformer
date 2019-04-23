using UnityEngine;

public class PlatformDestroyer : MonoBehaviour {

    public GameObject destroyPoint;

	// Use this for initialization
	void Start () {
        destroyPoint = GameObject.Find ("DestroyPoint");
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < destroyPoint.transform.position.y)
        {
            gameObject.SetActive(false);
        }
	}
}
