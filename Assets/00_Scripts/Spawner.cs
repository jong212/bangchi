using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monster_prefab;

    public int m_Count;
    public float m_SpawnTime;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for (int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere *  5.0f;
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
            });

            StartCoroutine(ReturnCoroutine(goObj));
        }
        yield return new WaitForSeconds(m_SpawnTime);

        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator ReturnCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(obj);
    }
}
