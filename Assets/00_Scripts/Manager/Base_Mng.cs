using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// Base_Mng 는 모든 매니저 스크립트를 관리하기 위해 싱글톤 방식으로 생성 한다.
// 전역에서 Base_Mng 접근 후 다른 매니저에 쉽게 접근할 수 있음 Base_Mng.pool 이런식으로
 */
public class Base_Mng : MonoBehaviour
{
    private static Pool_Mng     s_Pool   = new Pool_Mng();
    private static Player_Mng   s_Player = new Player_Mng();

    public static Base_Mng      instance = null; 
    public static Pool_Mng      Pool     { get { return s_Pool; } } 
    public static Player_Mng    Player   { get { return s_Player; } }    

    private void Awake()
    {
        singletonInit();
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

    public GameObject instantiate_path(string path) // instantiate_path 에서 게임오브젝트를 반환하는 이유는 풀 매니저가 모노 상속을 안 받고 있기 때문에 instantiate를 할 수 없어서 여기서 생성하고 반환하는 것이다.
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    IEnumerator Return_Pool_Coroutine(float time, GameObject obj, string path)
    {
        yield return new WaitForSeconds(time);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
}
