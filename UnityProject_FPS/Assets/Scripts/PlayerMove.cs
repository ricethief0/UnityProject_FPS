using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    [SerializeField] float yMin = -70f;
    [SerializeField] float yMax = 70f;
    [SerializeField] float gravity = 20f;
    [SerializeField] float jumpSpeed = 8f;
    int jumpCount=0;
    float velocityY=0f;
    Vector3 dir = Vector3.zero;
    
    private float mouseX=0f;
    private float mouseY=0f;
    private float rSpeed = 2.5f;
    private CharacterController controllerP;
  
    // Start is called before the first frame update
    void Start()
    {
        controllerP = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // Move1(); 내가 한거

        // Move2(); // 수업
        Move3(); // 점프수업
        
    }

    private void Move3()
    {
        
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v).normalized;

            dir = Camera.main.transform.TransformDirection(dir);

        
        
        //중력적용
        if(controllerP.isGrounded) // 플레이어가 땅에 닿아 있는 상태냐?
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            velocityY -= gravity * Time.deltaTime; //gravity
        }
            if (Input.GetButtonDown("Jump") && jumpCount <2)
            {
                velocityY = jumpSpeed;
            jumpCount++;
            }
        //if(controllerP.collisionFlags == CollisionFlags.Below) // Above / Side 도 있음.  
        //{
        //
        //}
        dir.y = velocityY;
        controllerP.Move(dir * speed * Time.deltaTime);
    }

    private void Move2()
    {
        if (controllerP.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v).normalized;

            //transform.Translate(dir * speed * Time.deltaTime);

            dir = Camera.main.transform.TransformDirection(dir);

            
            if(Input.GetButtonDown("Jump"))
            {
                dir.y = jumpSpeed;
            }
        }
        //transform.Translate(dir * speed * Time.deltaTime);

        //문제점 : 충돌처리안됌, 공중부양, 땅파고들기
        //캐릭터 컨트롤러 컴포넌트를 사용한다 위와 같은 문제점을 해결하기 위해
        //캐릭터 컨트롤러는 충돌감지만 하고 물리적용이 안됌.
        // 따라서 충돌감지 하기 위해서는 반드시 캐릭터 컨트롤러 컴포넌트가 제공해주는 함수로 이동처리 해야한다.
        dir.y -= gravity * Time.deltaTime; //gravity
        controllerP.Move(dir * speed * Time.deltaTime);
    }

    private void Move1()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y") * rSpeed;

        mouseY = Mathf.Clamp(mouseY, yMin, yMax);
        controllerP.transform.rotation = Quaternion.Euler(mouseY, mouseX * rSpeed, 0);
        transform.Rotate(Vector3.forward * 0);

        if (v != 0)
            controllerP.Move(transform.forward * v * Time.deltaTime * speed);
        if (h != 0)
            controllerP.Move(transform.right * h * Time.deltaTime * speed);
    }
}
