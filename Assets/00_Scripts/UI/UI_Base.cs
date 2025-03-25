using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    protected bool init = false;

    public virtual bool Init()
    {
        if (init)
        {
            return false;
        }
        return init = true;
    }

    private void Start()
    {
        Init();
    }

    public virtual void DisableObj()
    {
        Utils.UI_Holder.Pop();
        Destroy(this.gameObject);
    }



}
