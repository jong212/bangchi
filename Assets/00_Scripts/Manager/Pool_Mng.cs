using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool {
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; } 
    GameObject Get(Action<GameObject> action = null);
    void Return(GameObject obj,Action<GameObject> action = null);

}

public class Object_Pool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();
    public Transform parentTransform { get ; set; }

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        if(action != null)
        {
            action?.Invoke(obj);
        }

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.parent = parentTransform;
        obj.SetActive(false);
        if(action != null)
        {
            action?.Invoke(obj);
        }
    }
}

public class Pool_Mng
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    Transform base_Obj = null;
    public void initalize(Transform T)
    {
        base_Obj = T;
    }
    public IPool Pooling_Obj(string path)
    {
        if(m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        if (m_pool_Dictionary[path].pool.Count <= 0) Add_Queue(path);
        return m_pool_Dictionary[path];
    }
    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##Pool");
        obj.transform.SetParent(base_Obj);
        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;     
        return obj;
    } 
    private void Add_Queue(string path)
    {
        var go = Base_Mng.instance.instantiate_path(path);

        // var go �� Resources ���� ������ ������Ʈ�� 
        // .parent�� ���� �� ������Ʈ�� �θ����?
        // parent = �� ������ ������Ʈ�� �θ� �����ϰڴ� ����?
        // ��ųʸ��� parentTransform����
        // parentTransofrm�� Add_Pool �޼��� �ܰ迡�� new GameObject �ؼ� Monster##Pool �̷������� �������
        // ����� Monster##Pool ������Ʈ�� �θ��̰� �� ������ Resourfes���� ���� �� ������Ʈ�� �ڽ����� �����ϰڴٴ� �ڵ�
        go.transform.parent = m_pool_Dictionary[path].parentTransform;

        m_pool_Dictionary[path].Return(go);
    }
}
