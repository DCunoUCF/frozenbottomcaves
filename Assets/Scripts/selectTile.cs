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
        List<GameObject> delete = new List<GameObject>();

        tiles = new List<Vector3>();

        // Loop through looking for tiles in invalid locations and add them to the deletion list
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (!BattleManager.Instance.isPassable(parent + this.transform.GetChild(i).transform.localPosition))
                delete.Add(this.transform.GetChild(i).gameObject);
        }

        // Delete tiles in the deletion list
        foreach(GameObject g in delete)
        {
            Destroy(g);
            g.transform.parent = null;
        }

        // Grab the remaining tiles positions for potential attack locations
        for (int i = 0; i < this.transform.childCount; i++)
        {
            tiles.Add(parent + this.transform.GetChild(i).transform.localPosition);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit") && filled)
        {
            PlayerManager.Instance.setSelectedTile(tiles);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("pos: "+transform.parent.position);
        PlayerManager.Instance.setSelectedTile(tiles);
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
