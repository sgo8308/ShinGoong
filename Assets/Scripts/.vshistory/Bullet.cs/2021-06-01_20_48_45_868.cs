using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 _direction;
    bool _isDirectionChecked;
    GameObject _mite;
    SpriteRenderer _miteSpriteRenderer;
    public float speed;

    void Start()
    {
        _mite = this.transform.root.transform.root.gameObject;
        _miteSpriteRenderer = _mite.GetComponent<SpriteRenderer>();
        if (_miteSpriteRenderer.flipX) 
        {
            _direction = transform.right;
        }
        else
        {
            _direction = transform.right * -1;
        }

        Invoke("Destroy", 5);
    }

    void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Wall")
        {
            Destroy(this);
        }
    }
}
