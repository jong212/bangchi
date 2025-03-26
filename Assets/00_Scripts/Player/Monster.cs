using System.Collections;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;

    public float stopDistance = .5f;
    bool isSpawn = false;


    protected override void Start()
    {
        base.Start();
    }
    public void init()
    {
        isDead = false;
        ATK = 10;
        HP = 5;
        _attackRange = .5f;
        _targetRange = Mathf.Infinity;
        StartCoroutine(Spawn_Start());
    }
    private void Update()
    {
        if (isSpawn == false) return;
        if(m_Target == null) FindClosetTarget(Spawner.m_Players.ToArray());


        if(m_Target != null)
        {
            float targetDistance = Vector3.Distance(transform.position, m_Target.position);

            if (targetDistance > _attackRange && isAttack == false)
            {
                AnimationChange("isMove");
                transform.LookAt(m_Target);
                transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
            }
            else if (targetDistance <= _attackRange && isAttack == false)
            {
                isAttack = true;
                transform.LookAt(m_Target);
                AnimationChange("isAttack");
                Invoke("InitAttack", 1.0f);
            }
        } 

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
    public override void GetDamage(double dmg)
    {
        if (isDead) return;

        bool critical = Critical(ref dmg);

        Base_Mng.Pool.Pooling_Obj("Hit_Text").Get((value) => {
            value.GetComponent<HitText>().init(transform.position, dmg, false,critical);
        });
        HP -= dmg;
        if(HP <= 0)
        {
            isDead = true;
            DeadEvent();
            
        }
    }
    public void DeadEvent()
    {
        Base_Mng.Stage.Count++;
        MainUI.instance.MonsterSliderCount();
        Spawner.m_Monster.Remove(this);
        var smokeObj = Base_Mng.Pool.Pooling_Obj("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, .5f, transform.position.z);
            Base_Mng.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
        });

        Base_Mng.Pool.Pooling_Obj("CoinParent").Get((value) =>
        {
            value.GetComponent<CoinParent>().Init(transform.position);
        });

        for (int i = 0; i < 3; i++)
        {
            Base_Mng.Pool.Pooling_Obj("Item_Obj").Get((value) =>
            {
                value.GetComponent<Item_Obj>().Init(transform.position);
            });
        }
        Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
    }
    private  bool Critical(ref double dmg)
    {
        float criticalPercentage = Random.Range(0.0f, 100.0f);
        if (criticalPercentage <= Base_Mng.Player.CriticalPercent)
        {
            dmg *= Base_Mng.Player.CriticalDamage / 100;
            return true;
        }
        else
        {
            return false;
        }
    }
}
