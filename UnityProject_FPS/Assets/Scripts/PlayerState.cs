using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private int hp=50;

    public int hpManager { get { return hp; } set { hp = value; } }

    void Update()
    {
        Die();
    }

    private void Die()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
