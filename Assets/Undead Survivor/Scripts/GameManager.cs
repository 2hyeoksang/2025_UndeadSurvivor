using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float MaxGameTime = 6 * 10f;

    [Header("# Player Info")]
    public int playerID;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    // public Transform uiJoy;  이거 왜 스크립트가 따로 적용이 안되냐
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;    // Awake 생명주기에서 인스턴스 변수를 자기자신 this로 초기화
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerID = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        // 임시 스크립트 (첫번째 캐릭터 선택)
        uiLevelUp.Select(playerID % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());   // 그냥 Stop하면 묘비 애니메이션 못보니깐 delay 시키고

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }


    public void GameVictory()
    {
        AudioManager.instance.PlayBgm(false);
        StartCoroutine(GameVictoryRoutine());   // 그냥 Stop하면 묘비 애니메이션 못보니깐 delay 시키고
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }


    public void GameQuit()
    {
        Application.Quit();     // 에디터를 종료하는 기능이 아니기 때문에 빌드 버전에서만 작동함.
    }


    private void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > MaxGameTime)
        {
            gameTime = MaxGameTime;
            GameVictory();
        }
    }

    public void GetExp(int enemyExp)
    {
        if (!isLive)
            return;

        exp += enemyExp;

        if(exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])    // 무한 경험치를 만드는 로직
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        // uiJoy.localScale = Vector3.zero;
    }


    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        // uiJoy.localScale = Vector3.one;
    }

    public void PressButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
}
