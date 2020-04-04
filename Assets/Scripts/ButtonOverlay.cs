using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOverlay : MonoBehaviour
{
    public static ButtonOverlay Instance { get; set; }
    public Button inventory, options, quit;
    public bool inv, opt;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        inv = false;
        opt = false;
        inventory.onClick.AddListener(invOpen);
        options.onClick.AddListener(optOpen);
        quit.onClick.AddListener(quitGame);
    }

    private void invOpen()
    {
        if (!PlayerManager.Instance.inCombat)
        {
            inv = true;
            //PlayerManager.Instance.invImg.color = Color.gray;
            PlayerManager.Instance.inventoryOpen();
        }
        if (opt)
        {
            options.onClick.Invoke();
            PlayerManager.Instance.gm.om.dm.setUninteractable();
        }
    }

    private void optOpen()
    {
        if (inv)
        {
            inv = false;
            print("SETTING INV COLOR");
            PlayerManager.Instance.invImg.color = Color.white;
        }
    }

    private void quitGame()
    {
        Application.Quit();
    }
}
