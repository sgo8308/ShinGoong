
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using UnityEngine.SceneManagement;
using TentuPlay.Api;

public enum StageState
{
    CLEAR,
    UNCLEAR
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public StageState stageState;

    public int totalNumOfMosters;

    public int numOfMonstersKilled;

    Stopwatch stopWatch;

    public GameObject player;

    public GameObject nextStageTeleport;

    public delegate void OnStageClear();
    public OnStageClear onStageClear;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TPStashEvent myStashEvent = new TPStashEvent();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ResetStopWatch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartStopWatch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeStage;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SetNextStageTeleport;

        stopWatch = new Stopwatch();

        player = GameObject.Find("Player");

        player.GetComponent<Player>().onPlayerDead += StopStopWatch;
    }

    public bool CheckStageClear()
    {
        return numOfMonstersKilled >= totalNumOfMosters;
    }

    public void InitializeStage(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "ShelterScene":
                stageState = StageState.CLEAR;
                InitializePlayerPosition();
                break;
            case "Stage1Scene":
                stageState = StageState.UNCLEAR;
                totalNumOfMosters = 11;
                break;
            case "BossScene":
                stageState = StageState.UNCLEAR;
                totalNumOfMosters = 1;
                break;
            default:
                break;
        }
    }

    private void InitializePlayerPosition()
    {
        Transform playerStartPosition = GameObject.Find("PlayerStartPosition").transform;
        player.transform.position = playerStartPosition.position;
    }

    void NextStageTeleportOn()
    {
        nextStageTeleport.SetActive(true);
    }
    void SetNextStageTeleport(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            return;

        nextStageTeleport = GameObject.Find("Teleport");
        nextStageTeleport.SetActive(false);
    }

    #region StopWatch
    void StartStopWatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            return;

        stopWatch.Start();
    }

    void StopStopWatch()
    {
        stopWatch.Stop();
    }

    void ResetStopWatch(Scene scene, LoadSceneMode mode)
    {
        stopWatch.Reset();
    }

    public string GetPlayTime()
    {
        TimeSpan ts = stopWatch.Elapsed;
        string playTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);

        return playTime;
    }
    #endregion

    #region Shelter

    public GameObject storePanel;
    public GameObject mainMenuPanel;
    public void InitializeStore()
    {
        GameObject.Find("StoreNpc").GetComponent<StoreOpener>().storePanel = this.storePanel;
        GameObject.Find("StoreNpc").GetComponent<StoreOpener>().mainMenuPanel = this.mainMenuPanel;
    }
    #endregion

    #region Stage1Boss
    public Transform platformPlayerSteppingOn { get; private set; }
    public void SetPlatformPlayerSteppingOn(Transform platform)
    {
        platformPlayerSteppingOn = platform;
    }
    #endregion

    public void AddNumOfMonsterKilled()
    {
        numOfMonstersKilled++;

        ClearStage();
    }

    public void ClearStage()
    {
        if (CheckStageClear())
        {
            numOfMonstersKilled = 0;
            stageState = StageState.CLEAR;
            NextStageTeleportOn();
            onStageClear.Invoke();
        }
    }

    #region TentuPlayMethod
    void PlayStage()
    {
    }


    #endregion
}