using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RollMaster : MonoBehaviour
{
    public GameObject d1, d2;
    public DiceRoller d1r, d2r;
    public Button d1b, d2b, endCheck, startRoll;
    public TextMeshProUGUI modText, resText, limText;
    private bool waiting, clickd1, clickd2;


    // Start is called before the first frame update
    void Start()
    {
        d1b.onClick.AddListener(d1Click);
        d2b.onClick.AddListener(d2Click);
        startRoll.onClick.AddListener(sRoll);
        endCheck.onClick.AddListener(eRoll);

        d1r = d1.GetComponent<DiceRoller>();
        d2r = d2.GetComponent<DiceRoller>();

        d1b.interactable = false;
        d2b.interactable = false;
        startRoll.interactable = false;

        waiting = true;
    }

    private void d1Click()
    {
        clickd1 = true;
    }

    private void d2Click()
    {
        clickd2 = true;
    }

    private void sRoll()
    {
        waiting = false;
        startRoll.interactable = false;
        startRoll.gameObject.SetActive(false);
        endCheck.interactable = false;
        endCheck.gameObject.SetActive(true);
    }

    private void eRoll()
    {
        waiting = true;
        endCheck.interactable = false;
        endCheck.gameObject.SetActive(false);
        startRoll.gameObject.SetActive(true);
    }

    public IEnumerator waitForStart(string atribute, int modifier, int difficulty)
    {
        startRoll.interactable = true;
        limText.text = "Need: " + difficulty;
        modText.text = "+ " + atribute + "(" + modifier + ")";
        while (waiting)
        {
            yield return new WaitForSeconds(.1f);
        }
        yield return StartCoroutine(d1r.RollTheDice());
        yield return StartCoroutine(d2r.RollTheDice());
        resText.text = "Rolled: " + (d1r.final + d2r.final + modifier);

        yield return StartCoroutine(waitForEnd());
    }

    public IEnumerator waitForEnd()
    {
        endCheck.interactable = true;
        while (!waiting){ yield return new WaitForSeconds(.1f); }
        yield break;
    }
}
