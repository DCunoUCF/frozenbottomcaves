// MenuManager
// Written By: Christopher Walen

// Very rudimentary Menu Control, also handles every menu button in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum UIType
{
	NewGame, Continue, Options, Exit, Back
}

public class MenuManager : MonoBehaviour
{
	public UIType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonAction()
    {
    	switch (type)
    	{
    		case UIType.NewGame:
    			Debug.Log("Clicked new game!");
    			break;
    		case UIType.Continue:
    			Debug.Log("Clicked continue!");
    			break;
    		case UIType.Options:
    			Debug.Log("Clicked options!");
    			OpenOptions();
    			break;
    		case UIType.Exit:
    			Debug.Log("Clicked exit!");
    			QuitGame();
    			break;
    		case UIType.Back:
    			Debug.Log("Clicked back!");
    			// ExitOptions();
    			ReturnToMainMenu();
    			break;
    		default:
    			Debug.Log("Clicked a button!"); break;
    	}
    }

    void OpenOptionsOnTop()
    {
    	SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    void OpenOptions()
    {
    	SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Single);
    }

    void ExitOptions()
    {
    	SceneManager.UnloadSceneAsync("OptionsMenu");
    }

    void ReturnToMainMenu()
    {
    	SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void QuitGame()
    {
    	Application.Quit();
    }
}
