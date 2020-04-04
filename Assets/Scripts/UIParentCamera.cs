using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParentCamera : MonoBehaviour
{
    public Camera UICamOW, UICamBW;
    bool updateCanvas;
    // Start is called before the first frame update
    void Start()
    {
        updateCanvasOW();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.inCombat)
        {
            UICamBW = GameObject.Find("MainCameraBW").GetComponent<Camera>();
            if (!updateCanvas)
                updateCanvasBW();
        }
        else
        {
            if (!updateCanvas)
                updateCanvasOW();
        }
    }

    private void updateCanvasOW()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = UICamOW;
        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = UICamOW;
        this.transform.GetChild(2).GetComponent<Canvas>().worldCamera = UICamOW;
    }

    private void updateCanvasBW()
    {
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = UICamBW;
        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = UICamBW;
        this.transform.GetChild(2).GetComponent<Canvas>().worldCamera = UICamBW;
    }
}
