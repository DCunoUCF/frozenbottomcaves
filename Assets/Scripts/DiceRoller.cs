using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{

    // Array of dice sides sprites to load from Resources folder
    //private Sprite[] diceSides;
    private Sprite[] diceSides;
    // Reference to sprite renderer to change sprites
    private Image img;

    public Button b;

    public bool clicked, rolling;

    public int final;
    // Use this for initialization
    private void Start()
    {
        img = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        b.onClick.AddListener(click);

        //StartCoroutine(RollTheDice());

    }

    private void click()
    {
        this.clicked = true;
    }

    private void OnMouseDown()
    {
        if (rolling)
            clicked = true;
    }

    // Coroutine that rolls the dice
    public IEnumerator RollTheDice()
    {
        rolling = true;
        this.gameObject.SetActive(true);
        b.interactable = true;

        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;
        int prevRoll = 4;
        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 6);
            while(randomDiceSide == prevRoll)
            {
                randomDiceSide = Random.Range(0, 6);
            }

            // Set sprite to upper face of dice from array according to random value
            
            img.sprite = diceSides[randomDiceSide];

            if (clicked)
                break;

            // Pause before next itteration
            yield return new WaitForSeconds(0.1f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log(finalSide);
        final = finalSide;
        b.interactable = false;
        yield break;
    }
}