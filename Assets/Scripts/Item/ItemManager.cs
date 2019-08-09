using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	public static ItemManager Instance;

	[Header("Thunder")]
	public Transform transformThunder;
	public List<GameObject>	thunder;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;
	}

	// /// <summary>
	// /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	// /// </summary>
	// void FixedUpdate()
	// {
	// 	if(ThunderGenerat.Instance.isThunder) {
	// 		for(int i = 0; i < 4; i++) {
	// 			thunder[i].enabled = false;
	// 		}
	// 		ThunderGenerat.Instance.isThunder = false;
	// 	}
	// }

	// spawn thunder manager
	public void SpawnThunder() {
		for(int i = 0; i < 3; i++) {
			thunder[i].GetComponent<SpriteRenderer>().enabled = true;
			thunder[i].GetComponent<Animator>().enabled = true;
			thunder[i].transform.position = new Vector3(Random.Range(-8, 8), transformThunder.position.y, 0);
		}
	}
}
