using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Character_Scriptable _characterData;
    public GameObject TrailObject; 
    public string CharacterName;

    Vector3 startPos;
    Quaternion rot;
    protected override void Start()
    {
        base.Start();

        DataSet(Resources.Load<Character_Scriptable>("Scriptable/" + CharacterName));

        Spawner.m_Players.Add(this);

        Base_Mng.Stage.ReadyEvent += OnReady;
        Base_Mng.Stage.BossEvent += OnBoss;

        startPos = transform.position;
        rot = transform.rotation;
    }
    private void DataSet(Character_Scriptable data)
    {
        _characterData = data;
        _attackRange = data.AttackRange;
        Set_ATKHP();
    }

    public void Set_ATKHP()
    {
        ATK = Base_Mng.Player.Get_Atk(_characterData.Rarity);
        HP = Base_Mng.Player.Get_Hp(_characterData.Rarity);
        
    }
    private void OnReady()
    {
        Debug.Log(startPos);
        transform.position = startPos;
    }

    private void OnBoss()
    {
        AnimationChange("isIdle");
    }
    private void Update()
    {
        // 타겟할 몬스터가 없으면 0 0 0 으로 'Move'
        // 타겟할 몬스터가 없고  0 0 0 이면 'Idle'


    

        if (Base_Mng.Stage.State != Stage_State.Play) return;
            FindClosetTarget(Spawner.m_Monster.ToArray());

        if (m_Target == null)
        {
            float targetPos = Vector3.Distance(transform.position, startPos);
            if (targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimationChange("isMove");
            }
            else
            {
                transform.rotation = rot;
                AnimationChange("isIdle");
            }
        }
        else
        {
            if (m_Target.GetComponent<Character>().isDead) FindClosetTarget(Spawner.m_Monster.ToArray());

            float targetDistance = Vector3.Distance(transform.position, m_Target.position);
            if (targetDistance <= _targetRange && targetDistance > _attackRange && isAttack == false)
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
        // 타겟할 몬스터가 Dead 상태면 죽은 몬스터를 향해 갈 순 없으니까 
        // 일단 현재 활성화 되어있는 몬스터가 있는지 체크하고 없다면 return 
        // 리턴 이유는 어차피 타겟할 몬스터가 없어서 위에서 move 해서 0 0 0가면 됨

        if (Spawner.m_Monster.Count > 0)
        {
        }

   
    }



    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        var goObj = Base_Mng.Pool.Pooling_Obj("Hit_Text").Get((value) =>
        {
            value.GetComponent<HitText>().init(transform.position, dmg, true);
        });
        HP -= dmg;
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObject.SetActive(true);
        Invoke("TrailDisable", 1.0f);
    }

    private void TrailDisable() => TrailObject.SetActive(false);
}
