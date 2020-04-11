using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDMGNumbers : MonoBehaviour
{
    public EnemyHealthBar ehb;
    public TextMeshPro t;

    public void gimmeDemNumbers(int dam)
    {
        print("WAAAAAAAAAAAAAAAAAAAAAA");
        if (this.gameObject.activeSelf)
        {
            if (dam > 0)
                StartCoroutine(displayNum(dam));
        }
    }

    public IEnumerator displayNum(int damage)
    {
        t.SetText("" + damage);
        yield return new WaitForSeconds(1.5f);
        t.SetText("");
        yield break;
    }

}
