using UnityEngine;
using UnityEngine.UI;

public class MenusManager : MonoBehaviour
{
    #region Object Canvas

    [Header("Menus Panel")]
    [SerializeField] private GameObject createRoomPanel;
    [SerializeField] private GameObject FindRoomPanel;

    [Header("Create Button")]
    [SerializeField] private Button createRoomBtnIn;
    [SerializeField] private Button createRoomBtnOut;    

    [Header("Find Button")]
    [SerializeField] private Button findRoomBtnIn;
    [SerializeField] private Button findRoomBtnOut;

    bool isActiveCreateRoomPanel = false;
    bool isActiveFindRoomPanel = false;

    #endregion


    private void Start()
    {
        // OnClickListener
        createRoomBtnIn.onClick.AddListener(ActiveCreateRoom);
        createRoomBtnOut.onClick.AddListener(ActiveCreateRoom);

        findRoomBtnIn.onClick.AddListener(ActiveFindRoom);
        findRoomBtnOut.onClick.AddListener(ActiveFindRoom);
    }

    #region OnClickListener

    void ActiveCreateRoom()
    {
        if (!isActiveCreateRoomPanel)
        {
            createRoomPanel.SetActive(true);
            isActiveCreateRoomPanel = true;
        }
        else
        {
            createRoomPanel.SetActive(false);
            isActiveCreateRoomPanel = false;
        }
    }

    void ActiveFindRoom()
    {
        if (!isActiveFindRoomPanel)
        {
            FindRoomPanel.SetActive(true);
            isActiveFindRoomPanel = true;
        }
        else
        {
            FindRoomPanel.SetActive(false);
            isActiveFindRoomPanel = false;
        }
    }

    #endregion
}