using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMite : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rigid;
    SpriteRenderer _spriteRenderer;
    GameObject _radar;
    RectTransform _radarRectTranform;
    float _speed;
    int _nextMove; // -1 , 0 , 1 -> left, stop, right


    #region Move
    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _radar = this.transform.Find("MonsterCanvas").transform
                                .Find("RadarImage").gameObject;
        _radarRectTranform = _radar.GetComponent<RectTransform>();
        _speed = 1;

        Invoke("Think", 5); // think per 5 seconds
    }

    void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(_nextMove * _speed, _rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(_rigid.position.x + _nextMove, _rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0), 10.0f, false);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Think()
    {
        _nextMove = Random.Range(-1, 2);

        _anim.SetInteger("WalkSpeed", _nextMove);

        FlipMoster();

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        _nextMove = _nextMove * -1;
        FlipMoster();

        CancelInvoke();
        Invoke("Think", 3);
    }

    void FlipMoster()
    {
        // Flip Monster Sprite and Radar image 
        if (_nextMove != 0)
        {
            if (_nextMove == 1)
            {
                _spriteRenderer.flipX = true;
                _radarRectTranform.rotation =
                    Quaternion.Euler(_radarRectTranform.rotation.x,
                                     0, _radarRectTranform.rotation.z);
            }
            else
            {
                _spriteRenderer.flipX = false;
                _radarRectTranform.rotation =
                    Quaternion.Euler(_radarRectTranform.rotation.x,
                                     180, _radarRectTranform.rotation.z);
            }
        }
    }
    #endregion

    public GameObject bullet;
    public Transform bulletPocket;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Attack();
            _radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
            _speed = 2;
            Invoke("RestoreRadarColor", 5);
            Invoke("RestoreSpeed", 5);
        }
    }

    //void Attack()
    //{
    //    GameObject child = Instantiate(bullet, bulletPocket.position,
    //                                    transform.rotation, bulletPocket);
    //}

    void RestoreRadarColor()
    {
        _radar.GetComponent<Image>().color = new Color(1, 1, 0, 0.5f);
    }

    void RestoreSpeed()
    {
        _speed = 1;
    }

    void GetAngry()
    {
        _radar.GetComponent<Image>().color = new Color(1, 0, 0, 0.75f);
        _speed = 2;
        if (_nextMove == 0 && _spriteRenderer.flipX)
        {
            _nextMove = -1;
        }else if (_nextMove == 0 && !_spriteRenderer.flipX)
        {
            _nextMove = 1;
        }
    }
}
