//using System;
using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private float distanceMax = 205f;
    public GameObject bulletFactory;
    //public ParticleSystem bulletFx;
    float lastAtkTime = 0f;
    public float atkTime = 1f;
    public GameObject bombFactory;
    public GameObject firePoint;
    float bombPower = 20f;
    public int bombCountMax = 1;
    public int bombCount = 0;
    [SerializeField] int normalDam = 5;
    
    [SerializeField] private GameObject[] test;
    PlayerState anim;

    public Camera cam;
    void Start()
    {
        anim = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
      
        //Shoot();
       
        Fire();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;

            //레이랑 충돌했냐?
            // int layer = (-1)-(1 << LayerMask.NameToLayer("Player"));
            //int layer = 1 << LayerMask.NameToLayer("Player");
            //layer = ~layer;
            int layer = 1 << 8;
            if(Physics.Raycast(ray,out hitInfo, distanceMax,~layer))
            {
                //Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.red);


                anim.animator.SetTrigger("Fire");

                StartCoroutine(coFire1(0.2f, hitInfo));
               

            }
        }
        IEnumerator coFire1(float num, RaycastHit hitInfo)
        {
            yield return new WaitForSeconds(num);
            //충돌지점에 총알 이펙트만 생성하면 된다.
            //총알이펙트 생성
            GameObject bulletFx = Instantiate(bulletFactory);
            bulletFx.transform.position = hitInfo.point;

            //normal을 사용하면 충돌체로부터의 수직을 구해준다.
            bulletFx.transform.forward = hitInfo.normal;

            //유니티 최적화 
            /* 
             * 1. 오브젝트 풀링
             * 2. 정점수 줄이기 (Level of Detail) 멀리있는 것은 정점수를 줄여서 대충그리고 가까이있는것만 정점수를 많이 가져서 디테일하게 그리기
             * 3. 파티클에서 3D모델보다는 가급적 스프라이트를 사용하기 (이미지를 이용한 파티클을 사용하는게 줄일수있다고 한다..?)
             * 4. 비어있는 함수 제거하기
             * 5. 레이어 마스크 사용 충돌처리 
             * - 유니티 내부적으로 속도향상을 위해 비트연산 처리가 된다. 총 32비트를 사용하기 때문에 레이어도 32개까지 추가 가능함.
             * 

             */
            //- 레이어 마스크 사용방법
            //int layer = gameObject.layer;
            ////layer = 1 << 8;
            ////0000 0000 0000 0001 -> 0000 0001 0000 0000 
            //layer = 1 << 8 | 1 << 9 | 1 << 12;
            ////0000 0001 0000 0001 -> Player
            ////0000 0010 0000 0001 -> Enemy
            ////0001 0000 0000 0001 -> Boss
            ////0001 0011 0000 0001 -> P,E,B 모두다 충돌처리 가능

            //if(Physics.Raycast(ray,out hitInfo, 100, layer)) // 레이어의 포함되어 있는 아이들만 충돌하라는 의미이다. ~layer (not비트연산자)
            //{

            //}


            //마우스 우측 버튼 클릭시 수류탄 던지기



            EnemyAtk(hitInfo, normalDam);


        }
        if (Input.GetButtonDown("Fire2") && bombCount<bombCountMax)
        {
            bombCount++;
                anim.animator.SetTrigger("Bomb");
                StartCoroutine(coBomb(0.375f));
        }
        IEnumerator coBomb(float num)
        {
            yield return new WaitForSeconds(num);

            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePoint.transform.position;
            //폭탄은 플레이어가 던지기 때문에 폭탄의 리지드바디를 이용해서 던진다.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //전방으로 물리적인 힘을 가한다. 
            rb.AddForce((cam.transform.forward + cam.transform.up).normalized * bombPower, ForceMode.Impulse);
        }
    }

    private void EnemyAtk(RaycastHit hitInfo, int damage)
    {
        if(hitInfo.collider.tag == "Enemy")
        {
            EnemyFSM enemy = hitInfo.collider.GetComponent<EnemyFSM>();
            if (enemy == null) return;
            enemy.Damaged(damage);
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastAtkTime + atkTime)
        {
        
        RaycastHit hit;
        //Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward,out hit,distanceMax))
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = hit.point;
            lastAtkTime = Time.time;

        }
        }
    }
}
