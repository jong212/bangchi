using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Csv_Importer 
{
    public static    List<Dictionary<string,object>> Exp = new List<Dictionary<string,object>>(CSVReader.Read("EXP"));
}
