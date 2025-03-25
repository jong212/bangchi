using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Start()
    {
        heroBtn.onClick.AddListener(() => GetUI("#Heros", true));
    }
    public Transform Coin;
    [SerializeField] Transform layer;
    [SerializeField] Button heroBtn;

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
        return layer.GetChild(value);
    }


    public void GetUI(string temp, bool fade = false)
    {

        if (fade)
        {
            MainUI.instance.FadeInOut(false,true, () => GetPopupUI(temp));
            return;
        }
        GetPopupUI(temp);
    }
    void GetPopupUI(string temp)
    {
        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        Utils.UI_Holder.Push(go);
    }
}
