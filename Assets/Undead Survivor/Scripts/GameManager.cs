using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    void Awake()
    {
        instance = this;    // Awake 생명주기에서 인스턴스 변수를 자기자신 this로 초기화     
    }
}
