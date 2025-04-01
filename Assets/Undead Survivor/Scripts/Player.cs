using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // Player 안에 있는 Rigidbody2D가 rigid 라는 변수에 들어가게 된 것
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    //void Update()
    //{
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

     void FixedUpdate()
    {
        //// 1. 힘을 준다.
        //rigid.AddForce(inputVec);

        //// 2. 속도 제어
        //rigid.linearVelocity = inputVec;  얘네는 참고 내용

        // 3. 위치 이동
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime ; 
        // normalized : 어느 방향이든지 벡터 값의 크기가 1이 되도록 좌표가 수정된 값
        // fixedDeltaTime : FixedUpdate 한번만큼, 즉 물리 프레임 하나가 소모된 시간
        rigid.MovePosition(rigid.position + nextVec);  // inputVec는 월드 위치, 현재 위치에 inputVec 만큼의 포지션이 더해지는 것
    }

    void LateUpdate()   // 프레임이 종료되기 전 실행되는 생명주기 함수
    {
        anim.SetFloat("Speed", inputVec.magnitude);  
        // SetFloat 첫 번째 인자 : Parameter 이름, 두 번째 인자 : inputVec.magnitude : 벡터의 순수한 크기 값

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;     // inputVec.x < 0 가 true or false 반환
        }
    }
}
