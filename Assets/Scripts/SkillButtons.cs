using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtons : MonoBehaviour
{
    //public Button b1, b2, b3, b4;
    public Button[] b;
    // Start is called before the first frame update
    //void Start()
    //{
    //    b[0].onClick.AddListener(s1);
    //    b[1].onClick.AddListener(s2);
    //    b[2].onClick.AddListener(s3);
    //    b[3].onClick.AddListener(s4);
    //    string[] temp;
    //    for (int i = 1; i < 5; i++)
    //    {
    //        temp = PlayerManager.Instance.getSkillInfo(i);
    //        b[i - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp[0];
    //        // grab desc for when hovering on button
    //    }
    //}

    public void initSkillButtons()
    {
        b[0].onClick.AddListener(s1);
        b[1].onClick.AddListener(s2);
        b[2].onClick.AddListener(s3);
        b[3].onClick.AddListener(s4);
        string[] temp;
        for (int i = 1; i < 5; i++)
        {
            temp = PlayerManager.Instance.getSkillInfo(i);
            b[i - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp[0];
            // grab desc for when hovering on button
        }
    }


    private void s1()
    {
        PlayerManager.Instance.useSkill(1);
    }
    private void s2()
    {
        PlayerManager.Instance.useSkill(2);
    }
    private void s3()
    {
        PlayerManager.Instance.useSkill(3);
    }
    private void s4()
    {
        PlayerManager.Instance.useSkill(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
