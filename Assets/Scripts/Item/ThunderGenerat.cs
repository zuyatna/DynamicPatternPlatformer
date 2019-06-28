using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGenerat : MonoBehaviour {

	public static ThunderGenerat Instance;

	private SpriteRenderer thunderSprite;
	private Animator thunderAnim;

	[HideInInspector] public bool isThunder;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;
		thunderSprite = this.gameObject.GetComponent<SpriteRenderer>();
		thunderAnim = this.gameObject.GetComponent<Animator>();
	}	

	public void AlertThunderEnd(string _message) {
		if(_message.Equals("ThunderAnimEnded")) {
			// isThunder = true;
			thunderSprite.enabled = false;
			thunderAnim.enabled = false;
		}
	}
}
