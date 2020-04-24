using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private GameManager gm;
    private OverworldManager om;
    private int nodeID, tutorialPictureIndex;
    private GameObject activeTutorial;
    public GameObject OverworldTutorial, CombatTutorial;
    public Button next, prev;
    public Toggle toggle;
    public Image owBasics, skBasics, comBasics, background;
    private bool owRead, diceRead, combatRead, waiting, allDone;

    // Start is called before the first frame update
    void Start()
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.om = GameObject.Find("GameManager").GetComponent<OverworldManager>();
        activeTutorial = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allDone && !this.gm.hideTutorial)
        {
            if (this.om.curDialogueNode != null)
            {
                this.nodeID = this.om.curDialogueNode.nodeId;
            }
            if (this.nodeID == 0 && !owRead)
            {
                owRead = true;
                StartCoroutine(popup(OverworldTutorial));
            }

            if (this.nodeID == 9 && !combatRead && this.gm.pm.inCombat) // First combat left
            {
                combatRead = true;
                StartCoroutine(popup(CombatTutorial));
            }

            if ((this.nodeID == 17 || this.nodeID == 26 || this.nodeID == 21) && !combatRead && this.gm.pm.inCombat) // First combat right
            {
                combatRead = true;
                StartCoroutine(popup(CombatTutorial));
            }

            if (owRead && combatRead)
                allDone = true;
        }
    }

    private IEnumerator popup(GameObject tutorial)
    {
        this.background.gameObject.SetActive(true);
        tutorial.SetActive(true);
        tutorial.transform.GetChild(0).gameObject.SetActive(true);
        this.activeTutorial = tutorial;
        this.tutorialPictureIndex = 0;
        prev.interactable = false;
        if (tutorial.transform.childCount > 2)
            next.interactable = true;
        waiting = true;
        while(waiting)
        {
            Time.timeScale = 0;
            yield return null;
        }
    }

    public void nextPicture()
    {

        if (tutorialPictureIndex + 1 < activeTutorial.transform.childCount - 1 && activeTutorial.transform.GetChild(tutorialPictureIndex+1) != null)
        {
            activeTutorial.transform.GetChild(tutorialPictureIndex).gameObject.SetActive(false);
            activeTutorial.transform.GetChild(++tutorialPictureIndex).gameObject.SetActive(true);
            prev.interactable = true;
        }

        if (tutorialPictureIndex == activeTutorial.transform.childCount - 2)
            next.interactable = false;
    }

    public void prevPicture()
    {
        if (tutorialPictureIndex - 1 >= 0 && activeTutorial.transform.GetChild(tutorialPictureIndex - 1) != null)
        {
            activeTutorial.transform.GetChild(tutorialPictureIndex).gameObject.SetActive(false);
            activeTutorial.transform.GetChild(--tutorialPictureIndex).gameObject.SetActive(true);
            next.interactable = true;
        }

        if (tutorialPictureIndex == 0)
            prev.interactable = false;
    }

    public void resumeGame()
    {
        waiting = false;
        OverworldTutorial.SetActive(false);
        CombatTutorial.SetActive(false);
        this.background.gameObject.SetActive(false);

        this.activeTutorial = null;
        this.tutorialPictureIndex = 0;

        Time.timeScale = 1f;
    }

    public void tutorialToggle()
    {
        if (this.gm != null)
        {
            if (toggle.isOn)
                this.gm.hideTutorial = true;
            else
                this.gm.hideTutorial = false;
        }
    }
}
