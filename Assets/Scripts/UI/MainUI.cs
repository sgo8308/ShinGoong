    using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

    public PlayerInfo playerInfo;

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
        _levelUI = transform.Find("LevelCount")
                                .GetComponent<TextMeshProUGUI>();

        _coinCountUI = transform.Find("CoinCount")
                                .GetComponent<TextMeshProUGUI>();

        _arrowCountUI = transform.Find("ArrowCount")
                                 .GetComponent<TextMeshProUGUI>();

        _gaugeBarUI = transform.Find("GaugeBar")
                                 .GetComponent<Image>();

        UpdateCoinUI();
        UpdateArrowCountUI();
        UpdateLevelUI();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += UpdateLevelUI;
    }

    public void UpdateCoinUI()
    {
        _coinCountUI.text = Inventory.instance.GetCoinCount().ToString();
    }

    public void UpdateLevelUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
        {
            _levelUI.text = playerInfo.level.ToString();
        }
    }

    public void UpdateLevelUI()
    {
        _levelUI.text = playerInfo.level.ToString();
    }

    public void UpdateArrowCountUI()
    {
        _arrowCountUI.text = Inventory.instance.GetArrowCount().ToString(); 
    }

    public void UpdateGaugeBarUI(float arrowPower, float arrowMaxPower)
    {
        _gaugeBarUI.fillAmount = arrowPower / arrowMaxPower;
    }
}
