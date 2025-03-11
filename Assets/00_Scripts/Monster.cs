using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : Character
{
    public float m_Speed;

    public float stopDistance = .5f;
    bool isSpawn = false;


    public void init()
    {
        StartCoroutine(Spawn_Start());
    }
    /// <summary>
    /// 1. ���Ͱ� ������ �� ���ڱ� ��Ÿ���� ���, �۰� ������ ���������� Ŀ������ �ε巯�� ȿ���� �ֱ� ����.
    /// 2. ���� ����(Lerp): Scale ���� 0���� 1���� 1�� ���� �յ��ϰ� ������Ű�� ���� ���.
    /// 
    /// percent = current / n���� n ���� �����ϸ� 0���� 1�� �Ǵ� �ð��� ������ �� �ֽ��ϴ�.
    /// n�� 1�̸� 1�� �� percent�� 1�� �ǰ�,
    /// n�� 0.5�̸� 0.5�� �Ŀ� percent�� 1�� �Ǿ� �ִϸ��̼� �ӵ��� �������ϴ�.
    /// </summary>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;

        while (percent < 1)
        {
            current += Time.deltaTime;             
            percent = current / 0.2f;
            
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpPos,LerpPos,LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(.3f);
        isSpawn = true;

    }
    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (isSpawn == false) return;

        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);

        if(targetDistance <= stopDistance)
        {
            AnimationChange("isIdle");
        }else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimationChange("isMove");
        }

    }
    
}
