using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    float timer;
    int level;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;     // deltaTime : �� �������� �Һ��� �ð�
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);  // ������ ���ڷ� ������ �ð��� ���� ������ �ö󰡵��� �ۼ�
        // FloorToInt : �Ҽ��� �Ʒ��� ������ Int������ �ٲٴ� �Լ�, CeilToInt : �Ҽ��� �ø��°�

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // 1���� �ϴ� ���� : GetComponentsInChildren�� ���� �ڱ� �ڽŵ� ����, Spawner ������Ʈ�� ù ��°�� spawnPoint �迭�� �߰���
        // �ڽ� ������Ʈ������ ���õǵ��� �ϱ� ����.
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// ����ȭ (Serializetion) : ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ
[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}