using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeDialogue : MonoBehaviour
{
    public GameObject DialoguePanel;
    public GameObject TextBox;
    public Button[] Choices;
    public Dialogue dialogue;

    public GameObject getTextBox()
    {
        return TextBox;
    }

    public Button[] getChoices()
    {
        return Choices;
    }

    public Button getChoice01()
    {
        return Choices[0];
    }

    public Button getChoice02()
    {
        return Choices[1];
    }

    public Button getChoice03()
    {
        return Choices[2];
    }
}
