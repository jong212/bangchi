using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

/*
// Base_Mng �� ��� �Ŵ��� ��ũ��Ʈ�� �����ϱ� ���� �̱��� ������� ���� �Ѵ�.
// �������� Base_Mng ���� �� �ٸ� �Ŵ����� ���� ������ �� ���� Base_Mng.pool �̷�������
 */
public class Base_Mng : MonoBehaviour
{
    private static Player_Mng   s_Player = new Player_Mng();
    private static Pool_Mng     s_Pool   = new Pool_Mng();
    private static Stage_Mng s_Stage;
    public static Stage_Mng Stage => s_Stage ??= new Stage_Mng();

    public static Base_Mng      instance = null; 
    public static Pool_Mng      Pool     { get { return s_Pool; } } 
    public static Player_Mng    Player   { get { return s_Player; } }    

    private void Awake()
    {
        singletonInit();
        Init_Managers();
        GameStartOrder();
    }
    private void Init_Managers()
    {
       
    }
    private void singletonInit()
    {
        if(instance == null)
        {
            instance = this;            
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        
    }
    // 1. Stage Mng �ʱ�ȭ
    // 2. MainUI���� �ʱ�ȭ �� Stage Mng�� �����ؼ� FadeOut �ֱ�
    // 3. ���� ����
    void GameStartOrder()
    {
        
        _ = Stage;
        MainUI.instance.RegisterStageEvents();
        Stage.State_Change(Stage_State.Ready);
    }
    public GameObject instantiate_path(string path) // instantiate_path ���� ���ӿ�����Ʈ�� ��ȯ�ϴ� ������ Ǯ �Ŵ����� ��� ����� �� �ް� �ֱ� ������ instantiate�� �� �� ��� ���⼭ �����ϰ� ��ȯ�ϴ� ���̴�.
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    public void Coroutine_Action(float timer, Action action)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }
    IEnumerator Return_Pool_Coroutine(float time, GameObject obj, string path)
    {
        yield return new WaitForSeconds(time);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }
}
