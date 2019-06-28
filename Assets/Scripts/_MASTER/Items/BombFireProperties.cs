using UnityEngine;

public class BombFireProperties : MonoBehaviour {

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
