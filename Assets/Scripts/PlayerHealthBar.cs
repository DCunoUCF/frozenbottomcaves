using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerHealthBar : MonoBehaviour
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

    // In practice will not use update and will call a method here whenever player takes dmg
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
