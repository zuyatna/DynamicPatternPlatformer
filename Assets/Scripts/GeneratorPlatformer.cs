using UnityEngine;
using UnityEngine.Serialization;

public class GeneratorPlatformer : MonoBehaviour {

    private static GeneratorPlatformer instance;

    [FormerlySerializedAs("DistanceBetween")] public float distanceBetween;
    [FormerlySerializedAs("GenerationPoint")] public Transform generationPoint;
    public ObjectPooler[] theObjectPools;
    
    [FormerlySerializedAs("PlatformSelector")] [HideInInspector] public int platformSelector;

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

        platformSelector = Random.Range(0, theObjectPools.Length);

        // Spawn pooling
        GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();        

        newPlatform.transform.position = transform.position;
        newPlatform.transform.rotation = transform.rotation;
        newPlatform.SetActive(true);        

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}