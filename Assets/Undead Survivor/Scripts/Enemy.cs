using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;    // 현재의 체력
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
        rigid.MovePosition(rigid.position + nextVec);   // 플레이어의 키 입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        rigid.linearVelocity = Vector2.zero;   // 물리 속도가 이동에 영향을 주지 않도록 제거
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable() // 스크립트가 활성화될 때, 호출되는 이벤트 함수
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
        if (!collision.CompareTag("Bullet") || !isLive)     // 사망 로직이 연달아 실행되는 것을 방지하기 위해 조건 추가
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());    // 문자열로 불러도 됨

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
            coll.enabled = false;   // collider 비활성화
            rigid.simulated = false;    // rigidbody의 물리적 비활성화는 simulated
            spriter.sortingOrder = 1;
            anime.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if(GameManager.instance.isLive)     // 언데드 사망 사운드는 게임 종료 시에는 나지 않도록 조건 추가
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;  // 다음 하나의 물리 프레임을 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
