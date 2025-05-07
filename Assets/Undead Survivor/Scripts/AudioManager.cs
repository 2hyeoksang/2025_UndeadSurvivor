using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("# SFX")]   // ȿ����
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // �پ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    AudioSource[] sfxPlayers;
    int channelsIndex;  // �� �������� �����ߴ� player�� index

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win, PlayerHit } // ���� ���ڴ� index ��ȣ


    private void Awake()
    {
        instance = this;
        Init();
    }


    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // AddComponent �Լ��� AudioSource�� �����ϰ� ������ ����
        bgmPlayer.playOnAwake = false;  // ������ ���ڸ��� ����� �����°� �ƴ�, ĳ���͸� ���� �� ����� ���;���.
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];     // ä�ΰ��� ����Ͽ� ������ҽ� �迭 �ʱ�ȭ
        // �̰� �迭�� �ʱ�ȭ�Ѱ�, ���빰�� �ʱ�ȭ �ȵż� ������Ʈ�� ����

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
        // ä�� ������ŭ ��ȸ�ϵ��� ä���ε��� ���� Ȱ��
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
            break;  // ȿ���� ����� �� ���Ŀ��� �� break�� �ݺ��� ����
        }
    }

}
 