using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class HitText : MonoBehaviour
{
    Vector3 _target;
    Camera _cam;
    public TextMeshProUGUI Text;

    [SerializeField] GameObject _critical;
    private float _upRange = 0.0f;
    private void Start()
    {
        _cam = Camera.main;
    }

    public void init(Vector3 pos, double dmg,bool Monster = false, bool Critical = false)
    {
        pos.x += Random.Range(-0.1f, 0.3f);
        pos.z += Random.Range(-0.1f, 0.3f);

        _target = pos;
        Text.text = StringMethod.ToCurrencyString(dmg);

        if(Monster) Text.color = Color.red;
        else Text.color = Color.white;


        transform.parent = Base_Canvas.instance.HolderLayer(1);

        _critical.SetActive(Critical);
        Base_Mng.instance.Return_Pool(2.0f, this.gameObject, "Hit_Text");
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(_target.x, _target.y + _upRange, _target.z);
        transform.position = _cam.WorldToScreenPoint(targetPos);
        if(_upRange < .3f)
        {
            _upRange += Time.deltaTime;
        }
    }
    private void ReturnText()
    {
        Base_Mng.Pool.m_pool_Dictionary["Hit_Text"].Return(this.gameObject);
    }
}
