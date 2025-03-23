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
        startPos = transform.position;
        rot = transform.rotation;
    }
    /*    protected void FindClosetTarget<T>(T[] targets) where T : Component*/
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
    private void Update()
    {
        // Ÿ���� ���Ͱ� ������ 0 0 0 ���� 'Move'
        // Ÿ���� ���Ͱ� ����  0 0 0 �̸� 'Idle'
        if(m_Target == null)
        {
            FindClosetTarget(Spawner.m_Monster.ToArray());

            float targetPos = Vector3.Distance(transform.position, startPos);
            if(targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position,startPos,Time.deltaTime);
                transform.LookAt(startPos);
                AnimationChange("isMove");
            } else
            {
                transform.rotation = rot;
                AnimationChange("isIdle");
            }

            return;
        }
        // Ÿ���� ���Ͱ� Dead ���¸� ���� ���͸� ���� �� �� �����ϱ� 
        // �ϴ� ���� Ȱ��ȭ �Ǿ��ִ� ���Ͱ� �ִ��� üũ�ϰ� ���ٸ� return 
        // ���� ������ ������ Ÿ���� ���Ͱ� ��� ������ move �ؼ� 0 0 0���� ��
        if (m_Target.GetComponent<Character>().isDead) FindClosetTarget(Spawner.m_Monster.ToArray());
        if (m_Target == null) return;
            float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        if(targetDistance <= _targetRange && targetDistance > _attackRange && isAttack == false)
        {
            AnimationChange("isMove");
            transform.LookAt(m_Target);
            transform.position = Vector3.MoveTowards(transform.position,m_Target.position,Time.deltaTime);
        } else if (targetDistance <= _attackRange && isAttack == false)
        {
            isAttack = true;
            transform.LookAt(m_Target);
            AnimationChange("isAttack");
            Invoke("InitAttack", 1.0f);
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
