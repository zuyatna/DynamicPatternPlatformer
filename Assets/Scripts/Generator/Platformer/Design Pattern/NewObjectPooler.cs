using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPooler : MonoBehaviour {

	#region public variables

	[Tooltip("Object will pooled")]
	public GameObject pooledObject;

	[Tooltip("How much amount you want to pooled")]
	public int pooledAmount;

	#endregion

	#region private varibles

	private List<GameObject> objectsPooling;

	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		objectsPooling = new List<GameObject>();

		for(int i = 0; i < pooledAmount; i++) {
			GameObject _obj = (GameObject)Instantiate(pooledObject);
			_obj.SetActive(true);
			objectsPooling.Add(_obj);
		}
	}
}
