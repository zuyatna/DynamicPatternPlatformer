using UnityEngine;
using Photon.Pun;

public class SimulationPlayer : MonoBehaviourPunCallbacks {

    #region player properties - variables

    private Animator playerAnim;
    private Rigidbody2D playerBody;

    #endregion

    #region other player/object - outside

    private GameObject[] otherObject;

    #endregion


    private void Awake()
    {
        // inisialisasi player
        playerAnim = this.gameObject.GetComponent<Animator>();
        playerBody = this.gameObject.GetComponent<Rigidbody2D>();

        otherObject = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		
	}

    #region player properties - methods

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.tag == "Ground")
        {
            playerAnim.SetBool("Ground", true);
        }             
    }

    #endregion
}
