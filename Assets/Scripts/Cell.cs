using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool pass;
    public GameObject entity;
    public Vector3 center;

    public Cell(bool passIn, GameObject entityIn, Vector3 centerIn)
    {
        center = centerIn;
        pass = passIn;
        entity = entityIn;
    }
}
