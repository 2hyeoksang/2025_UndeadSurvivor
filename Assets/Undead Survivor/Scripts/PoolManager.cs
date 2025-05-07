using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수 : 2개
    public GameObject[] prefabs;

    // 오브젝트 풀을 구성하는 리스트들이 필요 : 2개
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        // pool이란 GameObject를 담을 List를 prefabs의 길이만큼 만든 리스트 -> 리스트 안에 리스트

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임 오브젝트 접근
            
            // ... 발견하면 select 변수에 할당

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // ... 못 찾았으면?
            
        if (select == null)
        {
            // ... 새롭게 생성하고 select 변수에 할당 
            select = Instantiate(prefabs[index], transform);    // 계속 생성되면 지저분해지니깐 PoolManager에 넣겠다 (정리)
            pools[index].Add(select);
        }

        return select;
    }
}
