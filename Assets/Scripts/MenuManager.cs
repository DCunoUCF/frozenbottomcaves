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
    NewGame, Continue, Options, Exit, Back, Return, Restart,
	WizardClass, KnightClass, RogueClass, MonkClass,
    MusicMute, EffectMute
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

        if (SceneManager.GetActiveScene().name == "OptionsMenu")
        {
            GameObject.Find("MusicSlider").GetComponent<Slider>().value = this.gm.sm.getMusicVolume();
            GameObject.Find("EffectSlider").GetComponent<Slider>().value = this.gm.sm.getEffectVolume();
            GameObject.Find("MusicMuter").GetComponent<Toggle>().isOn = this.gm.sm.getMusicMute();
            GameObject.Find("EffectMuter").GetComponent<Toggle>().isOn = this.gm.sm.getEffectMute();
        }
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
    			Debug.Log("Clicked new game! REMEMBER TO CHANGE BACK TO MOVING TO CHARACTERSELECT");
               // OpenDemoLevel();
               // this.gm.sm.setBattleMusic();
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
                // this.gm.sm.setMusicFromDirectory("ForestOverworldMusic");
    			ReturnToMainMenu();
    			break;
            case UIType.Return:
                // TODO: change to Go back to Overworld
            case UIType.Restart:
                Debug.Log("Clicked return!");
                // this.gm.sm.setForestMusic();
                this.gm.sm.setMusicFromDirectory("ForestOverworldMusic");
                ReturnToMainMenu();
                break;
            case UIType.MusicMute:
                Debug.Log("Muting music!");
                if (this.gm != null)
                {
                    this.gm.sm.setMusicMute(GameObject.Find("MusicMuter").GetComponent<Toggle>().isOn);
                }
                break;
            case UIType.EffectMute:
                Debug.Log("Muting effects!");
                if (this.gm != null)
                {
                    this.gm.sm.setEffectMute(GameObject.Find("EffectMuter").GetComponent<Toggle>().isOn);
                }
                break;
            case UIType.KnightClass:
                Debug.Log("Selected Knight!");
                gm.pm.playerScript = CharacterSelection.writeStats("Knight.txt");
                //OpenDemoLevel();
                //this.gm.sm.setBattleMusic();
                //gm.pm.combatInitialized = true;
                //gm.pm.inCombat = true;
                OpenOverworld();
                break;
            case UIType.WizardClass:
                OpenDemoLevel();
                // this.gm.sm.setBattleMusic();
                this.gm.sm.setMusicFromDirectory("ForestBattleMusic");
                gm.pm.combatInitialized = true;
                gm.pm.inCombat = true;
                break;
            case UIType.MonkClass:
                gm.pm.playerScript = CharacterSelection.writeStats("Monk.txt");
                OpenOverworld();
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
                Slider ms = GameObject.Find("MusicSlider").GetComponent<Slider>();

                if (ms != null && this.gm != null)
                {
                    this.gm.sm.setMusicVolume(ms.value);
                    Debug.Log("Set value of Music Channel!");
                }
                break;
            case SliderType.EffectLevel:
                Slider es = GameObject.Find("EffectSlider").GetComponent<Slider>();

                if (es != null && this.gm != null)
                {
                    this.gm.sm.setEffectVolume(es.value);
                    Debug.Log("Set value of Effect Channel!");
                }
                break;
            default:
                break;
        }
    }

    void OpenDemoLevel()
    {
        SceneManager.LoadScene("Battleworld", LoadSceneMode.Single);
    }

    void OpenOverworld()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
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
