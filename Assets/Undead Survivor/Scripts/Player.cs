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
        rigid = GetComponent<Rigidbody2D>();    // Player �ȿ� �ִ� Rigidbody2D�� rigid ��� ������ ���� �� ��
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
        anim.SetFloat("Speed", inputVec.magnitude);  
        // SetFloat ù ��° ���� : Parameter �̸�, �� ��° ���� : inputVec.magnitude : ������ ������ ũ�� ��

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;     // inputVec.x < 0 �� true or false ��ȯ
        }
    }
}
