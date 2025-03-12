using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : Character
{
    public float m_Speed;

    public float stopDistance = .5f;
    bool isSpawn = false;


    protected override void Start()
    {
        base.Start();
        HP = 5;
    }
    public void init()
    {
        isDead = false;
        HP = 5;
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
    public void GetDamage(double dmg)
    {
        if (isDead) return;
        Base_Mng.Pool.Pooling_Obj("Hit_Text").Get((value) => {
            value.GetComponent<HitText>().init(transform.position, dmg, false);
        });
        HP -= dmg;
        if(HP <= 0)
        {
            isDead = true;
            Spawner.m_Monster.Remove(this);
            var smokeObj = Base_Mng.Pool.Pooling_Obj("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x,.5f,transform.position.z);
                Base_Mng.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
            });
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
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
