using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CList
{
    public GameObject entity;
    public bool move;
    public Vector3 movTar;
    public Vector3[] atkTar;
    public int dir, attack, attackDmg, hp;
    public int gridX, gridY;

    public CList(GameObject newEntity)
    {
        entity = newEntity;
        move = false;
        movTar = new Vector3();
        dir = 0;
        attack = 0; // stand in before AI is choosing attack or move. attack will be a number based on attack type
        hp = 10; // stand in before calling entity hp
        attackDmg = 0;
        atkTar = null;
        gridX = 0;
        gridY = 0;
    }
}
