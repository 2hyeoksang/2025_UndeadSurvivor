using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    float timer;
    int level;
    public float levelTime;


    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.MaxGameTime / spawnData.Length;
        // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;     // deltaTime : 한 프레임이 소비한 시간
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);  
        // 적절한 숫자로 나누어 시간에 맞춰 레벨이 올라가도록 작성
        // FloorToInt : 소수점 아래는 버리고 Int형으로 바꾸는 함수, CeilToInt : 소수점 올리는거

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
        // 1부터 하는 이유 : GetComponentsInChildren을 쓰면 자기 자신도 포함, Spawner 오브젝트도 첫 번째로 spawnPoint 배열에 추가됨
        // 자식 오브젝트에서만 선택되도록 하기 위해.
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// 직렬화 (Serializetion) : 개체를 저장 혹은 전송하기 위해 변환
[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
    public int enemyExp;
}