using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]

public class MonsterBody : MonoBehaviour
{
    MonsterMite _monsterMite;
    public UnityEvent<Collision2D> OnMonsterHit;

    private void Start()
    {
        _monsterMite = this.GetComponentInParent<MonsterMite>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnMonsterHit.Invoke(collision);
    }


}
