using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 이 스크립트는 뭘 하고 싶은가? 
/// - 몬스터 처치 시 코인에 해당하는 이미지가 몬스터 위치에서 랜덤하게 퍼졌다가 좌상단 UI로 빠르게 이동하게 하고 싶다. (간략하게 말하면 몬스터 처치 시 골드 획득 하는 것 처럼 보이게)
/// 
/// 누구에 의해 실행되는가?
/// - Monster.cs에서 오브젝트 풀을 통해 스폰 됨 
/// 
/// 어떻게 코드를 짤 것인가.
/// - 
/// </summary>
public class CoinParent : MonoBehaviour
{
    Vector3 _target;
    Camera _cam;
    RectTransform[] _childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField] float _distanceRange, speed;
    private void Awake()
    {
        _cam = Camera.main;
        for(int i = 0; i < _childs.Length; i++) _childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
    }

    public void Init(Vector3 pos)
    {
        _target = pos;

        transform.position = _cam.WorldToScreenPoint(pos);
        for(int i = 0; i < 5; i++) { _childs[i].anchoredPosition = Vector2.zero; }
        transform.parent = Base_Canvas.instance.HolderLayer(0);

        StartCoroutine(Coint_Effect());
    }

    IEnumerator Coint_Effect()
    {
        Vector2[] randomPos = new Vector2[_childs.Length];
        for(int i = 0; i <_childs.Length; i++)
        {
            randomPos[i] = new Vector2(_target.x, _target.y) + Random.insideUnitCircle * Random.Range(-_distanceRange, _distanceRange);
        }
        while (true)
        {
            for(int i = 0; i < _childs.Length; i++) {
                {
                    RectTransform rect = _childs[i];
                    rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, randomPos[i], Time.deltaTime * speed);

                } 
            }

            if (Distance_Boolean(randomPos, 0.5f)) break;
            yield return null;
        }
        yield return new WaitForSeconds(.3f);

        while (true)
        {
            for(int i = 0; i < _childs.Length; i++)
            {
                RectTransform rect = _childs[i];
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.Coin.position, Time.deltaTime * (speed * 20));
            }

            if (Distance_Boolean_World(0.5f))
            {
                Base_Mng.Pool.m_pool_Dictionary["CoinParent"].Return(this.gameObject);
                break;
            }
            yield return null;
        }
    }
    bool Distance_Boolean(Vector2[] end, float range)
    {
        for(int i = 0; i <_childs.Length; i++)
        {
            float distance = Vector2.Distance(_childs[i].anchoredPosition, end[i]);
            if(distance > range)
            {
                return false;
            }
        }
        return true;
    }

    bool Distance_Boolean_World(float range)
    {
        for(int i = 0; i < _childs.Length;i++)
        {
            float distance = Vector2.Distance(_childs[i].position, Base_Canvas.instance.Coin.position);
                if(distance > range)
            {
                return false;
            }
        }
        return true;
    }
}
