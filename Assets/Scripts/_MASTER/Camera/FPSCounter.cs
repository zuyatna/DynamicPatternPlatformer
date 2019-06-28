using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FPSCounter : MonoBehaviourPunCallbacks {

    public Text FPStext;
    public Text PINGtext;
    
    private float count;
    private string tempPing;

    IEnumerator Start()
    {
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                FPStext.text = "fps: " + (Mathf.Round(count));

                PINGtext.text = "ping: " +PhotonNetwork.GetPing().ToString() +"ms";                           
            }
            else
            {
                FPStext.text = "Pause";
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
