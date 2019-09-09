using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPunCallbacks, IPunObservable
{

	public static PlayerAttack Instance;

	#region Private variables

	[Tooltip("Player Animation")]
	private Animator m_Anim;
	private Rigidbody2D m_Body;
	private Transform m_Transform;

	private float lastTime;

	[Tooltip("Current Combo")]
	private int combo;

	[HideInInspector] public bool attackSpecialWeapon;
	[HideInInspector] public bool lerpAttack;

	#endregion

	#region Public Variables
	
	[HideInInspector] public bool isPunch;

	[Header("Timer Combo and Cooldown")]
	public float cooldown;

	[HideInInspector] public Button attackBttn;

	[Header("Player Items")]	
	public GameObject grabItemsButton;
	public GameObject grabActiveItemButton;
	[HideInInspector] public bool onItemGrab;
	[HideInInspector] public bool activeItemGrab;

	[Header("Player Special Weapon")]
	public GameObject activeUltiObject;
	public Button ultiButton;	

	[HideInInspector] public bool activeWeaponUlti;
	[HideInInspector] public bool onSpecialWeapon;
	[HideInInspector] public bool activeSpecialWeapon;

	public float forceAttack;
	public float time;
	private float tempTime;

	[Header("Player Weapon")]
	[HideInInspector] public bool onWeapon;
	[HideInInspector] public bool activeWeapon;
	[HideInInspector] private bool activeLance;

	[Header("Throw Fire Animation")]    
    public GameObject throwFire;

	[Header("Action Point and Consumtion")]
	[SerializeField] private Image actionPointPlayer;
	public float basicAttackConsumtion;
	public float limitBasicAttack;
	public float specialWepoanConsumtion;
	public float limitSpecialWeapon;
	public float lanceWeaponConsumtion;
	public float limitLanceWeapon;

	#endregion

	#region Eksternal Script
		[SerializeField] private RoyalGuard_Control royalGuard_Control;
	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;
		m_Anim = GetComponent<Animator>();
		m_Body = GetComponent<Rigidbody2D>();
		m_Transform = GetComponent<Transform>();		

		StartCoroutine("Punch");
		StartCoroutine("SpecialWeapon");
		StartCoroutine("ShieldUltimateAttack");
		StartCoroutine("LanceAttack");

		grabItemsButton.GetComponent<Button>().onClick.AddListener(Grab_And_Throw_Items);
		grabActiveItemButton.GetComponent<Button>().onClick.AddListener(Grab_And_Throw_Items);
		ultiButton.onClick.AddListener(RGShieldUlti);		
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {            
            return;
        }

		if(photonView.IsMine)
		{
			if(activeItemGrab)
			{
				onItemGrab = false;
				photonView.RPC("RPCActiveItemBomb", RpcTarget.All, true);

				photonView.RPC("RPCActiveItemBombCollider", RpcTarget.All, true);               
				
				tempTime -= Time.deltaTime;
				if(tempTime < 0)
				{
					photonView.RPC("RPCActiveItemBombCollider", RpcTarget.All, false);
					tempTime = time;
					activeItemGrab = false;
				}				
			}

			if(activeSpecialWeapon)
			{
				onSpecialWeapon = false;
				activeSpecialWeapon = false;
				photonView.RPC("RPCActiveShield", RpcTarget.All, true);

				activeUltiObject.SetActive(true);
				ultiButton.interactable = true;
			}

			if(activeWeapon)
			{
				onWeapon = false;
				activeWeapon = false;
				activeLance = true;

				photonView.RPC("RPCActiveLance", RpcTarget.All, true);
			}

			if(attackSpecialWeapon)
			{	

				if(m_Anim.GetBool("Ground") == true)
				{
					if(m_Transform.localScale.x > 0)
					{
						m_Body.velocity = new Vector2(forceAttack, 0);	
					}
					else
					{
						m_Body.velocity = new Vector2(-forceAttack, 0);	
					}					
				}
				else
				{
					if(m_Transform.localScale.x > 0)
					{
						// m_Body.velocity = new Vector2((forceAttack - 5), transform.position.y);
						m_Body.velocity = new Vector2((forceAttack - 5), -7);
					}
					else
					{
						// m_Body.velocity = new Vector2(-(forceAttack - 5), transform.position.y);
						m_Body.velocity = new Vector2(-(forceAttack - 5), -7);
					}
				}

				tempTime -= Time.deltaTime;
				if(tempTime < 0)
				{
					attackSpecialWeapon = false;					
					tempTime = time;
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Space))
			{
				isPunch = true;
			}

			if(Input.GetKeyUp(KeyCode.Space))
			{
				isPunch = false;
			}
		}
	}

	#region Player Attack and Ultimate Input
	
	public void PlayerOnPunch()
	{		
		isPunch = true;		
	}

	public void PlayerNotPunch()
	{
		isPunch = false;
	}

	public void RGShieldUlti()
	{
		if(activeUltiObject.activeInHierarchy == true)
		{
			activeWeaponUlti = true;
			actionPointPlayer.fillAmount += 0.5f;	
		}		
	}

	#endregion	

	#region Punch Attack IEnumerator
	
	IEnumerator Punch()
	{
		if(photonView.IsMine)
		{
			while(true)
			{
				if(isPunch && activeUltiObject.activeInHierarchy == false && m_Anim.GetBool("Item_ThrowFire_Run") == false && actionPointPlayer.fillAmount < limitBasicAttack)
				{					
					m_Anim.SetBool("Run", false);					
					m_Anim.SetBool("Attack1_0", true);

					if(m_Anim.GetBool("Ground") == true)
					{
						m_Body.velocity = new Vector2(0, 0);
					}

					if(m_Anim.GetBool("Ground") == false)
					{
						m_Anim.SetBool("Attack_Jump", true);
					}

					actionPointPlayer.fillAmount += basicAttackConsumtion;			

					lastTime = Time.time;
					yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
				}
				else
				{
					m_Anim.SetBool("Attack1_0", false);
					m_Anim.SetBool("Attack_Jump", false);
				}
				
				yield return null;
			}
		}
	}

	#endregion	

	#region Weapon Attack IEnumerator
	
	IEnumerator SpecialWeapon()
	{
		if(photonView.IsMine)
		{
			while(true)
			{
				if(isPunch && activeUltiObject.activeInHierarchy == true && actionPointPlayer.fillAmount < limitSpecialWeapon)
				{
					m_Anim.SetBool("ShieldAttack", true);

					actionPointPlayer.fillAmount += specialWepoanConsumtion;					

					lastTime = Time.time;
					yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
				}
				else
				{
					m_Anim.SetBool("ShieldAttack", false);
				}			

				yield return null;
			}
		}		
	}

	#endregion

	#region Shield Ultimate IEnumerator
	
	IEnumerator ShieldUltimateAttack()
	{
		if(photonView.IsMine)
		{
			while(true)
			{
				if(activeWeaponUlti && m_Anim.GetBool("Ground") == true)
				{
					m_Body.velocity = new Vector2(0, 0);
					m_Anim.SetBool("ShieldUltimate", true);

					photonView.RPC("RPCUsingUltimate", RpcTarget.All, true);									

					lastTime = Time.time;
					yield return new WaitForSeconds((cooldown - 0.4f) - (Time.time - lastTime));
				}
				else
				{
					// photonView.RPC("RPCUsingUltimate", RpcTarget.All, false);
					m_Anim.SetBool("ShieldUltimate", false);
				}			

				yield return null;
			}
		}		
	}

	#endregion

	#region Lance Attack IEnumerator
	
	IEnumerator LanceAttack()
	{
		if(photonView.IsMine)
		{
			while(true)
			{
				if(isPunch && activeLance && actionPointPlayer.fillAmount < limitLanceWeapon)
				{					
					m_Anim.SetBool("LanceRun", false);					
					m_Anim.SetBool("LanceAttack", true);

					if(m_Anim.GetBool("Ground") == true)
					{
						m_Body.velocity = new Vector2(0, 0);
					}

					if(m_Anim.GetBool("Ground") == false)
					{
						m_Anim.SetBool("LanceJumpAttack", true);
					}					

					actionPointPlayer.fillAmount += lanceWeaponConsumtion;

					lastTime = Time.time;
					yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
				}
				else
				{
					m_Anim.SetBool("LanceAttack", false);
					m_Anim.SetBool("LanceJumpAttack", false);
				}
				
				yield return null;
			}
		}
	}

	#endregion

	#region Grab Items Bomb
	
	public void Grab_And_Throw_Items()
	{
		if(onItemGrab)
		{	
			activeItemGrab = true;

			grabItemsButton.SetActive(false);
			grabActiveItemButton.SetActive(true);
		}

		if(throwFire.activeInHierarchy)
		{
			activeItemGrab = false;
			onItemGrab = false;

			grabItemsButton.SetActive(true);
			grabActiveItemButton.SetActive(false);

			actionPointPlayer.fillAmount += basicAttackConsumtion;

			if(photonView.IsMine)
			{
				m_Anim.SetBool("Throw_Fire", true);
				photonView.RPC("RPCActiveItemBomb", RpcTarget.All, false);
			}
		}

		if(onSpecialWeapon)
		{
			activeSpecialWeapon = true;

			grabItemsButton.SetActive(false);
			grabActiveItemButton.SetActive(true);			

			photonView.RPC("RPCActiveShieldCollider", RpcTarget.All, true);			
		}

		if(royalGuard_Control.shield1.enabled)
		{			
			grabItemsButton.SetActive(true);
			grabActiveItemButton.SetActive(false);
			activeUltiObject.SetActive(false);

			actionPointPlayer.fillAmount += basicAttackConsumtion;

			photonView.RPC("RPCDirection_ThrowingShield", RpcTarget.All);
			photonView.RPC("RPCActiveShieldCollider", RpcTarget.All, false);
			photonView.RPC("RPCActiveShield", RpcTarget.All, false);
		}

		if(onWeapon)
		{
			activeWeapon = true;

			grabItemsButton.SetActive(false);
			grabActiveItemButton.SetActive(true);
			
			photonView.RPC("RPCActiveLanceCollider", RpcTarget.All, true);
		}

		if(royalGuard_Control.lanceAnim)
		{
			grabItemsButton.SetActive(true);
			grabActiveItemButton.SetActive(false);

			actionPointPlayer.fillAmount += basicAttackConsumtion;

			photonView.RPC("RPCDirection_ThrowingLance", RpcTarget.All);
			photonView.RPC("RPCActiveLanceCollider", RpcTarget.All, false);
			photonView.RPC("RPCActiveLance", RpcTarget.All, false);

			activeLance = false;
		}
	}

	#endregion		

	#region OnTrigger 2D

	/// <summary>
	/// Sent each frame where another object is within a trigger collider
	/// attached to this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "ItemGrab")
		{
			Debug.LogWarning("on items");
			onItemGrab = true;
		}

		if(other.gameObject.tag == "WeaponSpecial" && grabItemsButton.activeInHierarchy)
		{
			onSpecialWeapon = true;			
		}

		if(other.gameObject.tag == "Weapon" && grabItemsButton.activeInHierarchy)
		{
			onWeapon = true;	
		}
	}	

	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "ItemGrab")
		{
			Debug.LogWarning("out items");
			onItemGrab = false;
		}

		if(other.gameObject.tag == "WeaponSpecial")
		{
			onSpecialWeapon = false;			
		}

		if(other.gameObject.tag == "Weapon")
		{
			onWeapon = false;
		}
	}

	#endregion

	public void AlertThrowFire(string _message)
	{
		// check if animation "Item-ThrowFire" is finish
        if(_message.Equals("ThrowFireEnded"))
		{
			if(photonView.IsMine)
			{
				m_Anim.SetBool("Throw_Fire", false);
				photonView.RPC("RPCDirection_ThrowingItems", RpcTarget.All);
			}				
        }
	}

	public void AlertWeaponAnimation(string _message)
	{
		if(_message.Equals("ShieldAttackEnded"))
		{
			attackSpecialWeapon = true;			
        }
	}

	public void AlertRGShieldUltiamate(string _message)
	{
        if(_message.Equals("RGShieldUltiamateEnded"))
		{
            activeWeaponUlti = false;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // // We own this player: send the others our data

            // send isPunch
            stream.SendNext(isPunch);

			// send onItemGrab
			stream.SendNext(onItemGrab);

			// send activeItemGrab
			stream.SendNext(activeItemGrab);

			// send onSpecialWeapon
			stream.SendNext(onSpecialWeapon);			

			// send activeSpecialWeapon
			stream.SendNext(activeSpecialWeapon);

			// send activeWeaponUlti
			stream.SendNext(activeWeaponUlti);
        }
        else
        {
            // // Network player, receive data

            // receive isPunch
            this.isPunch = (bool)stream.ReceiveNext();

			// receive onItemGrab
			this.onItemGrab = (bool)stream.ReceiveNext();

			// receive activeItemGrab
			this.activeItemGrab = (bool)stream.ReceiveNext();

			// receivea ctiveSpecialWeapon
			this.activeSpecialWeapon = (bool)stream.ReceiveNext();

			// receive onSpecialWeapon
			this.onSpecialWeapon = (bool)stream.ReceiveNext();

			// receive activeWeaponUlti
			this.activeWeaponUlti = (bool)stream.ReceiveNext();
        }
    }
}