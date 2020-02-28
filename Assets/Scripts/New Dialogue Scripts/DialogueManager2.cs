using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager2 : MonoBehaviour
{
    public GameObject Manager;
    public GameObject Canvas;
    public GameObject DialoguePanel;
    public GameObject TextBox;
    public GameObject ScrollListContent;
    public Button[] Choices;
    public Dialogue dialogue;

    private GameObject CurrentPanel;
    public GameObject ContinueButton;
    public static int currentNode = 0;

    // Start is called before the first frame update
    void Start()
    {
        Program p = new Program();
        dialogue = p.LoadFile("./Assets/Resources/Dialogue/tutorial.txt");

        // Add Listeners
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        // Start Choices as Inactive
        for (int i = 0; i < 3; i++)
            Choices[i].gameObject.SetActive(false);

        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }
    }

    // Do this if user clicks 1st option
    public void choiceOption01()
    {

        // This is where we currently are in the dialogue
        currentNode = dialogue.nodes[currentNode].options[0].destId;

        // If the current node is -1, end dialogue.
        if (currentNode == -1)
        {
            CurrentPanel.SetActive(false);
            return;
        }

        // Initialize new Dialogue
        CurrentPanel = Instantiate(DialoguePanel);
        CurrentPanel.SetActive(true);

        // Set the parent to Canvas
        CurrentPanel.transform.SetParent(ScrollListContent.transform, false);

        // Get text box of new dialogue
        GameObject newTextBox = CurrentPanel.GetComponent<InitializeDialogue>().getTextBox();

        // Add text of currentNode to new Dialogue
        newTextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;


        // Remove Listeners
        for(int i = 0; i < 3; i++)
        {
            Choices[i].onClick.RemoveAllListeners();
        }

        // Reset Choices to new Dialogue
        Choices = CurrentPanel.GetComponentsInChildren<Button>();
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Start the New Buttons as Inactive
        for (int i = 0; i < 3; i++)
            Choices[i].gameObject.SetActive(false);

        // Set Text to New Buttons
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        // This is your script David
        // DialogueSizer();

        GameObject.Find("ScrollList").GetComponent<ScrollRect>().velocity = new Vector2(0f, 1000f);

    }

    // Do this if user clicks 2nd option
    public void choiceOption02()
    {
        // This is where we currently are in the dialogue
        currentNode = dialogue.nodes[currentNode].options[1].destId;

        // If the current node is -1, end dialogue.
        if (currentNode == -1)
        {
            CurrentPanel.SetActive(false);
            return;
        }

        // Initialize new Dialogue
        CurrentPanel = Instantiate(DialoguePanel);
        CurrentPanel.SetActive(true);

        // Set the parent to Canvas
        CurrentPanel.transform.SetParent(ScrollListContent.transform, false);

        // Get text box of new dialogue
        GameObject newTextBox = CurrentPanel.GetComponent<InitializeDialogue>().getTextBox();

        // Add text of currentNode to new Dialogue
        newTextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;


        // Remove Listeners
        for (int i = 0; i < 3; i++)
        {
            Choices[i].onClick.RemoveAllListeners();
        }

        // Reset Choices to new Dialogue
        Choices = CurrentPanel.GetComponentsInChildren<Button>();
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Start the New Buttons as Inactive
        for (int i = 0; i < 3; i++)
            Choices[i].gameObject.SetActive(false);

        // Set Text to New Buttons
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        // This is your script David
        // DialogueSizer();

        GameObject.Find("ScrollList").GetComponent<ScrollRect>().velocity = new Vector2(0f, 1000f);
    }

    // Do this if user clicks 3rd option
    public void choiceOption03()
    {
        // This is where we currently are in the dialogue
        currentNode = dialogue.nodes[currentNode].options[2].destId;

        // If the current node is -1, end dialogue.
        if (currentNode == -1)
        {
            CurrentPanel.SetActive(false);
            return;
        }

        // Initialize new Dialogue
        CurrentPanel = Instantiate(DialoguePanel);
        CurrentPanel.SetActive(true);

        // Set the parent to Canvas
        CurrentPanel.transform.SetParent(ScrollListContent.transform, false);

        // Get text box of new dialogue
        GameObject newTextBox = CurrentPanel.GetComponent<InitializeDialogue>().getTextBox();

        // Add text of currentNode to new Dialogue
        newTextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;


        // Remove Listeners
        for (int i = 0; i < 3; i++)
        {
            Choices[i].onClick.RemoveAllListeners();
        }

        // Reset Choices to new Dialogue
        Choices = CurrentPanel.GetComponentsInChildren<Button>();
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Start the New Buttons as Inactive
        for (int i = 0; i < 3; i++)
            Choices[i].gameObject.SetActive(false);

        // Set Text to New Buttons
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }

        // This is your script David
        // DialogueSizer();

        GameObject.Find("ScrollList").GetComponent<ScrollRect>().velocity = new Vector2(0f, 1000f);

    }

    private void DialogueSizer()
    {
        RectTransform panelRect = DialoguePanel.GetComponent<RectTransform>();
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
        int middleBuffer = winHeightBuffer / 2;
        int optionBuffer = (int)((option1Rect.rect.height) + winHeightBuffer);

        // Char Height/Width based on font size. Bonus magic buffer numbers!
        float charHeight = fontSize + 4;
        float charWidth = (fontSize / 2) + 1;

        // Number of lines
        int charsPerLine = Mathf.CeilToInt((float)dialogueRect.rect.width / (float)charWidth);
        int numLines = Mathf.CeilToInt((float)numChars / (float)charsPerLine);

        // Resize Dialogue Box by only the new height
        dialogueRect.sizeDelta = new Vector2(dialogueRect.rect.width, Mathf.CeilToInt((float)numLines * charHeight));

        // Move dialogue options beneath the dialogue box

        float newPanelTopY = dialogueRect.transform.localPosition.y + (dialogueRect.rect.height / 2) + winHeightBuffer + middleBuffer;
        float newPanelBotY = optionBuffer * optionRect.Count;
        panelRect.sizeDelta = new Vector2(panelRect.rect.width, newPanelTopY + newPanelBotY);

        dialogueRect.anchoredPosition = new Vector2(0, (panelRect.rect.height / 2) - (dialogueRect.rect.height / 2) - winHeightBuffer);

        for (int i = 0; i < optionRect.Count; i++)
        {
            optionRect[i].anchoredPosition = new Vector2(0, (-1 * ((panelRect.rect.height / 2) - (optionRect[i].rect.height / 2) - winHeightBuffer - optionBuffer * i)));
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
