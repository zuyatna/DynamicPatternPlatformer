using UnityEngine;

public class GeneratorPlatformer : MonoBehaviour {

    public static GeneratorPlatformer Instance;

    public float DistanceBetween;
    public Transform GenerationPoint;
    public ObjectPooler[] theObjectPools;
    
    [HideInInspector] public int PlatformSelector;

    void Awake()
    {
        Instance = this;
    }

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

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}