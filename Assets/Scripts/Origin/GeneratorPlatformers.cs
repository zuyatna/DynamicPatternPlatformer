using UnityEngine;
using UnityEngine.Serialization;

public class GeneratorPlatformers : MonoBehaviour {

    private static GeneratorPlatformers instance;

    [FormerlySerializedAs("DistanceBetween")] public float distanceBetween;
    [FormerlySerializedAs("GenerationPoint")] public Transform generationPoint;
    public ObjectPoolers[] theObjectPools;
    
    [FormerlySerializedAs("PlatformSelector")] [HideInInspector] public int platformSelector = 0;
    
    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (transform.position.y < generationPoint.position.y)
        {
            PositionTransform();
            PlatformerReplacing.Instance.SwapAllByArray();
        }
    }
    
    private void PositionTransform() {
    
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceBetween, 0);

        if (platformSelector > theObjectPools.Length - 1)
        {
            platformSelector = 0;
        }
        
        Debug.Log("platform: " +platformSelector);
        // Spawn pooling
        GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();        

        newPlatform.transform.position = transform.position;
        newPlatform.transform.rotation = transform.rotation;
        newPlatform.SetActive(true);        

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        platformSelector++;
    }
}