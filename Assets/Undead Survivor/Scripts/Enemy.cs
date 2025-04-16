using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;    // ������ ü��
    public float Maxhealth;

    public RuntimeAnimatorController[] animeCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anime;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anime = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anime.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);   // �÷��̾��� Ű �Է� ���� ���� �̵� = ������ ���� ���� ���� �̵�
        rigid.linearVelocity = Vector2.zero;   // ���� �ӵ��� �̵��� ������ ���� �ʵ��� ����
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable() // ��ũ��Ʈ�� Ȱ��ȭ�� ��, ȣ��Ǵ� �̺�Ʈ �Լ�
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anime.SetBool("Dead", false);
        health = Maxhealth;
    }

    public void Init(SpawnData data)
    {
        anime.runtimeAnimatorController = animeCon[data.spriteType];
        speed = data.speed;
        Maxhealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)     // ��� ������ ���޾� ����Ǵ� ���� �����ϱ� ���� ���� �߰�
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());    // ���ڿ��� �ҷ��� ��

        if (health > 0)
        {
            // .. Live, Hit Action
            anime.SetTrigger("Hit");

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        else
        {
            // .. Dead
            isLive = false;
            coll.enabled = false;   // collider ��Ȱ��ȭ
            rigid.simulated = false;    // rigidbody�� ������ ��Ȱ��ȭ�� simulated
            spriter.sortingOrder = 1;
            anime.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if(GameManager.instance.isLive)     // �𵥵� ��� ����� ���� ���� �ÿ��� ���� �ʵ��� ���� �߰�
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;  // ���� �ϳ��� ���� �������� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
