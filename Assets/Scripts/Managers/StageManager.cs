
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
    public PlayerSkill playerSkill;

    public GameObject nextStageTeleport;

    public delegate void OnStageClear();
    public OnStageClear onStageClear;

    private string player_uuid = "TentuPlayer"; // player_uuid can be anything that uniquely identifies each of your game user.
    private string character_uuid = TentuPlayKeyword._DUMMY_CHARACTER_ID_;
    private string[] character_uuids = new string[] { TentuPlayKeyword._DUMMY_CHARACTER_ID_ };

    TPStashEvent myStashEvent;

    

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
        myStashEvent = new TPStashEvent();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ResetStopWatch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartStopWatch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeStage;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SetNextStageTeleport;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartStage;

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
                totalNumOfMosters = 1;
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

            switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            {
                case "Stage1Scene":
                    Debug.Log("클리어 스테이지 스테이지 원 씬");
                    myStashEvent.PlayStage(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuids: character_uuids,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_1", // unique identifier of played stage
                        stage_category_slug: "general", // category slug of stage (optional)
                        stage_level: "1", // level of stage (optional)
                        stage_score: null, // score is null when stage starts (optional)
                        stage_status: stageStatus.Win, // "Start"
                        stage_playtime: null // playtime is null when stage starts (optional)
                        );
                    break;

                case "BossScene":
                    myStashEvent.PlayStage(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuids: character_uuids,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_2", // unique identifier of played stage
                        stage_category_slug: "boss", // category slug of stage (optional)
                        stage_level: "1", // level of stage (optional)
                        stage_score: null, // score is null when stage starts (optional)
                        stage_status: stageStatus.Win, // "Start"
                        stage_playtime: null // playtime is null when stage starts (optional)
                        );
                    break;

                default:
                    break;
            }
        }
    }

    public string GetStageName()
    {
        string nowScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        switch (nowScene)
        {
            case "ShelterScene":
                return "shelter";
            case "Stage1Scene":
                return "stage1_1";
            case "BossScene":
                return "stage1_2";
            default:
                break;
        }

        return "NoStage";
    }

    public string GetStageCategory()
    {
        string nowScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        switch (nowScene)
        {
            case "ShelterScene":
                return "shelter";
            case "Stage1Scene":
                return "general";
            case "BossScene":
                return "boss";
            default:
                break;
        }

        return "NoStageCategory";
    }
    

    #region TentuPlayMethod
    void StartStage(Scene scene, LoadSceneMode mode)
    {
        TPStashEvent myStashEvent = new TPStashEvent();

        switch (scene.name) 
        {
            case "Stage1Scene":

                Debug.Log("스테이지1 스타트");
                new TPStashEvent().PlayStage(
                    player_uuid: player_uuid, // unique identifier of player
                    character_uuids: character_uuids,
                    stage_type: stageType.PvE,
                    stage_slug: "stage1_1", // unique identifier of played stage
                    stage_category_slug: "general", // category slug of stage (optional)
                    stage_level: "1", // level of stage (optional)
                    stage_score: null, // score is null when stage starts (optional)
                    stage_status: stageStatus.Start, // "Start"
                    stage_playtime: null // playtime is null when stage starts (optional)
                    );

                if (playerSkill.hasSkill)
                {
                    new TPStashEvent().PlayStageWithSkill(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuid: character_uuid,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_1", // unique identifier of played stage
                        stage_category_slug: "general", // category slug of stage (optional)
                        skill_slug: playerSkill.GetSkillName(),
                        skill_category_slug: null
                        ) ;
                }

                if (Inventory.instance.isItemEquipped())
                {
                    new TPStashEvent().PlayStageWithEquipment(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuid: character_uuid,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_1", // unique identifier of played stage
                        stage_category_slug: "general", // category slug of stage (optional)
                        item_slug: Inventory.instance.GetEquippedItem().itemName
                        );
                }

                break;

            case "BossScene":
                myStashEvent.PlayStage(
                    player_uuid: player_uuid, // unique identifier of player
                    character_uuids: character_uuids,
                    stage_type: stageType.PvE,
                    stage_slug: "stage1_2", // unique identifier of played stage
                    stage_category_slug: "boss", // category slug of stage (optional)
                    stage_level: "1", // level of stage (optional)
                    stage_score: null, // score is null when stage starts (optional)
                    stage_status: stageStatus.Start, // "Start"
                    stage_playtime: null // playtime is null when stage starts (optional)
                    );

                if (playerSkill.hasSkill)
                {
                    new TPStashEvent().PlayStageWithSkill(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuid: character_uuid,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_2", // unique identifier of played stage
                        stage_category_slug: "boss", // category slug of stage (optional)
                        skill_slug: playerSkill.GetSkillName(),
                        skill_category_slug: null
                        );
                }

                if (Inventory.instance.isItemEquipped())
                {
                    new TPStashEvent().PlayStageWithEquipment(
                        player_uuid: player_uuid, // unique identifier of player
                        character_uuid: character_uuid,
                        stage_type: stageType.PvE,
                        stage_slug: "stage1_2", // unique identifier of played stage
                        stage_category_slug: "boss", // category slug of stage (optional)
                        item_slug: Inventory.instance.GetEquippedItem().itemName
                        );
                }
                break;

            default:
                break;
        }
    }

    public void LoseStage()
    {
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "Stage1Scene":
                myStashEvent.PlayStage(
                    player_uuid: player_uuid, // unique identifier of player
                    character_uuids: character_uuids,
                    stage_type: stageType.PvE,
                    stage_slug: "stage1_1", // unique identifier of played stage
                    stage_category_slug: "general", // category slug of stage (optional)
                    stage_level: "1", // level of stage (optional)
                    stage_score: null, // score is null when stage starts (optional)
                    stage_status: stageStatus.Lose, // "Start"
                    stage_playtime: null // playtime is null when stage starts (optional)
                    );
                break;

            case "BossScene":
                myStashEvent.PlayStage(
                    player_uuid: player_uuid, // unique identifier of player
                    character_uuids: character_uuids,
                    stage_type: stageType.PvE,
                    stage_slug: "stage1_2", // unique identifier of played stage
                    stage_category_slug: "boss", // category slug of stage (optional)
                    stage_level: "1", // level of stage (optional)
                    stage_score: null, // score is null when stage starts (optional)
                    stage_status: stageStatus.Lose, // "Start"
                    stage_playtime: null // playtime is null when stage starts (optional)
                    );
                break;

            default:
                break;
        }
    }
    #endregion
}