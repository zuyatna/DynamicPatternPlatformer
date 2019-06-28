using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks {

	public float time; //detik
    private float minutes;
	private float seconds;

	public Text numberText;
	private bool activeTimer;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(LevelManager.Instance.activeTimer)
		{
			minutes = Mathf.Floor(time / 60);
            seconds = Mathf.RoundToInt(time % 60);
            time -= Time.deltaTime;

            numberText.text = minutes.ToString("00") + " : " + seconds.ToString("00");
            if (time <= 0)
            {                
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovingCamera>().enabled = false;               			
                // PhotonNetwork.LeaveRoom();							
            }
		}
	}
}
