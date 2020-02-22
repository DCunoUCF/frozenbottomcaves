using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{

    // Game Objects
    public GameObject Manager;
    public GameObject Panel;
    public GameObject TextBox;
    public Button[] Choices;
    public Dialogue dialogue;

    public GameObject ContinueButton;

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
        // Will change this to a static load so that we don't have to initialize
        Program p = new Program();

        // Loads the file
        dialogue = p.LoadFile("./Assets/Resources/Dialogue/tutorial_emptynodes.txt");

        // Adds Listeners to the options
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Dialogue text
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < 3; i++)
        {
            Choices[i].gameObject.SetActive(false);
        }

        // Dialogue Choices
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        DialogueSizer();
    }

    // Run if user clicks first choice
    public void choiceOption01()
    {
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
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        DialogueSizer();
    }

    // Run if user clicks second choice
    public void choiceOption02()
    {
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
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        DialogueSizer();
    }

    // Run if user clicks third choice
    public void choiceOption03()
    {
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
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        DialogueSizer();
    }

    public void EventComplete()
    {
        if (currentNode == -1)
        {
            this.SetPanelAndChildrenFalse();
            return;
        }

        this.SetChildrenFalse();

        this.SetChildrenTrue();

        this.DialogueSizer();
    }

    public void SetChildrenTrue()
    {
        TextBox.SetActive(true);
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
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

    public void DialogueSizer()
    {
        // Components for days
        RectTransform panelRect = Panel.GetComponent<RectTransform>();
        RectTransform dialogueRect = TextBox.GetComponent<RectTransform>();
        List<RectTransform> optionRect = new List<RectTransform>();
        RectTransform option1Rect = Choices[0].GetComponent<RectTransform>();
        RectTransform option2Rect = Choices[1].GetComponent<RectTransform>();
        RectTransform option3Rect = Choices[2].GetComponent<RectTransform>();
        int numChars = TextBox.GetComponent<Text>().text.Length;
        int fontSize = TextBox.GetComponent<Text>().fontSize;

        // Only adding active buttons
        if (Choices[2].IsActive())
            optionRect.Add(option3Rect);
        if (Choices[1].IsActive())
            optionRect.Add(option2Rect);
        if (Choices[0].IsActive())
            optionRect.Add(option1Rect);
        //for (int i = 0; i < this.Choices.Count; i++)
        //{
        //    optionRect.Add(Choices[i].GetComponent<RectTransform>());
        //}

        // Buffers
        int winHeightBuffer = 20;
        int middleBuffer = winHeightBuffer / 2;
        int optionBuffer = (int) ((optionRect[0].rect.height) + middleBuffer);

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
        float newPanelTopY = dialogueRect.rect.height + winHeightBuffer*2;
        float newPanelBotY = optionBuffer*optionRect.Count;
        panelRect.sizeDelta = new Vector2(panelRect.rect.width, newPanelTopY + newPanelBotY);

        // Readjust Dialogue box location to be buffer from top of Panel
        dialogueRect.anchoredPosition = new Vector2(0,(panelRect.rect.height / 2) - (dialogueRect.rect.height/2) - winHeightBuffer);
        
        // Readjust each option button to be buffer from bottom of Panel
        for (int i = 0; i < optionRect.Count; i++)
        {
            optionRect[i].anchoredPosition = new Vector2(0, (-1*((panelRect.rect.height / 2) - (optionRect[i].rect.height / 2) - winHeightBuffer - optionBuffer * i)));
        }
    }
}

//// David's Start Method
//void Start()
//{
//    // Will change this to a static load so that we don't have to initialize
//    Program p = new Program();

//    // Loads the file
//    dialogue = p.LoadFile("./Assets/Resources/Dialogue/tutorial_emptynodes.txt");
//    currentOptionsCount = this.dialogue.nodes[currentNode].options.Count;
//    optionParent = GameObject.Find("Option01");

//    Choices = new List<Button>(currentOptionsCount);

//    // Adds Listeners to the options
//    for (int i = 0; i < Choices.Count; i++)
//    {
//        Choices.Add(GameObject.Instantiate(optionParent.GetComponent<Button>(), Panel.transform));
//    }

//    choiceCounter = 0;
//    foreach (Button button in Choices)
//    {
//        int i = choiceCounter++;
//        button.onClick.AddListener(() => this.ChoiceOption(i));
//    }

//    // Dialogue text
//    TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

//    for(int i = 0; i < Choices.Count; i++)
//    {
//        Choices[i].gameObject.SetActive(false);
//    }

//    // Dialogue Choices
//    for (int i = 0; i < currentOptionsCount; i++)
//    {
//        Choices[i].gameObject.SetActive(true);
//        Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
//    }

//    optionParent.SetActive(false);
//    DialogueSizer();
//}

//private void ChoiceOption(int choice)
//{
//    this.currentNode = this.dialogue.nodes[currentNode].options[choice].destId;

//    if (currentNode == -1)
//    {
//        this.SetPanelAndChildrenFalse();
//        return;
//    }

//    this.SetChildrenFalse();

//    this.SetChildrenTrue();

//    this.DialogueSizer();
//}
