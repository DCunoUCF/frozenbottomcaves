using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOverlay : MonoBehaviour
{
    public Button inventory, options, quit;
    private bool inv, opt;


    void Start()
    {
        inventory.onClick.AddListener(invOpen);
        options.onClick.AddListener(optOpen);
        quit.onClick.AddListener(quitGame);
    }

    private void invOpen()
    {
        if (!PlayerManager.Instance.inCombat)
            PlayerManager.Instance.inventoryOpen();
        if (opt)
        {
            options.onClick.Invoke();
            PlayerManager.Instance.gm.om.dm.setUninteractable();
        }
    }

    private void optOpen()
    {
        if (!opt)
        {
            PlayerManager.Instance.inOptions = true;
            opt = true;
        }
        else
        {
            opt = false;
            PlayerManager.Instance.inOptions = false;
        }

        //options.interactable = false;
        //inventory.interactable = false;
        //PlayerManager.Instance.inOptions = true;
    }

    private void quitGame()
    {
        Application.Quit();
    }
}
