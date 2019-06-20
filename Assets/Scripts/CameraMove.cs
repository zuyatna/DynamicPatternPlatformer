using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Text playBtn;
    [HideInInspector] public bool isCameraMove = false;

    private Vector3 _velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (isCameraMove)
        {
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }
    }

    public void MoveCameraBtn()
    {
        if (isCameraMove == false)
        {
            isCameraMove = true;
            playBtn.text = "Restart";
        }
        else
        {
            isCameraMove = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
