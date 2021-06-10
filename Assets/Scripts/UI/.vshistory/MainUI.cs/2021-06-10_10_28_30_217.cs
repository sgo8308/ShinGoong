using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

    public int coinCount;
    public int arrowCount;
    public int level;

    TextMeshProUGUI _coinCountUI;
    TextMeshProUGUI _arrowCountUI;
    TextMeshProUGUI _levelUI;
    Image _gaugeBarUI;

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
        InitializeArrowCount(45);

        _levelUI = transform.Find("LevelCount")
                                .GetComponent<TextMeshProUGUI>();

        _coinCountUI = transform.Find("CoinCount")
                                .GetComponent<TextMeshProUGUI>();

        _arrowCountUI = transform.Find("ArrowCount")
                                 .GetComponent<TextMeshProUGUI>();

        _gaugeBarUI = transform.Find("GaugeBar")
                                 .GetComponent<Image>();

        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        _coinCountUI.text = coinCount.ToString();
    }

    public void UpdateLevelUI()
    {
        _levelUI.text = level.ToString();
    }

    public void UpdateArrowCountUI()
    {
        _arrowCountUI.text = arrowCount.ToString(); 
    }

    public void UpdateGaugeBarUI(float arrowPower, float arrowMaxPower)
    {
        _gaugeBarUI.fillAmount = arrowPower / arrowMaxPower;
    }

    public void InitializeArrowCount(int arrowCount)
    {
        this.arrowCount = arrowCount;
    }
}
