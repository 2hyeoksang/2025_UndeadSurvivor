using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //// .. Test Code ..
        //if (Input.GetButtonDown("Jump"))
        //    LevelUp(10, 1);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);   
        // BroadcastMessag 를 초기화, 레벨업 함수 마지막 부분에서 호출
        // 데미지랑 카운트의 경우, 아이템을 누를 때 레벨업에서 적용 -> 다 올려놓은 GearDamage 없어짐
        // 레벨업했으니깐 이 레벨에 대한 기어 데미지도 올려달라 라는 뜻
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemID;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150f; // - 붙으면 시계방향
                Batch();
                break;

            default:
                speed = 0.3f; // 얘는 연사속도라고 생각하면 됨
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemtype];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; ++index)
        {
            Transform bullet;

            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }

            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  // PoolManager에서 1번 prefab을 가져와서 부모를 해당 Weapon으로 바꾸는 것
            }
                

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);     // -1 is Infinity Per.
        }
    }


    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;   // 벡터의 방향은 유지하고 크기는 1로 변환

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
