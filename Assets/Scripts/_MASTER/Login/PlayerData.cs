using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public InputField inputName;
    
    private string _username;
    private bool _canChangeScene;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        
        _username = "Player";
        _canChangeScene = false;
    }

    private void Update()
    {
        if (!inputName.text.Equals(""))
        {
            _username = inputName.text;
            _canChangeScene = true;
        }
    }

    public void ChangeScene()
    {
        if (_canChangeScene)
        {
            PlayerPrefs.SetString("playerName", _username);
            SceneManager.LoadScene("MenuGame 1");   
        }
        else
        {
            Debug.Log("inputName: Null");
        }
    }
}
