using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTile : MonoBehaviour
{
    private bool filled = false;
    public int index;
    public HighlightManager hm;

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit") && filled)
        {
            PlayerManager.Instance.setSelectedTile(transform.parent.position);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("pos: "+transform.parent.position);
        PlayerManager.Instance.setSelectedTile(transform.parent.position);
    }

    private void OnMouseOver()
    {
        hm.mousedOver(index);
        if (!filled)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        filled = true;
    }

    //private void OnMouseExit()
    //{
    //    transform.GetChild(0).gameObject.SetActive(false);
    //    filled = false;
    //}

    public void setSelected()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        filled = true;
    }

    public void setDeselected()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        filled = false;
    }
}
