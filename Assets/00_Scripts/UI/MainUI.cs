using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TextCheck();
    }

    [SerializeField] TextMeshProUGUI levelText;

    public void TextCheck()
    {
        levelText.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
    }
}
