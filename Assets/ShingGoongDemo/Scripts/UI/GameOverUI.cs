using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
/// <summary>
/// 스테이지가 종료되었을 때 나타나는 UI에 관한 스크립트
/// </summary>
public class GameOverUI : UIOpener
{
    public Button exitStageButtonInMainMenu;

    TextMeshProUGUI Title;
    GameObject gameOverPanel;

    TextMeshProUGUI playTime;
    TextMeshProUGUI level;
    TextMeshProUGUI coin;
    Image expBar;
    Button continueButton;
    Button exitButton;
    Player player;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameOverUI>();

        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();

        player = GameObject.Find("Player").GetComponent<Player>();

        OnPlayerDead();
        OnStageClear();
        OnClickExitStage();

        playTime = transform.Find("GameOverPanel")
                             .Find("PlayTime")
                             .Find("PlayTimeText")
                             .GetComponent<TextMeshProUGUI>();

        level = transform.Find("GameOverPanel")
                          .Find("Level")
                          .Find("LevelText")
                          .GetComponent<TextMeshProUGUI>();

        coin = transform.Find("GameOverPanel")
                          .Find("Coin")
                          .Find("CoinText")
                          .GetComponent<TextMeshProUGUI>();

        expBar = transform.Find("GameOverPanel")
                          .Find("ExpBarFrame")
                          .Find("ExpBar")
                          .GetComponent<Image>();

        Title = transform.Find("GameOverPanel")
                             .Find("GameOver")
                             .GetComponent<TextMeshProUGUI>();

        continueButton = transform.Find("GameOverPanel")
                          .Find("ContinueOrExit")
                          .Find("ContinueButton").GetComponent<Button>();


        exitButton = transform.Find("GameOverPanel")
                          .Find("ContinueOrExit")
                          .Find("ExitButton").GetComponent<Button>();

        continueButton.onClick.AddListener(Close);

        exitButton.onClick.AddListener(Close);
        exitButton.onClick.AddListener(GoToShelterScene);

        gameOverPanel = transform.Find("GameOverPanel").gameObject;

        level.text = PlayerInfo.instance.level.ToString();

        exitButton.onClick.AddListener(GoToShelterScene);
    }

    public int timesExpBarToBeFilled;
    public int timesExpBarIsFilled;
    public float expBarPercentToBeSet;
    public bool canFillExpBar = false;

    private void OnPlayerDead()
    {
        player.onPlayerDead += SetPlayTime;
        player.onPlayerDead += SetExpFillInfo;
        player.onPlayerDead += DelayedOpen;
        player.onPlayerDead += InvokeFillExpBar;
        player.onPlayerDead += SetCoinUI;
    }

    private void OnStageClear()
    {
        StageManager.instance.onStageClear += SetPlayTime;
        StageManager.instance.onStageClear += SetExpFillInfo;
        StageManager.instance.onStageClear += DelayedOpen;
        StageManager.instance.onStageClear += InvokeFillExpBar;
        StageManager.instance.onStageClear += SetCoinUI;
    }

    private void OnClickExitStage()
    {
        exitStageButtonInMainMenu.onClick.AddListener(SetPlayTime);
        exitStageButtonInMainMenu.onClick.AddListener(SetExpFillInfo);
        exitStageButtonInMainMenu.onClick.AddListener(Open);
        exitStageButtonInMainMenu.onClick.AddListener(InvokeFillExpBar);
        exitStageButtonInMainMenu.onClick.AddListener(SetCoinUI);
    }

    private void InvokeFillExpBar() 
    {
        Invoke("FillExpBar", 0.5f);
    }

    /// <summary>
    /// 스테이지 종료 후 얻은 경험치가 오르는 것을 담당하는 메소드
    /// </summary>
    private void FillExpBar()
    {
        if (!canFillExpBar)
            return;

        if (expBar.fillAmount >= 1)
        {
            expBar.fillAmount = 0;
            UpdateLevelUI();
            timesExpBarIsFilled++;

            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.LEVEL_UP);
        }

        if (timesExpBarIsFilled == timesExpBarToBeFilled)
        {
            if (expBar.fillAmount >= expBarPercentToBeSet)
            {
                canFillExpBar = false;
                timesExpBarIsFilled = 0;
                return;
            }
        }

        expBar.fillAmount += 0.03f;

        Invoke("FillExpBar", 0.04f);
    }

    private void SetExpFillInfo()
    {
        timesExpBarToBeFilled = PlayerInfo.instance.GetLevelUpCount();
        expBarPercentToBeSet = PlayerInfo.instance.CalculateNowExpPercent();
    }

    private void SetPlayTime()
    {
        playTime.text = StageManager.instance.GetPlayTime();
    }

    private void UpdateLevelUI()
    {
        int lev = Convert.ToInt32(level.text);

        lev++;

        this.level.text = lev.ToString();
    }

    private void SetCoinUI()
    {
        coin.text = Inventory.instance.GetCoinCountPerStage().ToString();
    }

    private void DelayedOpen()
    {
        Invoke("Open", 1f);
    }

    protected override void Open()
    {
        Time.timeScale = 1;

        isOpened = true;
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);
        SoundManager.instance.MutePlayerRunningSound();
        SoundManager.instance.MutePlayerSound();

        if (playerMove.animator.GetBool("isJumpingDown"))
            playerMove.animator.SetBool("isJumpingFinal", true);

        if (!Player.isDead && StageManager.instance.stageState == StageState.CLEAR)
        {
            Title.text = "STAGE CLEAR";
            playerMove.animator.SetBool("isRunning", false);
            continueButton.gameObject.SetActive(true);
            gameOverPanel.SetActive(true);
            canFillExpBar = true;
        }
        else
        {
            Title.text = "GAME OVER";
            playerMove.animator.SetBool("isRunning", false);
            continueButton.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            canFillExpBar = true;

            StageManager.instance.LoseStage();
        }
    }

    protected override void Close()
    {
        base.Close();

        gameOverPanel.SetActive(false);
        canFillExpBar = true;
        playerMove.SetCanMove(true);
        playerAttack.SetCanShoot(true);
        SoundManager.instance.UnMutePlayerSound();
        SoundManager.instance.UnMutePlayerRunningSound();
        playerMove.InitJumpValues();
    }

    private void GoToShelterScene()
    {
        SceneManager.instance.GoTo("ShelterScene");
    }
}
