using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("# SFX")]   // 효과음
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // 다양한 효과음을 낼 수 있도록 채널 개수 변수 선언
    AudioSource[] sfxPlayers;
    int channelsIndex;  // 맨 마지막에 실행했던 player의 index

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win, PlayerHit } // 옆에 숫자는 index 번호


    private void Awake()
    {
        instance = this;
        Init();
    }


    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // AddComponent 함수로 AudioSource를 생성하고 변수에 저장
        bgmPlayer.playOnAwake = false;  // 게임을 켜자마자 브금이 나오는게 아닌, 캐릭터를 누를 때 브금이 나와야함.
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];     // 채널값을 사용하여 오디오소스 배열 초기화
        // 이건 배열만 초기화한것, 내용물은 초기화 안돼서 컴포넌트가 없음

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }


    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }


    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }


    public void PlaySfx(Sfx sfx)
    {
        // 채널 개수만큼 순회하도록 채널인덱스 변수 활용
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelsIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelsIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;  // 효과음 재생이 된 이후에는 꼭 break로 반복문 종료
        }
    }

}
 