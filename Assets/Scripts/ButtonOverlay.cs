using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOverlay : MonoBehaviour
{
    public Button inventory, options;

    void Start()
    {
        inventory.onClick.AddListener(invOpen);
        options.onClick.AddListener(optOpen);
    }

    private void invOpen()
    {
        if (!PlayerManager.Instance.inCombat)
            PlayerManager.Instance.inventoryOpen();
    }

    private void optOpen()
    {
        // Open options menu
    }
}
