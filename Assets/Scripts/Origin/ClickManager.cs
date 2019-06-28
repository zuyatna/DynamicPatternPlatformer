using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePose2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit2D = Physics2D.Raycast(mousePose2D, Vector2.zero);
            if (hit2D.collider != null)
            {
                Debug.Log(hit2D.collider.name);
                hit2D.collider.gameObject.SetActive(false);
            }
        }   
    }
}
