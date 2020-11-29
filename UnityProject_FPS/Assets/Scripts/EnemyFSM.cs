using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 에너미상태
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die,
    }

    EnemyState state; // 에너미스테이트를 변수한것.

    private Animator animator;

    //유용한 기능
    #region "Idle 상태에 필요한 변수들"
    GameObject target;
    float searchArena = 20f;
    #endregion

    #region "Move 상태에 필요한 변수들"
    private CharacterController characterC;
    [SerializeField]  float speed = 5f;
    private float gravityPower = 20f;


    #endregion

    #region "Attack 상태에 필요한 변수들"
    int atkPower = 10;
    float atkSpeed = 1f;
    float atkArena;
    float atkDistance = 1.5f;
    float lastAtkTime=0f;
    public GameObject BulletFactory;
    #endregion

    #region "Return 상태에 필요한 변수들"
    Vector3 startPosition;
    float returnDistance = 30f;
    Vector3 startRotate;
    #endregion

    #region "Damaged 상태에 필요한 변수들"
    [SerializeField] int hp = 50;
    [SerializeField] float hitTime = 2f;
    private float lastHitTime = 0f;
    #endregion

    # region "Die 상태에 필요한 변수들"
    #endregion


    private void Awake()
    {
        animator = transform.Find("warrior_red").GetComponent<Animator>();
    }

    void Start()
    {
        //에너미 상태 초기화
        state = EnemyState.Idle;
        target = GameObject.Find("Player");
        characterC = GetComponent<CharacterController>();
        startPosition = transform.position;
        startRotate = transform.eulerAngles;
        atkArena = characterC.radius + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        //에너미 상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

        animator.SetInteger("state", (int)state);
        print(animator.GetInteger("state"));
     }
    

    private void Idle()
    {
        //1. 플레이어와 일정범위가 되면 이동상태로 변경(탐지범위)
        // -플레이어 위치 찾기(GameObject.Find("Player")) <타겟찾기>
        // -일정범위 20m(거리비교 : Distance, magnitude 아무거나 사용)
        // -상태 변경 -> 이동
        // -상태전환 출력
        
        if(Vector3.Distance(transform.position,target.transform.position)<=searchArena)
        {
            state = EnemyState.Move;
        }
    }
    private void Move()
    {
        /* 1. 플레이어를 향해서 이동 후 공격범위 안에 들어오면 공격상태로 변경
         * 2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어감녀 리턴상태로 변경
         *  - 플레이어처럼 캐릭터 컨트롤러를 이욯가ㅣ
         *  - 공격범위 1m
         *  - 상태변경 -> 공격 or 리턴
         *  - 상태전환 출력
         */
        Vector3 dir = (target.transform.position - transform.position).normalized;
        //dir.y -= gravityPower;
        //characterC.Move(dir*speed*Time.deltaTime);
        
        characterC.SimpleMove(dir * speed );
        characterC.transform.LookAt(target.transform);

        if (Vector3.Distance(transform.position, startPosition) >= returnDistance)
        {
            state = EnemyState.Return;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= atkArena)
        {
            state = EnemyState.Attack;
        }
    }
    private void Attack()
    {
        /* 1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어를 공격
         * 2. 플레이어가 공격범위를 벗어나면 이동상태로 변경
         * - 공격범위 1m
         * - 상태변경 -> 이동
         * - 상태전환 출력 
         */
        //Debug.Log("atk");
        
        if(Time.time >=lastAtkTime )
        {
            GameObject bullet = Instantiate(BulletFactory);
            bullet.transform.position = target.transform.position;
            bullet.transform.forward = (target.transform.position - transform.position).normalized;
            lastAtkTime = Time.time + atkSpeed;
            target.GetComponent<PlayerState>().hpManager -= atkPower;
           // target.GetComponent<PlayerState>().Hit(atkPower);
            
        }

        if(Vector3.Distance(transform.position,target.transform.position)> atkArena)
        {
            state = EnemyState.Move;

        }
    }

    private void Return()//복귀상태
    {
        /* 1. 에너미가 플레이어를 추격하더라도 처음위치에서 일정범위를 벗어나면 다시 돌아옴
         * - 처음위치에서 일정범위 30m
         * - 상태변경
         * - 상태전환 출력
         */
        characterC.transform.LookAt(startPosition);
        characterC.Move(transform.forward*speed*Time.deltaTime);
        if((startPosition-transform.position).sqrMagnitude <=0.5f)
        {
            state = EnemyState.Idle;
            transform.eulerAngles = startRotate;
        }

       
    }
    public void Damaged(int damage=0) //피격상태 (Any State)
    {
        if (damage == 0) return; // 피격당하지 않은상태

        /* 코루틴 사용하자
         * 1. 에너미 체력이 1이상일때만, 피격을 받을수 있도록 한다.
         * 2. 다시 이전상태로 변경 (코루틴을 사용해서 처리)
         * - 상태변경
         * - 상태전환출력
         */
        
            //lastHitTime = Time.time + hitTime; Debug.Log(lastHitTime); 
        if(state != EnemyState.Return)
            state = EnemyState.Damaged;
                               
        hp -= damage;
        if (hp <= 0)
            state = EnemyState.Die;
        //if (Time.time >= lastHitTime)dssssss
        //    state = EnemyState.Idle;
        
        else
        StartCoroutine(hitDelayCo());
       
    }
    IEnumerator hitDelayCo()
    {
        yield return new WaitForSeconds(hitTime);
        if (state != EnemyState.Return)
            state = EnemyState.Idle;
    }
    private void Die() //Any State
    {
        /* 코루틴 사용하자
         * 1. 체력이 0이하일 때 작동.
         * 2. 몬스터 오브젝트 삭제
         * - 상태변경
         * - 상태전환 출력
         */
        StartCoroutine(dieCo(1));
        
    }
    IEnumerator dieCo(int num)
    {
        yield return new WaitForSeconds(num);
        Destroy(gameObject);
    }
}

