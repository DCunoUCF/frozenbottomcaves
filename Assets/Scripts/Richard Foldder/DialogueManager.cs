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
        dialogue = p.LoadFile("sample.txt");

        // Adds Listeners to the options
        Choices[0].onClick.AddListener(choiceOption01);
        Choices[1].onClick.AddListener(choiceOption02);
        Choices[2].onClick.AddListener(choiceOption03);

        // Dialogue text
        TextBox.GetComponent<Text>().text = dialogue.nodes[currentNode].text;

        // Dialogue Choices
        for (int i = 0; i < dialogue.nodes[currentNode].options.Count; i++)
        {
            Choices[i].gameObject.SetActive(true);
            Choices[i].GetComponent<Button>().GetComponentInChildren<Text>().text = dialogue.nodes[currentNode].options[i].text;
        }
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

    }
}
