using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Platform in BossScene
/// </summary>
public class Platform : MonoBehaviour
{
    public GameObject bubble;
    static Platform platformNowBubbleOn;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BossScene")
            return;

        if (collision.gameObject.tag == "Player")
            StageManager.instance.SetPlatformPlayerSteppingOn(this.transform);

        if (collision.gameObject.tag == "Monster")
        {
            collision.gameObject.GetComponent<MonsterBoss>().nowPlatform = this.transform;
            bubble.SetActive(false);
        }
    }

    public void ShowBubble()
    {
        if (platformNowBubbleOn != null)
            platformNowBubbleOn.GetComponent<Platform>().bubble.SetActive(false);
        
        bubble.SetActive(true);
        platformNowBubbleOn = this;
    }
}
