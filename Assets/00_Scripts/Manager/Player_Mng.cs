using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///  플레이어 데이터를 관리하는 Model 같은 스크립트.


public class Player_Mng 
{
    public int Level;
    public double Exp;
    public double Atk = 10;
    public double Hp = 50;
    public float CriticalPercent = 20.0f;
    public double CriticalDamage = 147.0d;
    // 경험치 및 레벨 업 메서드
    public void ExpUp()
    {
        // 현재 레벨에 해당하는 경험치를 가져와 += 하고
        Exp += float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString());
        Atk += Next_Atk();
        Hp += Next_Hp();

        //  만약 현재 경험치가 목표경험치와 같거나 크다면 LevelUp
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

    // 경험치 퍼센트 메서드
    public float Exp_Percentage()
    {
        float exp = float.Parse(Csv_Importer.Exp[Level]["EXP"].ToString());//49 목표 경험치
        double myExp = Exp;                                              //15 현재 누적 경험치
        if(Level >= 1)
        {
            exp -= float.Parse(Csv_Importer.Exp[Level - 1]["EXP"].ToString()); // 34 >>> LEVEL -1이 이전 EXP임 그래서 15이고 이 값을 EXP에서 뺴면34임
            myExp -= float.Parse(Csv_Importer.Exp[Level - 1]["EXP"].ToString()); // 0 >>> lEVEL -1이 이전 EXP 임 그랫허 15이고 이 값을 현재 누적 경험치 15니까 0 임
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

    // 전투력
        //추후에는 유물 등 여러 것들을 다 + 해줄것임 
    public double AllCombatPower()
    {
        return Atk + Hp;
    }
}