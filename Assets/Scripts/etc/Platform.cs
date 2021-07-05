using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 보스 씬에서 보스가 플레이어의 위치 또는 자신이 갈 플랫폿을 정하기 위해 쓰이는 스크립트. 
/// 보스 씬 각각의 플랫폼마다 붙어있다.
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

    //보스가 날아갈 위치에 거품을 표시해준다.
    public void ShowBubble()
    {
        if (platformNowBubbleOn != null)
            platformNowBubbleOn.GetComponent<Platform>().bubble.SetActive(false);
        
        bubble.SetActive(true);
        platformNowBubbleOn = this;
    }
}
