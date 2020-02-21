using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int whatsMyId()
    {
        return this.myId;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Can't create this from the start because it relies on objects in the Arena Scene
        // this.bm = this.gameObject.AddComponent<BattleManager>();
        this.bm = null;
        this.panic = false;
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

        if (SceneManager.GetActiveScene().name == "Battleworld")
        {
            if (this.bm == null)
            {
                this.bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            }

            if (this.bm != null)
            {
                if (this.bm.isBattleResolved())
                {
                    string splash;
                    if (this.bm.didWeWinTheBattle())
                        splash = "WinSplash";
                    else
                        splash = "LoseSplash";

                    this.bm = null;

                    if (!this.panic)
                    {
                        // Debug.Log("Ahhh! We're currently in scene "+ SceneManager.GetActiveScene().name);
                        if (splash == "WinSplash")
                        {
                            this.sm.playWinJingle();
                        }
                        else
                        {
                            this.sm.playLoseJingle();
                        }

                        SceneManager.LoadScene(splash, LoadSceneMode.Additive);
                        this.bm = null;
                        this.panic = true;
                    }
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Overworld_emptynodes")
        {
            if (!this.panic)
            {
                this.panic = true;

                float unitsPerPixel = 16f / Screen.width;
                float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

                // GameObject.Find("DialogueManager").SetActive(true);
                GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize = desiredHalfHeight;
                // SceneManager.LoadScene("Dialogue", LoadSceneMode.Additive);
            }
        }
    }
}
