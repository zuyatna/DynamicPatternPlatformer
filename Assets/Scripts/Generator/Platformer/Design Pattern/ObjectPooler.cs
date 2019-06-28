using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    #region Public Variables

    public GameObject PooledObject;
    public int PooledAmount;

    #endregion

    #region Private Variables

    List<GameObject> PooledObjects;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        PooledObjects = new List<GameObject>();

        // for (int i = 0; i < PooledAmount; i++)
        // {
        //     GameObject obj = (GameObject)Instantiate(PooledObject);
        //     obj.SetActive(false);
        //     PooledObjects.Add(obj);
        // }
    }

    #endregion

    #region MonoBehaviour Callbacks

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }

        GameObject obj = (GameObject)Instantiate(PooledObject);
        obj.SetActive(false);
        PooledObjects.Add(obj);
        return obj;
    }

    #endregion
}
