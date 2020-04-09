using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemyScript;
    private int maxHP;
    private float percentFull;
    public Transform fill;
    private bool init;

    public void initBar(int maxHP)
    {
        this.maxHP = maxHP;
        fill.localScale = new Vector3(1, 1, 1);
    }

    public void updateBar(int hp)
    {
        print("HP: " + hp + " MAXHP: " + maxHP);
        if (hp > 0)
            fill.localScale = new Vector3((float)(hp / maxHP), 1, 1);
        else
            fill.localScale = new Vector3(0, 1, 1);
    }
}
