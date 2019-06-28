using System.Collections.Generic;
using UnityEngine;

public class newObjectPools {

	private GameObject prefabs;

	private List<GameObject> pool;

	public newObjectPools(GameObject _prefabs, int _initialSize)
	{
		this.prefabs = _prefabs;

		this.pool = new List<GameObject>();
		for(int i = 0; i < _initialSize; i++) {
			AllocateInstance();
		}
	}	

	public GameObject GetInstance() {
		if(pool.Count == 0) {
			AllocateInstance();
		}

		int _lastIndex = pool.Count - 1;
		GameObject _instance = pool[_lastIndex];
		pool.RemoveAt(_lastIndex);
		_instance.SetActive(true);
	
		return _instance;
	}

	public void ReturnInstance(GameObject _instance) {
		_instance.SetActive(false);
		pool.Add(_instance);
	}

	protected virtual GameObject AllocateInstance() {
		GameObject _instance = (GameObject) GameObject.Instantiate(prefabs);
		_instance.SetActive(false);
		pool.Add(_instance);

		return _instance;
	}
}