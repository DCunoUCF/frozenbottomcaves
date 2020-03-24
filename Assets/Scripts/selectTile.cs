using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTile : MonoBehaviour
{
    private bool filled = false;
    private GameObject fill ;

    private void OnMouseDown()
    {
        Debug.Log("pos: "+transform.parent.position);
        PlayerManager.Instance.setSelectedTile(transform.parent.position);
    }

    private void OnMouseOver()
    {
        if(!filled)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        filled = true;
    }

    private void OnMouseExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        filled = false;
    }
}
