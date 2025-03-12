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
    /// 1. 몬스터가 스폰될 때 갑자기 나타나는 대신, 작게 시작해 점진적으로 커지도록 부드러운 효과를 주기 위함.
    /// 2. 선형 보간(Lerp): Scale 값을 0에서 1까지 1초 동안 균등하게 증가시키기 위해 사용.
    /// 
    /// percent = current / n에서 n 값을 조절하면 0에서 1이 되는 시간을 설정할 수 있습니다.
    /// n이 1이면 1초 후 percent가 1이 되고,
    /// n이 0.5이면 0.5초 후에 percent가 1이 되어 애니메이션 속도가 빨라집니다.
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
