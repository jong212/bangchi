using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �� ��ũ��Ʈ�� �� �ϰ� ������? 
/// - ���� óġ �� ���ο� �ش��ϴ� �̹����� ���� ��ġ���� �����ϰ� �����ٰ� �»�� UI�� ������ �̵��ϰ� �ϰ� �ʹ�. (�����ϰ� ���ϸ� ���� óġ �� ��� ȹ�� �ϴ� �� ó�� ���̰�)
/// 
/// ������ ���� ����Ǵ°�?
/// - Monster.cs���� ������Ʈ Ǯ�� ���� ���� �� 
/// 
/// ��� �ڵ带 © ���ΰ�.
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
