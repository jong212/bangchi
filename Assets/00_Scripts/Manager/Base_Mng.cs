using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;
    private static Pool_Mng s_Pool = new Pool_Mng();
    public static Pool_Mng Pool { get { return s_Pool; } } 
    // Start is called before the first frame update
    private void Awake()
    {
        initalize();
    }

    private void initalize()
    {
        if(instance == null)
        {
            instance = this;
            Pool.initalize(transform);
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
    public GameObject instantiate_path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
}
