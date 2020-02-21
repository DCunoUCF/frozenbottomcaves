using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static int currentNode = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        // Will change this to a static load so that we don't have to initialize
        Program p = new Program();

        // Loads the file
        dialogue = p.LoadFile("./Assets/Resources/Dialogue/tutorial.txt");

        // Adds Listeners to the options
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Dialogue text
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        for(int i = 0; i < 3; i++)
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
        
        for(int i = 0; i < 3; i++)
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

    private void DialogueSizer()
    {
        RectTransform panelRect = Panel.GetComponent<RectTransform>();
        RectTransform dialogueRect = TextBox.GetComponent<RectTransform>();
        List<RectTransform> optionRect = new List<RectTransform>();
        RectTransform option1Rect = Choices[0].GetComponent<RectTransform>();
        RectTransform option2Rect = Choices[1].GetComponent<RectTransform>();
        RectTransform option3Rect = Choices[2].GetComponent<RectTransform>();

        if (Choices[2].IsActive())
            optionRect.Add(option3Rect);
        if (Choices[1].IsActive())
            optionRect.Add(option2Rect);
        if (Choices[0].IsActive())
            optionRect.Add(option1Rect);

        int numChars = TextBox.GetComponent<Text>().text.Length;
        int fontSize = TextBox.GetComponent<Text>().fontSize;
        int winHeightBuffer = 20;
        int optionBuffer = (int) ((option1Rect.rect.height) + winHeightBuffer);

        // Char Height/Width based on font size. Bonus magic buffer numbers!
        float charHeight = fontSize + 4;
        float charWidth = (fontSize / 2) + 1;

        // Number of lines
        int charsPerLine = Mathf.CeilToInt((float)dialogueRect.rect.width / (float)charWidth);
        int numLines = (int)((float)numChars / (float)charsPerLine);

        // Resize Dialogue Box by only the new height
        dialogueRect.sizeDelta = new Vector2(dialogueRect.rect.width, Mathf.CeilToInt((float)numLines * charHeight));

        // Move dialogue options beneath the dialogue box

        float newPanelTopY = dialogueRect.transform.localPosition.y + (dialogueRect.rect.height / 2) + winHeightBuffer;
        float newPanelBotY = optionBuffer*optionRect.Count;
        panelRect.sizeDelta = new Vector2(panelRect.rect.width, newPanelTopY + newPanelBotY);

        dialogueRect.anchoredPosition = new Vector2(0,(panelRect.rect.height / 2) - (dialogueRect.rect.height/2) - winHeightBuffer);
        
        for (int i = 0; i < optionRect.Count; i++)
        {
            optionRect[i].anchoredPosition = new Vector2(0, (-1*((panelRect.rect.height / 2) - (optionRect[i].rect.height / 2) - winHeightBuffer - optionBuffer*i)));
        }

        // Debugging
        //print("dialogueRect.transform.localPosition.y:" + dialogueRect.transform.localPosition.y + " dialogueRect.rect.height / 2:" + (dialogueRect.rect.height / 2));
        //print("option3Rect.rect.position.y:" + option3Rect.transform.localPosition.y + " option3Rect.rect.height / 2:" + (option3Rect.rect.height / 2));
        //print("newPanelTopY:" + newPanelTopY + " newPanelBotY:" + newPanelBotY);
        //Debug.Log("dialogueRect.rect.width:" + dialogueRect.rect.width + " charWidth:" + charWidth);
        //Debug.Log("numChars:" + numChars + " magicCharsPerLine:" + charsPerLine);
        //Debug.Log("numLines:" + numLines + " charHeight:" + charHeight);
        //Debug.Log("Setting dialogue box height: " + dialogueRect.rect.height);
    }
}
