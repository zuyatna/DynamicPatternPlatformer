using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoyalGuard_Health : MonoBehaviourPunCallbacks, IPunObservable {

	#region Private Variables

    private Animator m_Anim;
    private Rigidbody2D m_Body; 
	[SerializeField] private Image playerHealth;
	public Image playerMiniHealth;

    #endregion

	#region Player Knockback

	[Header("Player Knockback")]
	public float Knockback;

	private Vector2 directionPunch;
	public Text showDirection;

	public bool isPunchAttack;
	public float tempTime = 0.3f;

	[Header("Player Status")]
	public GameObject statusFire;
	public Image activeFire;
	private bool usingUltimate;

	[Tooltip("Spawn Player When Death")]
	private Transform spawnPlayerPoint;

	public int playerPoint;
	public Text playerPointText;

	public GameObject playerResource;

	private int standingPlayer = 4;

	[Header("Death Status")]
	private bool playerDeath;
	[SerializeField] private float cooldownDeath;
	float tempCooldownDeath;

	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		m_Body = this.gameObject.GetComponent<Rigidbody2D>();
        m_Anim = this.gameObject.GetComponent<Animator>();

		spawnPlayerPoint = GameObject.Find("SpawnPlayer").GetComponent<Transform>();
		tempCooldownDeath = cooldownDeath;		
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

        if (photonView.IsMine)
        {
			if(isPunchAttack)
			{
//				if(RoyalGuard_Control.Instance.FacingRight)
//				{
//					m_Body.velocity = new Vector2(Knockback, transform.position.y);					
//				}
//
//				if(!RoyalGuard_Control.Instance.FacingRight)
//				{
//					m_Body.velocity = new Vector2(-Knockback, transform.position.y);
//				}

				tempTime -= Time.deltaTime;
				if(tempTime < 0)
				{
					isPunchAttack = false;
					m_Anim.SetBool("Damage", false);
					tempTime = 0.3f;
				}
			}

			if(playerHealth.fillAmount == 1)
			{				
				m_Anim.SetBool("Death", true);
				playerDeath = true;
			}

			if(playerDeath)
			{
				tempCooldownDeath -= Time.deltaTime;

				if(tempCooldownDeath < 0)
				{
					photonView.RPC("RPCSpawnPlayer", RpcTarget.All);
					playerDeath = false;
					tempCooldownDeath = cooldownDeath;
				}								
			}			

			if(statusFire.activeInHierarchy)
			{
				activeFire.fillAmount -= 0.005f;

				photonView.RPC("RPCPlayerDamage", RpcTarget.All,  0.0002f);				

				if(activeFire.fillAmount == 0)
				{
					statusFire.SetActive(false);					
				}
			}

			if(spawnPlayerPoint == null)
			{
				spawnPlayerPoint = GameObject.Find("SpawnPlayer").GetComponent<Transform>();
			}
        }
	}

	private void PlayerDeath()
	{
		transform.position = new Vector3(0, spawnPlayerPoint.position.y, 0);
		this.gameObject.transform.position = transform.position;

		playerHealth.fillAmount = 0;
		playerMiniHealth.fillAmount = 1;		
	}

	private void PlayerSpawn()
	{
		transform.position = new Vector3(0, spawnPlayerPoint.position.y, 0);
		this.gameObject.transform.position = transform.position;
		m_Anim.SetBool("Death", false);

		playerHealth.fillAmount = 0;
		playerMiniHealth.fillAmount = 1;	
	}

	private void PlayerDamage(float _damage)
	{
		playerHealth.fillAmount += _damage;
		playerMiniHealth.fillAmount -= _damage;
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "PunchAttack" && usingUltimate == false)
		{
			directionPunch = (transform.position - other.transform.position).normalized;

			isPunchAttack = true;

			if(m_Anim.GetBool("LanceIdle") == true || m_Anim.GetBool("LanceRun") == true)
			{
				m_Anim.SetTrigger("LanceDamage");
			}
			else
			{
				m_Anim.SetBool("Damage", true);	
			}

			photonView.RPC("RPCPlayerDamage", RpcTarget.All,  0.12f);

			showDirection.text = directionPunch.ToString();
		}

		if(other.gameObject.tag == "FireBomb" && usingUltimate == false)
		{
			statusFire.SetActive(true);

			if(m_Anim.GetBool("LanceIdle") == true || m_Anim.GetBool("LanceRun") == true)
			{
				m_Anim.SetTrigger("LanceDamage");
			}
			else
			{
				m_Anim.SetBool("Damage", true);	
			}

			photonView.RPC("RPCPlayerDamage", RpcTarget.All,  0.12f);
		}

		if (other.gameObject.tag == "Border")
		{
			if(photonView.IsMine)
			{				
				photonView.RPC("RPCSpawnPlayer", RpcTarget.All);
				photonView.RPC("RPCPlayerDamage", RpcTarget.All,  0.3f);				
			}
		}
	}

	#region RPC
	
	[PunRPC]
	private void RPCSpawnPlayer()
	{		
		PlayerSpawn();
	}

	[PunRPC]
	private void RPCPlayerDamage(float _damage)
	{
		PlayerDamage(_damage);
	}

	[PunRPC]
	private void RPCUsingUltimate(bool _isUsing)
	{
		usingUltimate = _isUsing;
	}

	[PunRPC]
	private void RPCStandingPlayer(bool _isEliminate)
	{
		if(_isEliminate)
		{
			standingPlayer -= 1;
			_isEliminate = false;
		}
	}

	#endregion

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // // We own this player: send the others our data
			
			// send transform.position
			stream.SendNext(transform.position);

			// send m_Body.velocity
			stream.SendNext(m_Body.velocity);

			// send punch attack
			stream.SendNext(isPunchAttack);

			stream.SendNext(playerPoint);

        }
        else
        {
            // // Network player, receive data
            
			// receive transform.position
			this.transform.position = (Vector3)stream.ReceiveNext();

			// receive m_Body.velocity
			this.m_Body.velocity = (Vector2)stream.ReceiveNext();

			// receive punch attack
			this.isPunchAttack = (bool)stream.ReceiveNext();

			this.playerPoint = (int)stream.ReceiveNext();

        }
    }
}