using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;    // ��� �ݶ��̴��� �⺻ ������ �ƿ츣�� Ŭ����

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)     // Ʈ���Ű� üũ�� collider���� ������ �� �߻��ϴ� �Լ�
    {
        if (!collision.CompareTag("Area"))    // �츮�� Player���� ���Ӱ� �߰��� Area�̶� �浹�ؼ� ����� �� Ÿ�ϸ��� �������� ��.
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        Vector3 playerDirr = GameManager.instance.player.inputVec;  // �÷��̾��� �̵� ������ �����ϱ� ���� ���� �߰�

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
