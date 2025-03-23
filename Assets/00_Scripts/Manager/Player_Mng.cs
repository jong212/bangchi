using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///  �÷��̾� �����͸� �����ϴ� Model ���� ��ũ��Ʈ.


public class Player_Mng 
{
    public int Level;
    public double Exp;
    public double Atk = 10;
    public double Hp = 50;
    public float CriticalPercent = 20.0f;
    public double CriticalDamage = 147.0d;
    // ����ġ �� ���� �� �޼���
    public void ExpUp()
    {
        // ���� ������ �ش��ϴ� ����ġ�� ������ += �ϰ�
        Exp += float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString());
        Atk += Next_Atk();
        Hp += Next_Hp();

        //  ���� ���� ����ġ�� ��ǥ����ġ�� ���ų� ũ�ٸ� LevelUp
        if(Exp >= float.Parse(Csv_Importer.Exp[Level]["EXP"].ToString()))
        {
            Level++;
            MainUI.instance.TextCheck();
        }
        for(int i = 0; i < Spawner.m_Players.Count; i++)
        {
            Spawner.m_Players[i].Set_ATKHP();
        }

    }

    // ����ġ �ۼ�Ʈ �޼���
    public float Exp_Percentage()
    {
        float exp = float.Parse(Csv_Importer.Exp[Level]["EXP"].ToString());//49 ��ǥ ����ġ
        double myExp = Exp;                                              //15 ���� ���� ����ġ
        if(Level >= 1)
        {
            exp -= float.Parse(Csv_Importer.Exp[Level - 1]["EXP"].ToString()); // 34 >>> LEVEL -1�� ���� EXP�� �׷��� 15�̰� �� ���� EXP���� ����34��
            myExp -= float.Parse(Csv_Importer.Exp[Level - 1]["EXP"].ToString()); // 0 >>> lEVEL -1�� ���� EXP �� �׷��� 15�̰� �� ���� ���� ���� ����ġ 15�ϱ� 0 ��
        }
        return (float)myExp / exp;
    }
    public float Next_Exp()
    {
        float exp = float.Parse(Csv_Importer.Exp[Level]["EXP"].ToString());
        float myExp = float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString());

        if(Level >= 1)
        {
            exp -= float.Parse(Csv_Importer.Exp[Level - 1]["EXP"].ToString());
        }
        return (myExp / exp) * 100.0f;
    }
    public double Next_Atk()
    {
        float a = float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
        Debug.Log(a);
        return a;
    }

    public double Next_Hp()
    {
        return float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
    }

    public double Get_Atk(Rarity rarity)
    {
        return Atk * ((int)rarity + 1);
    }
    public double Get_Hp(Rarity rarity)
    {
        return Hp * ((int)rarity + 1);
    }

    // ������
        //���Ŀ��� ���� �� ���� �͵��� �� + ���ٰ��� 
    public double AllCombatPower()
    {
        return Atk + Hp;
    }
}