using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

    public int coinCount;
    public int arrowCount;
    public int level;

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

    public void UpdateCoinUI()
    {
        this.transform.Find("CoinCount")
                .GetComponent<TextMeshProUGUI>().text = Player.instance.coinCount.ToString();
    }
}
