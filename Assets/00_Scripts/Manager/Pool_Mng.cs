using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ Ǯ �������̽�
public interface IPool
{
    Transform parentTransform { get; set; }                        // ������Ʈ���� �θ� ����
    Queue<GameObject> pool { get; set; }                           // ������Ʈ ť�� ����    
    GameObject Get(Action<GameObject> action = null);              // ������Ʈ�� �������� �޼��� (�ɼ����� ������ �� �߰� ������ ������ �� ����)
    void Return(GameObject obj, Action<GameObject> action = null); // ������Ʈ�� ��ȯ�ϴ� �޼��� (�ɼ����� ��ȯ �� �߰� ������ ������ �� ����)
}

// ������Ʈ Ǯ�� �⺻ ����ü
public class Object_Pool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();// ������Ʈ�� ������ ť
    public Transform parentTransform { get; set; }                        // ������Ʈ Ǯ�� �θ� ������Ʈ
    public GameObject Get(Action<GameObject> action = null)               // ������Ʈ Ǯ���� �ϳ��� ������ ��ȯ
    {
        GameObject obj = pool.Dequeue();                                  // ť���� ������Ʈ ������
        obj.SetActive(true);                                              // Ȱ��ȭ
        action?.Invoke(obj);                                              // �߰����� ���� ���� (������)

        return obj;
    }

    // ������Ʈ�� �ٽ� ������Ʈ Ǯ�� ��ȯ
    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);                      // ť�� �ٽ� �ֱ�
        obj.transform.parent = parentTransform; // �θ� Ǯ�� Transform���� ����
        obj.SetActive(false);                   // ��Ȱ��ȭ
        action?.Invoke(obj);                    // �߰����� ���� ���� (������)
    }
}

// ������Ʈ Ǯ�� �����ϴ� Ŭ����
public class Pool_Mng
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();// ���� ���� ������Ʈ Ǯ�� �����ϴ� ��ųʸ� (key: ������ ���, value: ������Ʈ Ǯ)
    public IPool Pooling_Obj(string path)                                                // Ư�� ���(path)�� ���� ������Ʈ Ǯ�� �������ų�, ������ �����ϴ� �޼���
    {
        if (!m_pool_Dictionary.ContainsKey(path))                                        // ��ųʸ��� �ش� ������Ʈ Ǯ(key)�� ������ ����
        {
            Add_Pool(path);
        }

        if (m_pool_Dictionary[path].pool.Count <= 0)                                     // Ǯ�忡 ����� ������ ����� ����� Ǯ�忡 ����� �ִ´� 
        {
            Add_Queue(path);
        }
        return m_pool_Dictionary[path];                                                  // Ǯ�� ��ȯ (Ǯ���� �ִٸ� �� ���� �� ��ġ�µ� ���ٸ� �� ������ ���� ������ Ǯ���� ��������� ��� ���� �ϳ� ��������� ���� Ǯ���� ������ �����Ѵ�.
    }

    // ���ο� ������Ʈ Ǯ�� �����ϴ� �޼��� (Ǯ���� ���� �� Ǯ���� �����)
    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##Pool");     // �� ������Ʈ ����
        obj.transform.SetParent(Base_Mng.instance.transform); // obj�� �θ� Base_Mng�� ���� => ���̽� �Ŵ����� �ְ� �� ������ Ǯ�� ������Ʈ��(monster##pool,bullet##pool,effect##pool ��) ��ƵѶ��

        Object_Pool T_Component = new Object_Pool();          // �� Ǯ�� ����

        m_pool_Dictionary.Add(path, T_Component);             // Ǯ�� ���� ������ ��ųʸ��� ���� Ǯ������ ���

        T_Component.parentTransform = obj.transform;          // �� ������Ʈ(obj)�� transform�� ������ Ǯ(T_Component)�� parentTransform ������ ���� ���߿� Add_Queue()���� ������Ʈ�� ������ ��, �θ� �� parentTransform���� �����ϱ� ����
        return obj;
    }

    // Ǯ�忡 �� ����� �����ϰ� Ǯ�忡 �ֱ�
    private void Add_Queue(string path)
    {
        var go = Base_Mng.instance.instantiate_path("Pool_Obj/" + path);             // Resources���� ������Ʈ ����
        go.transform.parent = m_pool_Dictionary[path].parentTransform; // ������ ������Ʈ�� �θ� �ش� ������Ʈ Ǯ�� Transform���� ����
        m_pool_Dictionary[path].Return(go);                            // ������ ������Ʈ�� Ǯ�� ��ȯ (��, ��Ȱ��ȭ ���·� ť�� �߰���)
    }
}
