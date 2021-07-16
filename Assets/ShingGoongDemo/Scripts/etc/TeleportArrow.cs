using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 화살이 닿은 곳으로 이동할 수 있게 해주는 텔레포트 화살
/// </summary>
public class TeleportArrow : MonoBehaviour
{
    private int arrowColMaxCount = 4;

    private const int LAYER_NUM_ARROW_ON_PLATFORM = 14;
    private const int LAYER_NUM_ELECTRICITY_FOR_ARROW = 25;

    private bool arrowState = true;
    private bool isUsed = false;
    public bool isStop = false;

    private Vector2 zeroVelocity;
    private List<Vector2> arrowColList = new List<Vector2>();
    protected GameObject bombShotEffect;

    private GameObject teleportPosition;

    private float power;
    void Start()
    {
        var telportArrows = FindObjectsOfType<TeleportArrow>();

        if (telportArrows.Length > 1)
            Destroy(telportArrows[1].gameObject);

        power = PlayerAttack.nowPowerOfArrow;

        zeroVelocity = new Vector2(0, 0);

        teleportPosition = transform.Find("TeleportPosition").gameObject;

        Invoke("DestroyThis", 20);
    }

    void Update()
    {
        if (arrowState)
        {
            transform.right = GetComponent<Rigidbody2D>().velocity;  
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsed)
            return;

        if (collision.gameObject.layer == LAYER_NUM_ELECTRICITY_FOR_ARROW)
        {
            SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ARROW_BURN);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag != "Platform")
            return;

        if (!isStraightGravityArrow()) 
        {
            Stop();

            gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
        }

        if (isStraightGravityArrow())  
        {
            Reflect(collision);

            arrowColList.Add(collision.contacts[0].point);   

            if (arrowColList.Count == arrowColMaxCount)
            {
                Stop();

                gameObject.layer = LAYER_NUM_ARROW_ON_PLATFORM;
            }
        }
    }

    private void Stop()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; 
        GetComponent<Rigidbody2D>().velocity = zeroVelocity;
        arrowState = false; 

        isUsed = true;
        isStop = true;

        teleportPosition.transform.parent = null;
        Destroy(this.gameObject);
    }

    private void Reflect(Collision2D collision)
    {
        Vector2 inNormal = collision.contacts[0].normal;              
        Vector2 newVelocity = Vector2.Reflect(transform.right, inNormal);  
        GetComponent<Rigidbody2D>().velocity = newVelocity * power;   
    }

    private bool isStraightGravityArrow()
    {
        if (GetComponent<Rigidbody2D>().gravityScale == 0)
            return true;

        return false;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
