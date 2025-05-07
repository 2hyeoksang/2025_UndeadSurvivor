using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    float SfxDelay = 0.25f;
    float SfxTimer = 0f;
    bool isDamaged = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // Player 안에 있는 Rigidbody2D가 rigid 라는 변수에 들어가게 된 것
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }


    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerID];
    }

    private void Update()
    {
        if (isDamaged)
        {
            SfxTimer -= Time.deltaTime;
            if (SfxTimer < 0f)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.PlayerHit);
                SfxTimer = SfxDelay;
            }
        }

        else
        {
            SfxTimer = 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();    // Get<T> : 프로필에서 설정한 컨트롤 타입 T 값을 가져오는 함수
    }

     void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

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
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);  
        // SetFloat 첫 번째 인자 : Parameter 이름, 두 번째 인자 : inputVec.magnitude : 벡터의 순수한 크기 값

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;     // inputVec.x < 0 가 true or false 반환
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isDamaged = true;
        }

        GameManager.instance.health -= Time.deltaTime * 10;
        
        if (GameManager.instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isDamaged = false;
        }
    }
}
