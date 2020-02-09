using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTile : MonoBehaviour
{
    public PlayerManager pmScript;
    public GameObject pmObject;
    private void OnMouseDown()
    {
        pmObject = GameObject.Find("PlayerManager");
        pmScript = (PlayerManager) pmObject.GetComponent(typeof(PlayerManager));
        pmScript.setSelectedTile(transform.position);
    }
}
