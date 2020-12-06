using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //수류탄의 역할
    /*
     * 슈팅게임의 총알은 생성하면 지 스스로 날아가다 충돌하면 터졌다
     * 하지만 수류탄은 생성되자마자 스스로 이동하면 당연히 안된다. 
     * 수류탄은 플레이어가 직접 던져야한다.
     * 수류탄이 다른 오브젝트들과 충돌하면 터지고 자신도 사라져야한다.
     */

    public GameObject fxFactory; // 이펙트 프리팹도 필요하다.
    [SerializeField] int bombDam = 20;
    float timer = 0f;
    private GunFire gun;
    private void Start()
    {
         gun = GameObject.Find("Player").GetComponent<GunFire>();
    }
    //충돌처리 
    private void Update()
    {

        lateDestory();
    }

    private void lateDestory()
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            GameObject fx = Instantiate(fxFactory);
            fx.transform.position = transform.position;
            gun.bombCount--;
            
            //다른오브젝트 삭제
            //자기자신도 삭제하기
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //폭발이펙트 보여주기
        GameObject fx = Instantiate(fxFactory);
        fx.transform.position = transform.position;
        gun.bombCount--;
        if(collision.transform.tag=="Enemy")
        {
            EnemyFSM enemy = collision.transform.GetComponent<EnemyFSM>();
            enemy.Damaged(bombDam);
        }
        //다른오브젝트 삭제
        //자기자신도 삭제하기
        Destroy(gameObject);
    }


}
