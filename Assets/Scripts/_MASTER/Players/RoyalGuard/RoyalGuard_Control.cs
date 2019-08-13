using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoyalGuard_Control : MonoBehaviourPunCallbacks, IPunObservable
{

	public static RoyalGuard_Control Instance;

	#region player Ability

    public float m_MaxSpeed = 10f;    //player speed in x axis
    [SerializeField] private float m_JumpForce = 500f;  //force power player to jump
    [SerializeField] private LayerMask m_WhatIsGround;  //what's ground indentify

    #endregion

	#region player Properties

    [HideInInspector] public Rigidbody2D m_Body;    //this is player body    
    [HideInInspector] public Animator m_Anim;       //player animator
    [HideInInspector] public Vector3 m_LocalScale;  //local scale player

    const float k_GroundRadius = 0.2f;              //radius from ground check
	private float speed;                            //speed input repo    

    [HideInInspector] public bool m_Grounded = true; //is player collide with ground?
    [HideInInspector] public bool FacingRight = true;

	private Transform m_GroundCheck;      //check for ground
	private float originCountPosition = 1.2f;

	private bool canRun;                  //can player run?

    [Header("Skin Mesh Renderer Shield")]
    public SkinnedMeshRenderer shield1;
    public SkinnedMeshRenderer shield2;        

    private int getInputMovement;
    private int getInputJump;
    public float cooldown;
    private float lastTime;

    [Header("Lance Animation")]
    [HideInInspector] public bool lanceAnim;
    public GameObject lance;

    [Header("Throw Fire Animation")]
    private bool throwFireAnim;
    public GameObject throwFire;

    #endregion

    #region Player Image
    
    [Header("Action Point")]    
    [SerializeField] private Image actionPointPlayer;

    [Header("Regen and Consumtion")]
    public float regen;
    public float consumtion;
    public float limitJump;

    [Tooltip("Disable Collider2D when grab object drop")]
    private float timerCollider = 1f;
    private GameObject playerCollisionObject;

    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
	{
        Instance = this;

		// setting up reference
        m_Anim = GetComponent<Animator>();
        m_Body = GetComponent<Rigidbody2D>();
        m_LocalScale = transform.localScale;

        m_GroundCheck = transform.Find("GroundCheck");

        shield1.enabled = false;
        shield2.enabled = false;
        shield1.GetComponent<Collider2D>().enabled = false;             
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
            // refill action point player
            actionPointPlayer.fillAmount -= regen;

            UpdateIsGrounded();

            if(m_Anim.GetBool("ShieldUltimate") == false && m_Anim.GetBool("Attack1_0") == false)
            {
                UpdateMovement(getInputMovement);                
                UpdateJumping(getInputJump);

                #region PC input - keydown

                // PC input
                if (Input.GetKeyDown(KeyCode.A))
                {
                    PlayerMoveLeft();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    PlayerMoveRight();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerJump();
                }

                #endregion

            }

            #region PC input - keyup

            // PC input
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                PlayerNotMove();
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                PlayerDontJump();
            }

            #endregion

            if (lanceAnim)
            {
                m_Anim.SetBool("LanceIdle", true);
            }

            if(throwFireAnim)
            {
                m_Anim.SetBool("Item_ThrowFire_Idle", true);
                throwFire.GetComponent<Collider2D>().enabled = true;
            }

            if(shield1.enabled)
            {
                timerCollider -= Time.deltaTime;
                
                if(timerCollider < 0)
                {
                    photonView.RPC("RPCActiveShieldCollider", RpcTarget.All, false);
                    timerCollider = 1f;
                }
            }

            if(m_Anim.GetBool("LanceIdle"))
            {
                timerCollider -= Time.deltaTime;

                if(timerCollider < 0)
                {
                    photonView.RPC("RPCActiveLanceCollider", RpcTarget.All, false);
                    timerCollider = 1f;
                }                
            }            
        }
	}

	private void UpdateMovement(float MoveInput)
    {
        // set body velocity with input and jump velocity for move
        m_Body.velocity = new Vector2(MoveInput, m_Body.velocity.y);              
                    
        // m_Anim.SetBool("RunSteady", Mathf.Abs(m_Body.velocity.x) > 0.2);
        canRun = true;

        // change tag
        this.gameObject.tag = "Player";

        // if the input is moving the player to right or left...
        if (m_Body.velocity.x > 0.2)
        {
            transform.localScale = new Vector3(m_LocalScale.x, m_LocalScale.y, m_LocalScale.z);

            if(FacingRight == false)
            {
                transform.position = new Vector3(transform.position.x + originCountPosition, transform.position.y, transform.position.z);
            }

            FacingRight = true;
        }
        else if (m_Body.velocity.x < -0.2)
        {
            transform.localScale = new Vector3(-m_LocalScale.x, m_LocalScale.y, m_LocalScale.z);

            if(FacingRight == true)
            {
                transform.position = new Vector3(transform.position.x - originCountPosition, transform.position.y, transform.position.z);
            }

            FacingRight = false;
        }
        else
        {
            // set default if player stop moving
            canRun = false;
            m_Anim.SetBool("RunSteady", false);

            if(shield1.enabled)
            {
                m_Anim.SetBool("RunShield", false);
            }
            else if(lanceAnim)
            {
                m_Anim.SetBool("LanceRun", false);
                m_Anim.SetBool("LanceIdle", true);
            }
            else if(throwFireAnim)
            {
                m_Anim.SetBool("Item_ThrowFire_Run", false);
                m_Anim.SetBool("Item_ThrowFire_Idle", true);                
            }
            else
            {
                m_Anim.SetBool("Run", false);
            }                       
        }

		// if player ended animation "RunSteady", so switch to animation "Run"
        if(canRun || m_Grounded == false)
        {            

            speed = m_MaxSpeed * MoveInput;
            m_Body.velocity = new Vector2(speed, m_Body.velocity.y);

            if(shield1.enabled)
            {     
                m_Anim.SetBool("RunShield", Mathf.Abs(m_Body.velocity.x) > 0.2);
                m_Anim.SetBool("Run", false);
            }
            else if(lanceAnim)
            {
                m_Anim.SetBool("LanceRun", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
            else if(throwFireAnim)
            {
                m_Anim.SetBool("Item_ThrowFire_Run", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
            else
            {
                m_Anim.SetBool("Run", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
        }
        else if((m_Anim.GetBool("Run") == true || m_Anim.GetBool("RunShield") == true) && !canRun)
        {

            // animation "Run" directly when you step on the Ground
            speed = m_MaxSpeed * MoveInput;
            m_Body.velocity = new Vector2(speed, m_Body.velocity.y);

            if(shield1.enabled)
            {
                m_Anim.SetBool("RunShield", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
            else if(lanceAnim)
            {
                m_Anim.SetBool("LanceRun", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
            else if(throwFireAnim)
            {
                m_Anim.SetBool("Item_ThrowFire_Run", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }
            else
            {
                m_Anim.SetBool("Run", Mathf.Abs(m_Body.velocity.x) > 0.2);
            }                
        }          
    }	

    #region Control Player Move By Button
    
    public void PlayerMoveLeft()
    {
        getInputMovement = -1;
    }

    public void PlayerMoveRight()
    {
        getInputMovement = 1;
    }

    public void PlayerNotMove()
    {
        getInputMovement = 0;
    }

    #endregion

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        name = new System.Text.StringBuilder()
            .Append(photonView.Owner.NickName)
            .Append(" [")
            .Append(photonView.ViewID)
            .Append("]")
            .ToString();

        BroadcastMessage("OnInstantiate", info, SendMessageOptions.DontRequireReceiver);
    }

    public void SetCustomProperty(string propName, object value)
    {
        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
        prop.Add(propName, value);
        photonView.Owner.SetCustomProperties(prop);
    }

	#region Player Jump

    private void UpdateJumping(float _canJump)
    {		                
        if (m_Grounded && m_Anim.GetBool("Ground") && _canJump == 1)
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Anim.SetFloat("vSpeed", m_Body.velocity.y);

            // m_Body.AddForce(new Vector2(0f, m_JumpForce));
            m_Body.velocity = new Vector2(transform.position.x, 21);

            if(throwFireAnim)
            {
                m_Anim.SetBool("Item_ThrowFire_Jump", true);
            }           

            actionPointPlayer.fillAmount += consumtion;                             
    	}

        if(!m_Grounded && m_Body.velocity.y > 7) 
        {
            getInputJump = 0;
        }
	}    

    public void PlayerJump()
    {
        if(actionPointPlayer.fillAmount < limitJump)
        {
            getInputJump = 1;
            
        }        
    }

    public void PlayerDontJump()
    {
        getInputJump = 0;
    }
        
    #endregion

	#region Player Detect Ground

	private void UpdateIsGrounded()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;  
            }
        }

        m_Anim.SetBool("Ground", m_Grounded);

        if(lanceAnim)
        {
            m_Anim.SetBool("LanceJump", m_Grounded);
        }

        if(throwFireAnim)
        {
            m_Anim.SetBool("Item_ThrowFire_Jump", m_Grounded);
        }

        if(m_Anim.GetBool("Ground") == true)
        {
            if(shield1.enabled)
            {
                m_Anim.SetBool("Shield_Jump", false);
            } 
        }
        else
        {
            if(shield1.enabled)
            {
                m_Anim.SetBool("Shield_Jump", true);
            } 
        } 

        // set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Body.velocity.y);
    }

    #endregion    

    /// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
        #region CamLeft or CamRight
        
        // detect tag CamLeft or CamRight
        if(other.gameObject.tag == "CamLeft")
        {
            CameraCinematic.Instance.camLeft = true;
        }

        if(other.gameObject.tag == "CamRight")
        {
            CameraCinematic.Instance.camRight = true;
        }

        #endregion

	}

	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
        #region CamLeft or CamRight
        
        // detect tag CamLeft or CamRight
        if(other.gameObject.tag == "CamLeft")
        {
            CameraCinematic.Instance.camLeft = false;
        }

        if(other.gameObject.tag == "CamRight")
        {
            CameraCinematic.Instance.camRight = false;
        }

        #endregion

	}

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        playerCollisionObject = other.gameObject;        
    }

	public void AlertRunSteady(string _message)
    {
        // check if animation "RunSteady" is finish
        if(_message.Equals("RunSteadyAnimationEnded"))
        {
            canRun = true;
        }
    }

    [PunRPC]
	private void RPCActiveItemBomb(bool _activeBomb)
	{
		if(_activeBomb)
		{
            m_Anim.SetBool("Item_ThrowFire_Idle", true);
		}
		else
		{
			m_Anim.SetBool("Item_ThrowFire_Idle", false);            
		}

        throwFireAnim = _activeBomb;
	}

	[PunRPC]
	private void RPCActiveItemBombCollider(bool _activeBomb)
	{
		if(_activeBomb)
		{
            throwFire.GetComponent<Collider2D>().enabled = true;
		}
		else
		{
			throwFire.GetComponent<Collider2D>().enabled = false;
		}
	}

    [PunRPC]
    private void RPCActiveShield(bool _isActive)
    {
        if(_isActive)
        {            
            shield1.enabled = true;
            shield2.enabled = true;  

            m_Anim.SetBool("Run", false);          
        }
        else
        {
            shield1.enabled = false;
            shield2.enabled = false;

            m_Anim.SetBool("RunShield", false);
            Debug.LogWarning("Disable Shield");
        }
    }

    [PunRPC]
    private void RPCActiveShieldCollider(bool _isActive)
    {
        if(_isActive)
        {
            shield1.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            shield1.GetComponent<Collider2D>().enabled = false;
        }
    }

    [PunRPC]
    private void RPCActiveLance(bool _isActive)
    {        
        if(_isActive)
        {
            m_Anim.SetBool("LanceIdle", true);
        }
        else
        {
            m_Anim.SetBool("LanceIdle", false);
        }

        lanceAnim = _isActive;
    }

    [PunRPC]
    private void RPCActiveLanceCollider(bool _isActive)
    {
        if(_isActive)
        {
            lance.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            lance.GetComponent<Collider2D>().enabled = false;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // // We own this player: send the others our data

            // send transform.position
            stream.SendNext(transform.position);

            // send FacingRight
            stream.SendNext(FacingRight);                       
        }
        else
        {
            // // Network player, receive data

            // receive transform.position
            this.transform.position = (Vector3)stream.ReceiveNext();

            // receive FacingRight
            this.FacingRight = (bool)stream.ReceiveNext();
                        
        }
    }
}