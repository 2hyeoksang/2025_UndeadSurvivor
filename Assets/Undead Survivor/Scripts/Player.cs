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
        rigid = GetComponent<Rigidbody2D>();    // Player �ȿ� �ִ� Rigidbody2D�� rigid ��� ������ ���� �� ��
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
        inputVec = value.Get<Vector2>();    // Get<T> : �����ʿ��� ������ ��Ʈ�� Ÿ�� T ���� �������� �Լ�
    }

     void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //// 1. ���� �ش�.
        //rigid.AddForce(inputVec);

        //// 2. �ӵ� ����
        //rigid.linearVelocity = inputVec;  ��״� ���� ����

        // 3. ��ġ �̵�
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime ; 
        // normalized : ��� �����̵��� ���� ���� ũ�Ⱑ 1�� �ǵ��� ��ǥ�� ������ ��
        // fixedDeltaTime : FixedUpdate �ѹ���ŭ, �� ���� ������ �ϳ��� �Ҹ�� �ð�
        rigid.MovePosition(rigid.position + nextVec);  // inputVec�� ���� ��ġ, ���� ��ġ�� inputVec ��ŭ�� �������� �������� ��
    }

    void LateUpdate()   // �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);  
        // SetFloat ù ��° ���� : Parameter �̸�, �� ��° ���� : inputVec.magnitude : ������ ������ ũ�� ��

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;     // inputVec.x < 0 �� true or false ��ȯ
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
