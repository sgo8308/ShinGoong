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

    }

    void Update()
    {
        if (!_isDirectionChecked)
        {
            this.transform.root.transform.root
            _isDirectionChecked = true;
        }
        transform.Translate(transform.right * speed * Time.deltaTime);
    }
}
