using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GeneratorPlatformer : MonoBehaviour {

    public static GeneratorPlatformer Instance;

    public float DistanceBetween;
    public Transform GenerationPoint;

    public ObjectPooler[] theObjectPools;

    //public GameObject[] Platforms;
    [HideInInspector] public int PlatformSelector;

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
        if (transform.position.y < GenerationPoint.position.y)
        {
            PositionTransform();
        }
    }

    public void PositionTransform() {
    
        transform.position = new Vector3(transform.position.x, transform.position.y + DistanceBetween, 0);

        PlatformSelector = Random.Range(0, theObjectPools.Length);

        // Spawn pooling
        GameObject newPlatform = theObjectPools[PlatformSelector].GetPooledObject();

        newPlatform.transform.position = transform.position;
        newPlatform.transform.rotation = transform.rotation;
        newPlatform.SetActive(true);

        transform.position = new Vector3(transform.position.x, transform.position.y + DistanceBetween, 0);
    }
}