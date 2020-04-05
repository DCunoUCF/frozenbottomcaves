using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public BattleManager bm;
    public OverworldManager om;
    public SoundManager sm;
    public PlayerManager pm;

    private AudioSource gameMusicChannel;
    private AudioSource gameEffectChannel;
    private int myId;

    private string currentScene;
    private bool panic;
    private bool battleResolvedCheck;
    private bool battleLogicComplete;
    private bool splashUp;

    public int whatsMyId()
    {
        return this.myId;
    }

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        // Can't create this from the start because it relies on objects in the Arena Scene
        // this.bm = this.gameObject.AddComponent<BattleManager>();
        this.bm = null;
        this.panic = false;
        this.battleResolvedCheck = false;
        this.battleLogicComplete = false;
        this.currentScene = SceneManager.GetActiveScene().name;

        this.sm = this.gameObject.AddComponent<SoundManager>();
        this.om = this.gameObject.AddComponent<OverworldManager>();
        this.pm = this.gameObject.AddComponent<PlayerManager>();

        // this.sm.setAudioChannels(GameObject.Find("MusicChannel").GetComponent<AudioSource>(),
                                 // GameObject.Find("EffectChannel").GetComponent<AudioSource>());

        this.gameMusicChannel = this.gameObject.AddComponent<AudioSource>();
        this.gameEffectChannel = this.gameObject.AddComponent<AudioSource>();

        this.sm.setAudioChannels(this.gameMusicChannel, this.gameEffectChannel);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("GM"))
        {
            if (this.myId != g.GetComponent<GameManager>().whatsMyId())
                Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        this.myId = (int)(Random.value * 999999);

        Debug.Log("This is my GameManager id!\n" + this.myId);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameMusicChannel == null)
        {
            this.gameMusicChannel = this.gameObject.AddComponent<AudioSource>();
            this.sm.setMusicChannel(this.gameMusicChannel);
        }
        
        if (this.gameEffectChannel == null)
        {
            this.gameEffectChannel = this.gameObject.AddComponent<AudioSource>();
            this.sm.setEffectChannel(this.gameEffectChannel);
        }

        if (SceneManager.GetActiveScene().name != this.currentScene)
        {
            // this.sm.updateMusicList();
            this.currentScene = SceneManager.GetActiveScene().name;
            this.panic = false;
        }

        if (SceneManager.GetSceneByName("Battleworld").IsValid())
        {
            if (this.bm != null)
                battleResolvedCheck = this.bm.isBattleResolved();

            if (this.bm == null && !this.battleLogicComplete && GameObject.Find("BattleManager") != null)
            {
                print(GameObject.Find("BattleManager"));
                this.bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            }

            if (this.bm != null)
            {
                if (this.battleResolvedCheck)
                {
                    string splash;
                    bool won = false;
                    if (this.bm.didWeWinTheBattle())
                    {
                        splash = "WinSplash";
                        won = true;
                    }
                    else
                    { 
                        splash = "LoseSplash";
                    }

                    if (splash == "WinSplash")
                    {
                        this.sm.playWinJingle();
                    }
                    else
                    {
                        this.sm.playLoseJingle();
                    }

                    if (!SceneManager.GetSceneByName(splash).IsValid())
                    {
                        this.om.dm.setUninteractable();
                        SceneManager.LoadScene(splash, LoadSceneMode.Additive);
                        StartCoroutine(setReturnRestartActive(splash, won));
                    }
                    this.panic = true;
                    this.battleResolvedCheck = false;
                    this.bm = null;
                    this.battleLogicComplete = true;
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            if (!this.panic)
            {
                this.panic = true;

                //float unitsPerPixel = 16f / Screen.width;
                //float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
                //GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize = desiredHalfHeight;

                // GameObject.Find("DialogueManager").SetActive(true);
                // SceneManager.LoadScene("Dialogue", LoadSceneMode.Additive);
            }
        }

        if (this.pm.PLAYERDEAD && !splashUp)
        {
            this.om.dm.setUninteractable();
            splashUp = true;
            SceneManager.LoadScene("LoseSplash", LoadSceneMode.Additive);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            // SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            Application.Quit();
        }
    }

    public void setBM(BattleManager bm)
    {
        this.bm = bm;
    }


    IEnumerator setReturnRestartActive(string path, bool won)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(path));

        if (won)
            GameObject.Find("ReturnButton").GetComponent<Button>().Select();
        else
            GameObject.Find("RestartButton").GetComponent<Button>().Select();

        yield break;
    }
}
