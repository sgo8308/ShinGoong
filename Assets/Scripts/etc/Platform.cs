using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BossScene")
            return;

        if (collision.gameObject.tag == "Player")
            StageManager.instance.SetPlatformPlayerSteppingOn(this.transform);

        if (collision.gameObject.tag == "Monster")
            collision.gameObject.GetComponent<MonsterBoss>().nowPlatform = this.transform;
    }
}
