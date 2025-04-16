using System;
using System.Collections;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait;    // new를 하면 부를 때마다 새롭게 new를 하기 때문에 자원 낭비
    // 미리 선언을 하고 저장하는게 최적화에 도움이 됨
    // Realtime을 쓰는 이유 : 레벨업할 때 TimeScale이 0이 되는데 WaitForSecond는 TimeScale을 탐

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();     // 데이터가 없으면 초기화를 시켜주세요
                        // 우리가 처음에 플레이한 이후에 게임을 끄면 이제는 데이터가 남아있기 때문에 실행되지 않음
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);     // PlayerPrefs : 간단한 저장 기능을 제공하는 유니티 제공 클래스
                                             // SetInt 함수를 사용하여 key와 연결된 int형 데이터를 저장
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   // achive를 ToString()을 사용하여 문자열로 바꾸기
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    
    void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            // 잠금 버튼 배열을 순회하면서 인덱스에 해당하는 업적 이름 가져오기
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()   // 처리가 끝난 후속으로 점검을 하기 때문에 LateUpdate
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }


    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;

            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.MaxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
