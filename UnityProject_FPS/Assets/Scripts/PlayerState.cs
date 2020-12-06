using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private int hp = 50;

    public int hpManager { get { return hp; } set { hp = value; } }
    bool isDie = false;
    public Animator animator;

    private void Awake()
    {
        animator = transform.Find("PlayerModelNew").GetComponent<Animator>();
    }
    void Update()
    {
        Die();
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    if (0f != animator.transform.localRotation.y)
        //    {
        //        animator.transform.localRotation = Quaternion.Euler(0, 3f, 0);
        //    }

        //}
    }

    private void Die()
    {
        if (hp <= 0 &&!isDie)
        {
            animator.SetTrigger("Die");
            StartCoroutine(coDie(3.5f));
            isDie = true;
            PlayerMove playerMove = GetComponent<PlayerMove>();
            playerMove.enabled = false;
        }
    }
    IEnumerator coDie(float num)
    {
        yield return new WaitForSeconds(num);
        Destroy(gameObject);
    }
        

}
