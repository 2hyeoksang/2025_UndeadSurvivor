using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textlevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textlevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        // 이렇게 되는 이유는 texts라는 배열을 Text 컴포넌트를 가진 자식 오브젝트를 반환하기 때문.
        // item은 Text라는 컴포넌트를 갖고 있지 않아서 자기 자신 포함 안됨
        textName.text = data.itemName;
    }


    private void OnEnable()     // 활성화되었을 때 자동으로 실행되는 이벤트
    {
        textlevel.text = "Lv." + (level + 1);

        switch (data.itemtype)
        {
            case ItemData.ItemType.Melee:   
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;

            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }

    }


    public void OnClick()
    {
        switch (data.itemtype)
        {
            case ItemData.ItemType.Melee:   // 얘네 로직은 똑같아서 이렇게 붙여놓으면 같은 코드 공유
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();    // 스크립트로 GameObject를 생성
                    weapon = newWeapon.AddComponent<Weapon>();   // 게임오브젝트에 컴포넌트를 추가하는 함수
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0) { 
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUP(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
