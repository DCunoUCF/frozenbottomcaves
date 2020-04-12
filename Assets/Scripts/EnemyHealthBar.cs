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
    public TextMeshPro text;
    public EnemyDMGNumbers edn;

    public void updateBar(int hp)
    {
        this.maxHP = enemyScript.maxHp;
        //text.color = Color.red;
        print("HP: " + hp + " MAXHP: " + maxHP);
        print((hp / maxHP));
        percentFull = (float) hp / maxHP;
        if (hp > 0)
            fill.localScale = new Vector3(percentFull, 1, 1);
        else
            fill.localScale = new Vector3(0, 1, 1);

        //if (edn.gameObject.activeSelf)
        //{
        //    if (damageTaken > 0)
        //        StartCoroutine(edn.displayNum(damageTaken));
        //}

        //if (this.gameObject.activeSelf)
        //{
        //    if (damageTaken > 0)
        //        StartCoroutine(damageNumber(damageTaken));
        //}
    }

    //IEnumerator damageNumber(int damageTaken)
    //{
    //    text.SetText("" + damageTaken);
    //    yield return new WaitForSeconds(1.5f);
    //    text.SetText("");
    //}
}
