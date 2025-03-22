using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mng 
{
    public int Level;
    public double Exp;
    public double Atk = 10;
    public double hp = 50;

    /// <summary>
    /// 경헙치 Up , Level Up
    /// </summary>
    public void ExpUp()
    {
        // 현재 레벨에 해당하는 경험치를 가져와 += 하고
        Exp += float.Parse(Csv_Importer.Exp[Level]["Get_EXP"].ToString());

        //  만약 현재 경험치가 목표경험치와 같거나 크다면 LevelUp
        if(Exp >= float.Parse(Csv_Importer.Exp[Level]["EXP"].ToString()))
        {
            Level++;
            MainUI.instance.TextCheck();
        }

    }

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
}