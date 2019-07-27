using UnityEngine;

public class CustomMatch : MonoBehaviour
{
    public GameObject customMatch;
    private bool _isActiveCustomMatch = false;

    // Update is called once per frame
    void Update()
    {
        OnClickCustomMatch(_isActiveCustomMatch);
    }

    public void OnClickCustomMatch(bool isActive)
    {
        _isActiveCustomMatch = isActive;
        customMatch.SetActive(isActive);
    }
}
