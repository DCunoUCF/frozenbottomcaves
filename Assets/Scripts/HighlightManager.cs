using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    private bool initSelection, dpadPressed;
    private float horInput, vertInput;
    public selectTile[] TSarr;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        initSelection = false;
        horInput = 0f;
        vertInput = 0f;
        dpadPressed = false;
        TSarr = new selectTile[16];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Wait until there are actually highlights on the ground
        if (initSelection)
        {
            // Every .15s there will be a new input for this
            if (dpadPressed)
            {
                // Move clockwise around the player
                if (horInput > 0)
                {
                    TSarr[index].setDeselected();
                    index += 1;
                    index = index % TSarr.Length;
                    print(index);
                    TSarr[index].setSelected();
                }
                // Move CCW around the player
                else if (horInput < 0)
                {
                    if (index == 0)
                    {
                        TSarr[index].setDeselected();
                        index = TSarr.Length - 1;
                        TSarr[index].setSelected();
                    }
                    else
                    {
                        TSarr[index].setDeselected();
                        TSarr[--index].setSelected();
                    }
                }
                dpadPressed = false;
            }

        }
    }

    public void setTiles(List<GameObject> tiles)
    {
        print("Setting tiles in HM, size of tiles list: " + tiles.Count);
        int childCount = tiles.Count, i = 0;

        // Sets each item to null then resizes to fit exactly
        for (int j = 0; j < TSarr.Length; j++)
        {
            TSarr[j] = null;
        }
        Array.Resize<selectTile>(ref TSarr, childCount);

        // For each child, grab the script that manages active tiles and throw it into the array
        for (i = 0; i < childCount; i++)
        {
            Transform temp = this.transform.GetChild(i);
            print(temp.name);
            TSarr[i] = (selectTile)temp.GetChild(0).GetComponent("selectTile");
            TSarr[i].index = i;
            TSarr[i].hm = this;
        }

        index = 0;

        // Grabs dpad input every .15s
        InvokeRepeating("getDpadInput", 1f, .15f);

        // Set the initial tile selection
        TSarr[0].setSelected();                  // need to take into account mouse pos and last selected in the future
        initSelection = true;
    }

    public void clearTiles()
    {
        print("Clearing HM fields");
        CancelInvoke();
        this.dpadPressed = false;

        // Redundant step to nullify items in array
        if (TSarr[0] != null)
        {
            for (int i = 0; i < TSarr.Length; i++)
                TSarr[i] = null;
        }

        initSelection = false;
        //print(initSelection);
    }

    private void getDpadInput()
    {
        horInput = Input.GetAxis("HorizontalDpad");
        vertInput = Input.GetAxis("VerticalDpad");
        if (Mathf.Abs(horInput) > Mathf.Abs(vertInput))
        {
            vertInput = 0f;
            horInput = Mathf.Round(horInput);
        }
        else
        {
            horInput = 0f;
            vertInput = Mathf.Round(vertInput);
        }
        dpadPressed = true;
    }

    // Sets the new highlighted tile to the one the mouse is over and updates the index here
    public void mousedOver(int index)
    {
        //print(index);
        TSarr[this.index].setDeselected();
        this.index = index;
    }
}
