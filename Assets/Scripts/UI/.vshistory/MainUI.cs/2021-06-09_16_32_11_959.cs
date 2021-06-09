using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

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
