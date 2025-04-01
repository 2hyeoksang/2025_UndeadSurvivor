using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;    // 모든 콜라이더의 기본 도형을 아우르는 클래스

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)     // 트리거가 체크된 collider에서 나갔을 때 발생하는 함수
    {
        if (!collision.CompareTag("Area"))    // 우리가 Player에서 새롭게 추가한 Area이랑 충돌해서 벗어났을 때 타일맵을 움직여야 함.
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        Vector3 playerDirr = GameManager.instance.player.inputVec;  // 플레이어의 이동 방향을 저장하기 위한 변수 추가

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }

                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDirr * 30 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f,3f), 0));
                }
                break;
        }
    }
}
