using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxbackgrounds : MonoBehaviour {
	public Transform[] backgrounds;
	private float[] parallaxscale;
	public float smoothing;
	private Vector3 previousCameraPosition;

	void Start()
	{
		previousCameraPosition = transform.position;
		parallaxscale = new float[backgrounds.Length];

		for(int i = 0; i< parallaxscale.Length; i++) {
			parallaxscale[i] = backgrounds[i].position.y * -1;
		}

		Debug.Log("for");
	}

	void LateUpdate()
	{
		for(int i = 0; i < parallaxscale.Length; i++)
		{
			Vector3 parallax = (previousCameraPosition - transform.position) * (parallaxscale[i] / smoothing);

			backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);
		}

		previousCameraPosition = transform.position;
	}
}
