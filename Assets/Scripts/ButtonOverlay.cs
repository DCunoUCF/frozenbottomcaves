using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOverlay : MonoBehaviour
{
    public Button inventory, options, quit;

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
    }

    private void optOpen()
    {
        options.interactable = false;
        inventory.interactable = false;
        PlayerManager.Instance.inOptions = true;
    }

    private void quitGame()
    {
        Application.Quit();
    }
}
