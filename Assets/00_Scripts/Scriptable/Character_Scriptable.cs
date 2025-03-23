using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable",menuName ="Object/Character",order =int.MaxValue)]
public class Character_Scriptable : ScriptableObject
{
    public string CharacterName;
    public float AttackRange;
    public Rarity Rarity;


}
