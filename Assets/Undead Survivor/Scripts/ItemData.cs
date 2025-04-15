using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemtype;
    public int itemID;
    public string itemName;
    [TextArea]      // �ν����Ϳ� �ؽ�Ʈ�� ���� �� ���� �� �ְ� TextArea �Ӽ� �ο� -> itemDesc�� ū â���� �ٲ�
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
