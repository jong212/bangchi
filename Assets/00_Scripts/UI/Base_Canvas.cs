using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    private void Awake()
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

    public Transform Coin;
    [SerializeField] Transform _layer;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Utils.UI_Holder.Count >0 )
            {
                Utils.ClosePopupUI();
            } else
            {
                Debug.Log("게임 종료 팝업");
            }

        }
    }

    public Transform HolderLayer(int value)
    {
        return _layer.GetChild(value);
    }


    public void GetUI(string temp)
    {
        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        Utils.UI_Holder.Push(go);
    }
}
