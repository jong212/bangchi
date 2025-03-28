using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int m_Count;
    public float m_SpawnTime;

    // 스포너에 쉽게 접근하기 위해 static
    public static List<Monster> m_Monster = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    Coroutine coroutine;
    private void Start()
    {
        Base_Mng.Stage.PlayEvent += OnPlay;
        Base_Mng.Stage.BossEvent += OnBoss;
    }

    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }
    public void OnBoss()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        for (int i = 0; i < m_Monster.Count ; i++)
        {
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(m_Monster[i].gameObject);
        }
        m_Monster.Clear();
    }
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for (int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            pos.y = 0.0f;

             while(Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            var goObj = Base_Mng.Pool.Pooling_Obj("Monster").Get((value) =>
            {
                value.GetComponent<Monster>().init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);

                m_Monster.Add(value.GetComponent<Monster>());
            });

        }
        yield return new WaitForSeconds(m_SpawnTime);

        StartCoroutine(SpawnCoroutine());
    }
}
