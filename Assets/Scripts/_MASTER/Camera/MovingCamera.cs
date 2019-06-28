using UnityEngine;

public class MovingCamera : MonoBehaviour {
    public float speed;
	public static MovingCamera instance;

	void Awake(){
		instance = this;
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		UpdateCameraTransform();
	}

	void UpdateCameraTransform() {
        transform.Translate(0, Time.deltaTime * speed, 0);        
	}
}