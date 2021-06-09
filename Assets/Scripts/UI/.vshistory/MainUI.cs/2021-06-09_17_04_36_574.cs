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
        this.transform.Find("CoinCount")
                .GetComponent<TextMeshProUGUI>().text = coinCount.ToString();
    }

    public void UpdateArrowCountUI()
    {
        this.transform.Find("ArrowCount")
                      .GetComponent<TextMeshProUGUI>().text = arrowCount.ToString(); 
    }

    public void UpdateGaugeBarUI()
    {

        gaugeBar.fillAmount = _power / arrowMaxPower;

    }

    public void InitializeArrowCount(int arrowCount)
    {
        this.arrowCount = arrowCount;
    }
}
