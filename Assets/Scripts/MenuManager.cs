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
    MusicMute, EffectMute,
    BattleButton, OptionsOnTop, OptionBack,
    LoadGame, MainMenuButton, OpenQuitPrompt, ResumeGame,
    HideHPBars, HideDMGNum, VsyncEnable
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
    		    OpenCharacterSelect();
    			break;
    		case UIType.Continue:
    			Debug.Log("Clicked continue!");
    			break;
            // Used on MainMenu
    		case UIType.Options:
    			Debug.Log("Clicked options!");
    			OpenOptionsOnTop();
                //StartCoroutine(OptionActive());
    			break;
            // Used in OW
            case UIType.OptionsOnTop:
                if (!this.gm.pm.inOptions)
                {
                    this.gm.pm.inventoryUI.gameObject.SetActive(false);
                    this.gm.om.dm.setUninteractable();
                    OpenOptionsOnTop();
                }
                else
                {
                    ExitOptions();
                    if (this.gm.om.playerSpawned)
                    {
                        // this.gm.pm.invImg.color = Color.gray;
                        //this.gm.pm.inOptions = false;
                        this.gm.om.dm.setInteractable();
                        this.gm.om.dm.setInitialSelection();
                    }
                }
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
            case UIType.OptionBack:
                ExitOptions();
                if (this.gm.om.playerSpawned)
                {
                    this.gm.om.dm.setInteractable();
                    this.gm.om.dm.setInitialSelection();
                }
                break;
            case UIType.Return:
                Debug.Log("Clicked return!");
                // TODO: change to Go back to Overworld
                // this.gm.sm.setMusicFromDirectory("ForestOverworldMusic");
                this.gm.sm.setForestMusic();
                gm.pm.inCombat = false;
                Destroy(GameObject.Find("SceneCleaner"));
                ExitBattle();
                break;
            case UIType.Restart:
                this.gm.sm.setForestMusic();
                // this.gm.sm.setMusicFromDirectory("ForestOverworldMusic");
                ReturnToMainMenu();
                break;
            case UIType.BattleButton:
                OpenDemoLevel();
                this.gm.sm.setBattleMusic();
                // this.gm.sm.setMusicFromDirectory("ForestBattleMusic");
                gm.pm.combatInitialized = true;
                gm.pm.inCombat = true;
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
                gm.pm.pc = CharacterSelection.writeStats("Knight");
                OpenOverworld();
                break;
            case UIType.WizardClass:
                gm.pm.pc = CharacterSelection.writeStats("Wizard");
                OpenOverworld();
                break;
            case UIType.MonkClass:
                gm.pm.pc = CharacterSelection.writeStats("Monk");
                OpenOverworld();
                break;
            case UIType.RogueClass:
                gm.pm.pc = CharacterSelection.writeStats("Ninja");
                OpenOverworld();
                break;
            case UIType.LoadGame:
                this.gm.sm.setForestMusic();
                loadGame();
                break;
            case UIType.MainMenuButton:
                ReturnToMainMenuFromGame();
                break;
            case UIType.OpenQuitPrompt:
                openQuitPrompt();
                break;
            case UIType.ResumeGame:
                closeQuitPrompt();
                break;
            case UIType.HideHPBars:
                this.gm.showHPbars = GameObject.Find("HPBarToggle").GetComponent<Toggle>().isOn;
                SaveData.hpBar = GameObject.Find("HPBarToggle").GetComponent<Toggle>().isOn;
                break;
            case UIType.HideDMGNum:
                this.gm.showDMGnums = GameObject.Find("DMGNumToggle").GetComponent<Toggle>().isOn;
                SaveData.dmgNum = GameObject.Find("DMGNumToggle").GetComponent<Toggle>().isOn;
				break;
            case UIType.VsyncEnable:
                this.gm.vsyncEnabled = GameObject.Find("VSyncToggle").GetComponent<Toggle>().isOn;
                SaveData.vSync = GameObject.Find("VSyncToggle").GetComponent<Toggle>().isOn;
                break;
            default:
    			Debug.Log("Clicked a button!"); 
                break;
    	}
    }

    void openQuitPrompt()
    {
        if (this.gm.pm.inOptions)
        {
            ExitOptions();
        }
        if (this.gm.pm.inventoryUI.gameObject.activeSelf)
        {
            this.gm.pm.inventoryOpen();
        }
        this.gm.quitUp = true;
        this.gm.om.dm.setUninteractable();
        this.gm.pm.uiParent.SetActive(false);
        SceneManager.LoadScene("QuitPopup", LoadSceneMode.Additive);
    }

    void closeQuitPrompt()
    {
        this.gm.quitUp = false;
        this.gm.om.dm.setInteractable();
        this.gm.pm.uiParent.SetActive(true);
        SceneManager.UnloadSceneAsync("QuitPopup");
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

    IEnumerator OptionActive()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            disableMainMenuButtons();
            yield return null;
            GameObject.Find("OptionsCanvas").transform.Find("FakeBackground").gameObject.SetActive(true);
        }
        else if (this.gm.pm.inCombat)
        {
            disableCombatButtons();
            yield return null;
        }
        else if (this.gm.om.playerSpawned)
        {
            PlayerManager.Instance.optImg.color = Color.gray;
            ButtonOverlay.Instance.opt = true;
        }

        this.gm.pm.inOptions = true;

        //GameObject.Find("OptionsCanvas").GetComponent<Canvas>().worldCamera = GameObject.Find("MainCameraMM").GetComponent<Camera>();

        //yield return new WaitForSeconds(.001f);
        yield return null; // Wait 1 frame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("OptionsMenu"));
        GameObject.Find("MusicSlider").GetComponent<Slider>().value = this.gm.sm.getMusicVolume();
        GameObject.Find("EffectSlider").GetComponent<Slider>().value = this.gm.sm.getEffectVolume();
        GameObject.Find("MusicMuter").GetComponent<Toggle>().isOn = this.gm.sm.getMusicMute();
        GameObject.Find("EffectMuter").GetComponent<Toggle>().isOn = this.gm.sm.getEffectMute();
        GameObject.Find("HPBarToggle").GetComponent<Toggle>().isOn = SaveData.hpBar;
        GameObject.Find("DMGNumToggle").GetComponent<Toggle>().isOn = SaveData.dmgNum;
        GameObject.Find("VSyncToggle").GetComponent<Toggle>().isOn = SaveData.vSync;
        yield break;
    }

    private void disableMainMenuButtons()
    {
        GameObject.Find("NewGameButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ContinueButton").GetComponent<Button>().interactable = false;
        GameObject.Find("OptionsButton").GetComponent<Button>().interactable = false;
        GameObject.Find("QuitButton").GetComponent<Button>().interactable = false;

    }

    private void disableCombatButtons()
    {
        GameObject.Find("Skill1").GetComponent<Button>().interactable = false;
        GameObject.Find("Skill2").GetComponent<Button>().interactable = false;
        GameObject.Find("Skill3").GetComponent<Button>().interactable = false;
        GameObject.Find("Skill4").GetComponent<Button>().interactable = false;
    }

    private void enableCombatButtons()
    {
        GameObject.Find("Skill1").GetComponent<Button>().interactable = true;
        GameObject.Find("Skill2").GetComponent<Button>().interactable = true;
        GameObject.Find("Skill3").GetComponent<Button>().interactable = true;
        GameObject.Find("Skill4").GetComponent<Button>().interactable = true;
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
        StartCoroutine(OptionActive());
    }

    void OpenOptions()
    {
    	SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Single);
    }

    void ExitOptions()
    {
        if (this.gm.om.playerSpawned)
        {
            PlayerManager.Instance.optImg.color = Color.white;
            this.gm.pm.inOptions = false;
            ButtonOverlay.Instance.opt = false;
            SceneManager.UnloadSceneAsync("OptionsMenu");
            enableOWButtons();
            if (this.gm.pm.inCombat)
            {
                enableCombatButtons();
            }
        }
        else
        {
            this.gm.pm.inOptions = false;
            ReturnToMainMenu();
        }
    }

    private void enableOWButtons()
    {
        GameObject.Find("OptionsButtonOW").GetComponent<Button>().interactable = true;
        GameObject.Find("InventoryButtonOW").GetComponent<Button>().interactable = true;

    }

    private void loadGame()
    {
        GameObject overworldTilemap = GameObject.Find("OverworldGrid").transform.GetChild(0).gameObject;

        if (SceneManager.GetSceneByName("Battleworld").IsValid())
            SceneManager.UnloadSceneAsync("BattleWorld");

        if (SceneManager.GetSceneByName("LoseSplash").IsValid())
            SceneManager.UnloadSceneAsync("LoseSplash");

        overworldTilemap.SetActive(true);

        //this.gm.pm.pc.inventory.printList();

        if (this.gm.pm.pc.inventory.CheckItem(Item.ItemType.Resurrection) == null)
        {
            print("NO RES LEFT");
            GameObject.Find("LoadSaveButton").GetComponent<Button>().interactable = false;
        }

        this.gm.splashUp = false;
        this.gm.om.loadSave();
        this.gm.om.dm.setInteractable();
        this.gm.om.dm.setInitialSelection();
    }

    void ExitBattle()
    {
        GameObject overworldTilemap = GameObject.Find("OverworldGrid").transform.GetChild(0).gameObject;
    	SceneManager.UnloadSceneAsync("Battleworld");

        if (SceneManager.GetSceneByName("WinSplash").IsValid())
    	    SceneManager.UnloadSceneAsync("WinSplash");

        if (SceneManager.GetSceneByName("LoseSplash").IsValid())
            SceneManager.UnloadSceneAsync("LoseSplash");

        overworldTilemap.SetActive(true);
        this.gm.om.dm.setInteractable();
        this.gm.om.dm.setInitialSelection();
        //this.gm.om.rollParchment.SetActive(true);

    }

    void ReturnToMainMenu()
    {
        //DestroyImmediate(this.gm.gameObject);
    	SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }


    void ReturnToMainMenuFromGame()
    {
        SaveData.updateSettings(this.gm.sm.musicVolume, this.gm.sm.effectsVolume, this.gm.sm.musicMute, this.gm.sm.effectsMute);
        DestroyImmediate(this.gm.gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void QuitGame()
    {
    	Application.Quit();
    }
}
