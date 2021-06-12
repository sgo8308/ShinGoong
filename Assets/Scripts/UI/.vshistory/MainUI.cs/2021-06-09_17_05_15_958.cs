﻿using System.Collections;
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
        _coinCountUI = transform.Find("CoinCount")
                                .GetComponent<TextMeshProUGUI>();

        _arrowCountUI = transform.Find("ArrowCount")
                                 .GetComponent<TextMeshProUGUI>();

        _gaugeBarUI = transform.Find("GuageBar")
                                 .GetComponent<Image>();
    }

    public void UpdateCoinUI()
    {
        _coinCountUI.text = coinCount.ToString();
    }

    public void UpdateArrowCountUI()
    {
        _arrowCountUI.text = arrowCount.ToString(); 
    }

    public void UpdateGaugeBarUI(float power, float maxPower)
    {

        _gaugeBarUI.fillAmount = power / power;

    }

    public void InitializeArrowCount(int arrowCount)
    {
        this.arrowCount = arrowCount;
    }
}