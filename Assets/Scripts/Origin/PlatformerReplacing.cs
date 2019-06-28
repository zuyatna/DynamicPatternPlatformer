using UnityEngine;

public class PlatformerReplacing : MonoBehaviour
{
    public static PlatformerReplacing Instance;

    public GameObject newPrefab;
    public GameObject[] oldGameObjects;

    private void Awake()
    {
        Instance = this;
    }

    public void SwapAllByArray()
    {
        foreach (var t in oldGameObjects)
        {
            int random = Random.Range(1, 6);

            if (random == 3)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                SwapPrefabs(t);
            }
        }
    }

    private void SwapPrefabs(GameObject oldGameObject)
    {
        Quaternion rotation = oldGameObject.transform.rotation;
        Vector3 position = oldGameObject.transform.position;

        GameObject newGameObject = Instantiate(newPrefab, position, rotation);

        if (oldGameObject.transform.parent != null)
        {
            newGameObject.transform.SetParent(oldGameObject.transform.parent);
            newGameObject.SetActive(true);
        }

        //DestroyImmediate(oldGameObject);
        oldGameObject.SetActive(false);
    }
}
