using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtons : MonoBehaviour
{
    public Button[] b;
    public GameObject[] cdText;
    public TextMeshProUGUI tooltip;

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
            b[i - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i + ": " + temp[0];
            // grab desc for when hovering on button
        }

        for (int i = 0; i < 4; i++)
            cdText[i].GetComponent<TextMeshProUGUI>().text = "0";
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

    // Turn each button on/off depending on the cd
    public void updateButtons(int[] cds)
    {
        if (cds[0] > 0)
            b[0].interactable = false;
        else
            b[0].interactable = true;

        if (cds[1] > 0)
            b[1].interactable = false;
        else
            b[1].interactable = true;

        if (cds[2] > 0)
            b[2].interactable = false;
        else
            b[2].interactable = true;

        if (cds[3] > 0)
            b[3].interactable = false;
        else
            b[3].interactable = true;

        for (int i = 0; i < 4; i ++)
        {
            //if (cds[i] == 0)
            //    cdText[i].GetComponent<TextMeshProUGUI>().text = "r";
            //else
            cdText[i].GetComponent<TextMeshProUGUI>().text = cds[i].ToString();

        }
    }

    public void writeToolTip1()
    {
        tooltip.text = PlayerManager.Instance.pc.skill1desc;
    }
    public void writeToolTip2()
    {
        tooltip.text = PlayerManager.Instance.pc.skill2desc;
    }
    public void writeToolTip3()
    {
        tooltip.text = PlayerManager.Instance.pc.skill3desc;
    }
    public void writeToolTip4()
    {
        tooltip.text = PlayerManager.Instance.pc.skill4desc;
    }
    public void clearToolTip()
    {
        tooltip.text = "";
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
