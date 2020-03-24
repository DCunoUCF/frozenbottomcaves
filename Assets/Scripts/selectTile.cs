using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectTile : MonoBehaviour
{
    private bool filled = false;
    public int index;
    public HighlightManager hm;
    private List<Vector3> tiles;

    private void Start()
    {
        Vector3 parent = transform.parent.position;
        tiles = new List<Vector3>();

        for (int i = 0; i < this.transform.childCount; i++)
            tiles.Add(parent + this.transform.GetChild(i).transform.localPosition);
        //print(parent);
        //foreach (Vector3 v in tiles)
        //    print(v);

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
            for (int i = 0; i < this.transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
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
        for (int i = 0; i < this.transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        filled = true;
    }

    public void setDeselected()
    {
        for (int i = 0; i < this.transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        filled = false;
    }
}
