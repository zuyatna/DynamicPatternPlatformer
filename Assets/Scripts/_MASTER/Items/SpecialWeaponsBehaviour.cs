using UnityEngine;

public class SpecialWeaponsBehaviour : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "RGShield")
        {
            Destroy(gameObject);
        }
    }
}
