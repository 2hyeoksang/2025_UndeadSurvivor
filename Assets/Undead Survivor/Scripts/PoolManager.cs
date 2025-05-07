using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ���� : 2��
    public GameObject[] prefabs;

    // ������Ʈ Ǯ�� �����ϴ� ����Ʈ���� �ʿ� : 2��
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        // pool�̶� GameObject�� ���� List�� prefabs�� ���̸�ŭ ���� ����Ʈ -> ����Ʈ �ȿ� ����Ʈ

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���� ������Ʈ ����
            
            // ... �߰��ϸ� select ������ �Ҵ�

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // ... �� ã������?
            
        if (select == null)
        {
            // ... ���Ӱ� �����ϰ� select ������ �Ҵ� 
            select = Instantiate(prefabs[index], transform);    // ��� �����Ǹ� �����������ϱ� PoolManager�� �ְڴ� (����)
            pools[index].Add(select);
        }

        return select;
    }
}
