using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀 인터페이스
public interface IPool
{
    Transform parentTransform { get; set; }                        // 오브젝트들의 부모 역할
    Queue<GameObject> pool { get; set; }                           // 오브젝트 큐로 관리    
    GameObject Get(Action<GameObject> action = null);              // 오브젝트를 가져오는 메서드 (옵션으로 가져온 후 추가 동작을 지정할 수 있음)
    void Return(GameObject obj, Action<GameObject> action = null); // 오브젝트를 반환하는 메서드 (옵션으로 반환 후 추가 동작을 지정할 수 있음)
}

// 오브젝트 풀의 기본 구현체
public class Object_Pool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();// 오브젝트를 저장할 큐
    public Transform parentTransform { get; set; }                        // 오브젝트 풀의 부모 오브젝트
    public GameObject Get(Action<GameObject> action = null)               // 오브젝트 풀에서 하나를 꺼내서 반환
    {
        GameObject obj = pool.Dequeue();                                  // 큐에서 오브젝트 꺼내기
        obj.SetActive(true);                                              // 활성화
        action?.Invoke(obj);                                              // 추가적인 동작 수행 (선택적)

        return obj;
    }

    // 오브젝트를 다시 오브젝트 풀에 반환
    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);                      // 큐에 다시 넣기
        obj.transform.parent = parentTransform; // 부모를 풀의 Transform으로 설정
        obj.SetActive(false);                   // 비활성화
        action?.Invoke(obj);                    // 추가적인 동작 수행 (선택적)
    }
}

// 오브젝트 풀을 관리하는 클래스
public class Pool_Mng
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();// 여러 개의 오브젝트 풀을 관리하는 딕셔너리 (key: 프리팹 경로, value: 오브젝트 풀)
    public IPool Pooling_Obj(string path)                                                // 특정 경로(path)에 대한 오브젝트 풀을 가져오거나, 없으면 생성하는 메서드
    {
        if (!m_pool_Dictionary.ContainsKey(path))                                        // 딕셔너리에 해당 오브젝트 풀(key)이 없으면 생성
        {
            Add_Pool(path);
        }

        if (m_pool_Dictionary[path].pool.Count <= 0)                                     // 풀장에 사람이 없으면 사람을 만들고 풀장에 사람을 넣는다 
        {
            Add_Queue(path);
        }
        return m_pool_Dictionary[path];                                                  // 풀장 반환 (풀장이 있다면 위 과정 안 거치는데 없다면 위 과정을 통해 무조건 풀장이 만들어지고 사람 역시 하나 만들어진다 따라서 풀장을 무조건 리턴한다.
    }

    // 새로운 오브젝트 풀을 생성하는 메서드 (풀장이 없을 때 풀장을 만든다)
    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##Pool");     // 빈 오브젝트 생성
        obj.transform.SetParent(Base_Mng.instance.transform); // obj의 부모를 Base_Mng로 설정 => 베이스 매니저가 있고 그 밑으로 풀장 오브젝트들(monster##pool,bullet##pool,effect##pool 등) 모아둘라고

        Object_Pool T_Component = new Object_Pool();          // 찐 풀장 생성

        m_pool_Dictionary.Add(path, T_Component);             // 풀장 생성 했으니 딕셔너리에 무슨 풀장인지 기록

        T_Component.parentTransform = obj.transform;          // 빈 오브젝트(obj)의 transform을 생성한 풀(T_Component)의 parentTransform 변수에 저장 나중에 Add_Queue()에서 오브젝트를 생성할 때, 부모를 이 parentTransform으로 설정하기 위해
        return obj;
    }

    // 풀장에 들어갈 사람을 생성하고 풀장에 넣기
    private void Add_Queue(string path)
    {
        var go = Base_Mng.instance.instantiate_path("Pool_Obj/" + path);             // Resources에서 오브젝트 생성
        go.transform.parent = m_pool_Dictionary[path].parentTransform; // 생성된 오브젝트의 부모를 해당 오브젝트 풀의 Transform으로 설정
        m_pool_Dictionary[path].Return(go);                            // 생성된 오브젝트를 풀에 반환 (즉, 비활성화 상태로 큐에 추가됨)
    }
}
