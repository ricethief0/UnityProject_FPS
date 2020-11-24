using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 150f;
    float angleX, angleY; //직접 제어할 회전각도

    // Update is called once per frame
    void Update()
    {
        //카메라 회전
        Rotate();
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        //Vector3 dir = new Vector3(h, v, 0);
        //회전은 각각의 축을 기반으로 회전이 된다.
        //Vector3 dir = new Vector3(-v, h, 0);
        //transform.Rotate(dir * speed * Time.deltaTime);

        //유니티엔진에서 제공해주는 함수를 사용함에 있어서
        //Translate함수는 벡터연산이다 보니 사용하는데 큰 불편함이 없지만, 
        //회전을 담당하는 Rotate함수를 사용하면 우리가 회전처리를 제어하기 힘들다.
        //인스펙터창의 로테이션값은 사용자가 보기 편하도록 오일러각도로 표시되지만,
        //내부적으로는 쿼터니언으로 회전처리가 되어있다.
        //쿼터니온을 사용하는이유는 짐벌락현상을 방지할 수 있기 때문이다. (회전축이 겹쳐져서 같이 움직는 현상)
        //따라서, 편하게 하기 위해서는 회전을 직접 제어할 때는 Rotate함수를 사용하지 않고 트렌스폼의 오일러 앵글을 사용하면 된다.


        //이동공식 
        //P = P0 + vt;
        //P += vt;
        //ex) transform.position += dir * speed * Time.deltaTime;


        //각도 공식도 비슷하다.
        //transform.eulerAngles += dir * speed * Time.deltaTime;

        //카메라 상하회전각도를 제한을 줘서 제어를 해야 한다.
        //Vector3 angle = transform.eulerAngles;
        //angle += dir * speed * Time.deltaTime;
        //transform.eulerAngles = angle;
        //if (angle.x > 60) angle.x = 60;
        //if (angle.x < -60) angle.x = -60;
        //transform.eulerAngles = angle;


        //여기에는 또 문제가 있다. 
        //유니티엔진 내부적으로 -각도는 360도를 더해서 처리된다.
        //내가 만든 각도를 가지고 계산처리하자.

        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, - 60, 60);

        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }
}
