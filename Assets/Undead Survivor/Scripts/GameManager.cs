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
    // public Transform uiJoy;  �̰� �� ��ũ��Ʈ�� ���� ������ �ȵǳ�
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;    // Awake �����ֱ⿡�� �ν��Ͻ� ������ �ڱ��ڽ� this�� �ʱ�ȭ
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerID = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        // �ӽ� ��ũ��Ʈ (ù��° ĳ���� ����)
        uiLevelUp.Select(playerID % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());   // �׳� Stop�ϸ� ���� �ִϸ��̼� �����ϱ� delay ��Ű��

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
        StartCoroutine(GameVictoryRoutine());   // �׳� Stop�ϸ� ���� �ִϸ��̼� �����ϱ� delay ��Ű��
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
        Application.Quit();     // �����͸� �����ϴ� ����� �ƴϱ� ������ ���� ���������� �۵���.
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

        if(exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])    // ���� ����ġ�� ����� ����
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
