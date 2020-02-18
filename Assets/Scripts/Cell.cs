using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool pass;
    public GameObject entity;
    public Vector3 center;
    public int x, y;

    public Cell(bool passIn, GameObject entityIn, Vector3 centerIn, int x, int y)
    {
        center = centerIn;
        pass = passIn;
        entity = entityIn;
        this.x = x;
        this.y = y;
    }
}
