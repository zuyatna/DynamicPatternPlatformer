using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCanvas : MonoBehaviourPunCallbacks {	

	public GameObject player;
    private Camera cameraWorld;
	[SerializeField] private GameObject PlayerPanel;
	[SerializeField] private GameObject PlayerControlCanvas;

    private Vector3 playerPosition;
    private RectTransform rt;
    private RectTransform canvasRT;
    private Vector3 playerScreenPos;

    public RoyalGuard_Control playerControl;
    private Vector3 m_LocalScale;

    private GameObject repoCamera;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		
		playerPosition = player.transform.position;        

		cameraWorld = GameObject.Find("Main Camera").GetComponent<Camera>();

        rt = GetComponent<RectTransform>();
        canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        playerScreenPos = cameraWorld.WorldToViewportPoint(player.transform.TransformPoint(playerPosition));
        rt.anchorMax = playerScreenPos;
        rt.anchorMin = playerScreenPos;

        m_LocalScale = transform.localScale;
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		ScreenPos();
	}

	private void ScreenPos()
	{
		if (photonView.IsMine)
		{
			if (cameraWorld != null)
			{
				playerScreenPos = cameraWorld.WorldToViewportPoint(player.transform.TransformPoint(playerPosition));	

				rt.anchorMax = playerScreenPos;
				rt.anchorMin = playerScreenPos;

				if (playerControl.FacingRight == false)
				{
					transform.localScale = new Vector3(-m_LocalScale.x, m_LocalScale.y, m_LocalScale.z);
				}
				else
				{
					transform.localScale = new Vector3(m_LocalScale.x, m_LocalScale.y, m_LocalScale.z);
				}

				PlayerPanel.SetActive(true);
				PlayerControlCanvas.SetActive(true);
			} 
			else
			{
				cameraWorld = GameObject.Find("Main Camera").GetComponent<Camera>();				
			}							
		}		
	}
}
