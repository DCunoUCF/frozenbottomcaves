// MenuManager
// Written By: Christopher Walen

// Very rudimentary Menu Control, also handles every menu button in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum UIType
{
	None = -1,
    NewGame, Continue, Options, Exit, Back,
	WizardClass, KnightClass, RogueClass, MonkClass
}

public enum SliderType
{
    None = -1,
    MusicLevel, EffectLevel
}

public class MenuManager : MonoBehaviour
{
	public UIType type;
    public SliderType sliderType;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    			OpenCharacterSelect();
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

    public void SliderAction()
    {
        switch (sliderType)
        {
            case SliderType.MusicLevel:
                this.gm.sm.setMusicVolume(GameObject.Find("MusicSlider").GetComponent<Slider>().value);
                Debug.Log("Set value of Music Channel!");
                break;
            case SliderType.EffectLevel:
                this.gm.sm.setEffectVolume(GameObject.Find("EffectSlider").GetComponent<Slider>().value);
                Debug.Log("Set value of Effect Channel!");
                break;
            default:
                break;
        }
    }

    void OpenCharacterSelect()
    {
    	SceneManager.LoadScene("CharacterSelect", LoadSceneMode.Single);
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
