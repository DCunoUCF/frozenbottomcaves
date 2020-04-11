using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    // Game Objects
    public GameObject Manager;
    public GameObject Panel;
    public GameObject backgroundImage;
    public GameObject TextBox;
    public Button[] Choices;
    public Dialogue dialogue;
    public GameObject dialogueCanvas;

    public GameObject ContinueButton;

    public OverworldManager om;
    public WorldNode curNode;
    public bool waiting;
    public bool[] inactiveButtons; // remember inactive buttons
    public List<Button> bTemp;

    // Keeps track of position in dialogue
    // public static int currentNode = 0;
    public int currentNode = 0;

    // David's Trash for Multiple Options Testing
    //public List<Button> Choices;
    //public int choiceCounter;
    //public GameObject optionParent;
    //public int currentOptionsCount;

    void Start()
    {
        // Hardcoding Sort Order is absolutely necessary
        TextBox.GetComponent<MeshRenderer>().sortingOrder = 4;
        Choices[0].GetComponent<Button>().GetComponentInChildren<MeshRenderer>().sortingOrder = 4;
        Choices[1].GetComponent<Button>().GetComponentInChildren<MeshRenderer>().sortingOrder = 4;
        Choices[2].GetComponent<Button>().GetComponentInChildren<MeshRenderer>().sortingOrder = 4;
        backgroundImage.GetComponent<MeshRenderer>().sortingOrder = 3;
        // Will change this to a static load so that we don't have to initialize
        Program p = new Program();

        // Loads the file
        // dialogue = p.LoadFile(Application.streamingAssetsPath + "/tutorial.txt");
        dialogue = p.LoadFile("Dialogue/dialogue_new_format");

        // Adds Listeners to the options
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        inactiveButtons = new bool[3];
        bTemp = new List<Button>();

        init();
        // Dialogue text
        //TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        //for (int i = 0; i < 3; i++)
        //{
        //    Choices[i].gameObject.SetActive(false);
        //}

        //// Dialogue Choices
        //for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        //{
        //    Choices[i].gameObject.SetActive(true);
        //    Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
        //}

        DialogueSizer();
    }

    public void init()
    {
        TextBox.SetActive(true);
        TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        // Dialogue Choices
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
        }

        StartCoroutine(resizerDelayed());
    }

    // return proper index BACKUP WIP IF SOMETHING MAJOR BREAKS AGAIN
    //private int findNode(int ID)
    //{
    //    int index = 0;

    //    for (int i = 0; i < dialogue.nodes.Count; i++)
    //    {

    //    }

    //    foreach (DialogueNode d in dialogue.nodes)
    //    {
    //        if (d.nodeId == ID)
    //        {
    //            index =
    //        }
    //    }



    //    return index;
    //}

    // Run if user clicks first choice
    public void choiceOption01()
    {
        setInteractableAll();
        currentNode = dialogue.nodes[currentNode].options[0].destId;

        if (currentNode == -1)
        {
            TextBox.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                Choices[i].gameObject.SetActive(false);
            }

            Panel.SetActive(false);
            return;
        }

        TextBox.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        TextBox.SetActive(true);
        TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {

            for (int j = 0; j < om.nodes.Count; j++)
            {
                int nodeCount = 0;
                curNode = om.nodes[j].GetComponent<WorldNode>();
                foreach (int id in curNode.NodeIDs)
                {
                    if (id == dialogue.nodes[currentNode].options[i].destId)
                    {
                        if (curNode.NodeTypes[nodeCount] == FlagType.ItemLose)
                        {
                            if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item) == null)
                            {
                                print("item not found");
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item).count < curNode.NodeItemsLose[nodeCount].count)
                            {
                                print("insufficient item count" + id);
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else
                            {
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                            }
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(true);
                            Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                        }
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(true);
                        Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                    }
                    nodeCount++;
                }
            }

            //Choices[i].gameObject.SetActive(true);
            //Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
        }

        DialogueSizer();
        setInitialSelection();
    }

    // Run if user clicks second choice
    public void choiceOption02()
    {
        setInteractableAll();
        currentNode = dialogue.nodes[currentNode].options[1].destId;

        if (currentNode == -1)
        {
            TextBox.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                Choices[i].gameObject.SetActive(false);
            }

            Panel.SetActive(false);
            return;
        }

        TextBox.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        TextBox.SetActive(true);
        TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            print("NUM OPTIONS: " + dialogue.nodes[currentNode].options.Count);
            for (int j = 0; j < om.nodes.Count; j++)
            {
                int nodeCount = 0;
                curNode = om.nodes[j].GetComponent<WorldNode>();
                foreach (int id in curNode.NodeIDs)
                {
                    if (id == dialogue.nodes[currentNode].options[i].destId)
                    {
                        if (curNode.NodeTypes[nodeCount] == FlagType.ItemLose)
                        {
                            if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item) == null)
                            {
                                print("item not found");
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item).count <= curNode.NodeItemsLose[nodeCount].count)
                            {
                                print("insufficient item count" + id);
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else
                            {
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                            }
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(true);
                            Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                        }
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(true);
                        Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                    }
                    nodeCount++;
                }
            }
        }


        DialogueSizer();
        setInitialSelection();
    }

    // Run if user clicks third choice
    public void choiceOption03()
    {
        setInteractableAll();
        currentNode = dialogue.nodes[currentNode].options[2].destId;

        if (currentNode == -1)
        {
            TextBox.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                Choices[i].gameObject.SetActive(false);
            }

            Panel.SetActive(false);
            return;
        }

        TextBox.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        TextBox.SetActive(true);
        TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            for (int j = 0; j < om.nodes.Count; j++)
            {
                int nodeCount = 0;
                curNode = om.nodes[j].GetComponent<WorldNode>();
                foreach (int id in curNode.NodeIDs)
                {
                    if (id == dialogue.nodes[currentNode].options[i].destId)
                    {
                        if (curNode.NodeTypes[nodeCount] == FlagType.ItemLose)
                        {
                            if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item) == null)
                            {
                                print("item not found");
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item).count <= curNode.NodeItemsLose[nodeCount].count)
                            {
                                print("insufficient item count" + id);
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else
                            {
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                            }
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(true);
                            Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                        }
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(true);
                        Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                    }
                    nodeCount++;
                }
            }
        }

        DialogueSizer();
        setInitialSelection();
    }

    public void setInteractableAll()
    {
        foreach (Button b in Choices)
            b.interactable = true;
    }

    public void EventComplete()
    {
        print("curNode in DiaMan:" + this.currentNode);
        if (this.currentNode == -1)
        {
            this.SetPanelAndChildrenFalse();
            return;
        }

        this.SetChildrenFalse();

        this.SetChildrenTrue();

        this.DialogueSizer();
        print("curNode in DiaMan:" + this.currentNode);
    }

    public void SetChildrenTrue()
    {
        TextBox.SetActive(true);
        TextBox.GetComponent<TextMeshPro>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            for (int j = 0; j < om.nodes.Count; j++)
            {
                int nodeCount = 0;
                curNode = om.nodes[j].GetComponent<WorldNode>();
                foreach (int id in curNode.NodeIDs)
                {
                    if (id == dialogue.nodes[currentNode].options[i].destId)
                    {
                        print("Node we're checking for itemloss event" + id);

                        if (curNode.NodeTypes[nodeCount] == FlagType.ItemLose)
                        {
                            if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item) == null)
                            {
                                print("item not found");
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else if (this.om.gm.pm.pc.inventory.CheckItem(curNode.NodeItemsLose[nodeCount].item).count <= curNode.NodeItemsLose[nodeCount].count)
                            {
                                print("insufficient item count");
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                                Choices[i].interactable = false;
                            }
                            else
                            {
                                Choices[i].gameObject.SetActive(true);
                                Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                            }
                        }
                        else
                        {
                            Choices[i].gameObject.SetActive(true);
                            Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                        }
                    }
                    else
                    {
                        Choices[i].gameObject.SetActive(true);
                        Choices[i].GetComponent<Button>().GetComponentInChildren<TextMeshPro>().text = dialogue.nodes[currentNode].options[i].text;
                    }
                    nodeCount++;
                }
            }
        }
    }

    public void SetChildrenFalse()
    {
        TextBox.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }
    }

    public void SetPanelAndChildrenFalse()
    {
        TextBox.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        Panel.SetActive(false);
    }

    IEnumerator resizerDelayed()
    {
        yield return new WaitForSeconds(.01f);
        DialogueSizer();
    }

    public void DialogueSizer()
    {
        // Components for days
        RectTransform panelRect = Panel.GetComponent<RectTransform>();
        RectTransform dialogueRect = TextBox.GetComponent<RectTransform>();
        List<RectTransform> optionRect = new List<RectTransform>();
        RectTransform option1Rect = Choices[0].GetComponent<RectTransform>();
        RectTransform option2Rect = Choices[1].GetComponent<RectTransform>();
        RectTransform option3Rect = Choices[2].GetComponent<RectTransform>();
        int numChars = TextBox.GetComponent<TextMeshPro>().text.Length;
        //int fontSize = (int) TextBox.GetComponent<TextMeshPro>().fontSize; // Hardcoding to a reasonable number because of text mesh pro size 200 font
        int fontSize = 20;

        // Only adding active buttons
        if (Choices[2].IsActive())
            optionRect.Add(option3Rect);
        if (Choices[1].IsActive())
            optionRect.Add(option2Rect);

        optionRect.Add(option1Rect);
        //for (int i = 0; i < this.Choices.Count; i++)
        //{
        //    optionRect.Add(Choices[i].GetComponent<RectTransform>());
        //}

        // Buffers
        int winHeightBuffer = 20;
        int middleBuffer = winHeightBuffer / 2;
        int optionBuffer = (int)((optionRect[0].rect.height) + middleBuffer);

        // Char Height/Width based on font size. Increase width buffer to increase the number of rows. Increase heightBuffer to increase the height of the rows
        float heightBuffer = 4.0f, widthBuffer = 1.0f;
        float charHeight = fontSize + heightBuffer;
        float charWidth = (fontSize / 2) + widthBuffer;

        // Number of lines
        int charsPerLine = Mathf.CeilToInt((float)dialogueRect.rect.width / (float)charWidth);
        int numLines = Mathf.CeilToInt((float)numChars / (float)charsPerLine);

        // Resize Dialogue Box by only the new height
        dialogueRect.sizeDelta = new Vector2(dialogueRect.rect.width, Mathf.CeilToInt((float)numLines * charHeight));

        // Resize Panel box based on Dialogue box size, and number of options*option size + buffers
        float newPanelTopY = dialogueRect.rect.height + winHeightBuffer * 2;
        float newPanelBotY = optionBuffer * optionRect.Count;
        panelRect.sizeDelta = new Vector2(panelRect.rect.width, newPanelTopY + newPanelBotY);

        // Readjust Dialogue box location to be buffer from top of Panel
        dialogueRect.anchoredPosition = new Vector2(0, (panelRect.rect.height / 2) - (dialogueRect.rect.height / 2) - winHeightBuffer);

        // Readjust each option button to be buffer from bottom of Panel
        for (int i = 0; i < optionRect.Count; i++)
        {
            optionRect[i].anchoredPosition = new Vector2(0, (-1 * ((panelRect.rect.height / 2) - (optionRect[i].rect.height / 2) - winHeightBuffer - optionBuffer * i)));
        }
    }

    // Sets the initial choice selection as Option1
    // Called here to handle if we don't move and in OM after reaching a destination if moving since we disable the panel
    public void setInitialSelection()
    {
        Choices[0].Select();
        Choices[0].OnSelect(null);
    }

    // Used when the inventory is opened to prevent the dialogue options from being selectable/selected
    public void setUninteractable()
    {
        int i = 0;
        foreach (Button b in Choices)
        {
            if (b.interactable == true)
                bTemp.Add(b);
            b.interactable = false;
            i++;
        }
    }

    public void setInterableAll()
    {
        foreach (Button b in Choices)
            b.interactable = true;
    }

    // Opposite of above, as well as setting the first option back to the initial selection
    public void setInteractable()
    {
        //int i = 0;
        //foreach (Button b in Choices)
        //{
        //    if (inactiveButtons[i])
        //    {
        //        inactiveButtons[i] = false;
        //    }
        //    else
        //    {
        //        b.interactable = true;
        //    }
        //}
        if (!this.om.gm.pm.PLAYERDEAD)
        {
            foreach (Button b in bTemp)
            {
                b.interactable = true;
            }
            bTemp.Clear();
            setInitialSelection();
        }

    }

    public void putCanvasBehind()
    {
        print("CANVAS GOING BEHIND");
        dialogueCanvas.GetComponent<Canvas>().sortingOrder = -2;
    }

    public void putCanvasInFront()
    {
        dialogueCanvas.GetComponent<Canvas>().sortingOrder = 3;
    }

    private IEnumerator pause()
    {
        yield return new WaitForSeconds(.005f);
        waiting = true;
    }

}
