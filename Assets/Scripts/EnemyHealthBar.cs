using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyHealthBar : MonoBehaviour
{

    private float MaxHP = 20f;
    private float CurrentHP = 20f;
    public GameObject hpTextObj;
    private TextMeshProUGUI hpText;

    private void Start()
    {
        hpText = hpTextObj.GetComponent<TextMeshProUGUI>();
        hpText.text = "" + CurrentHP + "/" + MaxHP;
    }

    // This bar will be updated to show the value of the current selected enemy
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            CurrentHP -= 2f;
            if (CurrentHP <= 0)
                transform.localScale = new Vector3(0f, 1f, 1f);
            else
                transform.localScale = new Vector3(CurrentHP / MaxHP, 1f, 1f);

        }
        hpText.text = "" + CurrentHP + "/" + MaxHP;
    }
}
