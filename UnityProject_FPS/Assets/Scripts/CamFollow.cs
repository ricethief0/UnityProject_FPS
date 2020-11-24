using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // 카메라가 플레이어 따라다니기 
    // 플레이어 자식으로 붙여서 이동해도 상관없다.
    // 하지만 게임에 따라서 드라마틱한 연출이 필요할떄라던지
    // 게임기획에 따라 1인칭 또는 3인칭 등 변경이 필요할 수있다.
    // 지금은 우리 눈역할을 할거라서 그냥 순간이동 시킨다.

    public Transform target;    // 카메라가 따라다니게 할 타겟.
    public float speed = 10f;   //카메라 이동속도
    public Transform target1st;//1인칭 시점
    bool isFPS = false;         //1인칭이냐 or 3인칭이냐

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FollowTarget();
        //1인칭 to 3인칭, 3인칭 to 1인칭으로 카메라 변경
        ChangeView();

    }
    private void LateUpdate()
    {
        if (isFPS)
        {
            //카메라의 위치를 강제로 타겟에 고정해둔다.
            transform.position = target1st.position;
        }
        else
        {
            transform.position = target.position;
        }
    }

    private void ChangeView()
    {
        if(Input.GetKeyDown("1"))
        {
            isFPS = true;
        }
        if (Input.GetKeyDown("3"))
        {
            isFPS = false;
        }
       
    }


    private void FollowTarget()
    {
        // 타겟의 방향을 가져온다. (구한다)
        //방향 = 타겟 - 본인
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.Translate(speed * dir * Time.deltaTime);
        
        // 위와 같이 코드를 작성하면 도착지점을 사이로 계속 움직이기 때문에 덜덜덜 떨리는 현상이 나타난다.
        //해결방법은 일정거리가 되면 값을 고정시켜 계속 움직이는 것을 방지한다.

        
        //1. 벡터안의 Distance()함수를 사용 => 실수를 리턴 (정확한 계산을 위해서는 이걸 사용해야한다.) 충돌계산
        //2. 벡터안의 magnitude 속성을 사용 => 실수를 리턴 (정확한 계산을 위해서는 이걸 사용해야한다.) 충돌계산
        //3. 벡터안의 sqrMagnitude 속성을 사용 => 실수를 리턴 (최적화를 위해서는 이게 좋다) 대신 값이 대충나온다. 즉, 크기비교용

        //1. Distance() 
        //if(Vector3.Distance(target.position,transform.position) < 1.0f)
        //{
        //    transform.position = target.position;
        //}

        //2. magnitude
        //float distance = (target.position - transform.position).magnitude;
        //if (distance < 1.0f)
        //{
        //    transform.position = target.position;
        //}

        //3. sqrMagnitude 정확한 값은 아님. 비교용으로만 사용할것. 사용하는 이유는 sqrt함수를 사용하지 않기때문에 빠름.
        float distance = (target.position - transform.position).sqrMagnitude;  //<- 위에서 뒤에 magnitude가 아니라 sqrMagnitude로만 바꿔줌.
        if (distance < 1.0f)
        {
            transform.position = target.position;
        }

        //위와 같이 하고도 덜덜거리는 경우 
        //FollowTarget을 LateUpdate()에 넣으면 해결된다. 꿀팁!

    }
}
