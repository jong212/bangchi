using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// 1. ���� ȭ�� UI�� ���
// 2. �÷��̾��� ����, �������� ȭ�鿡 ǥ�� �ϴ� View �̴�.
// 3. Fade In Out �����ϵ��� �ʵ� ���� 
public class MainUI : MonoBehaviour
{
    public static MainUI instance = null;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI allatkText;

    [SerializeField] private Image fade;
    [SerializeField] private float fadeDutration;

    [SerializeField] Slider monsterSlider;
    [SerializeField] TextMeshProUGUI monsterValueText;

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
    public void RegisterStageEvents()
    {
        Base_Mng.Stage.ReadyEvent += () => FadeInOut(true);
    }

    public void MonsterSliderCount()
    {
        float value = (float)Base_Mng.Stage.Count / (float)Base_Mng.Stage.MaxCount;

        if(value >= 1.0f)
        {
            value = 1.0f;
            if(Base_Mng.Stage.State != Stage_State.Boss)
                Base_Mng.Stage.State_Change(Stage_State.Boss);
            
        }

        monsterSlider.value = value;
        monsterValueText.text = string.Format("{0:0.0}", value * 100) + "%";
    }

    /// <summary>
    /// 1. true�� FadeOut 
    /// 2. true�� ��ü ���°� false�� UI���� ȭ�鸸 ����
    /// 3. �ݹ����� FadeOut ����
    /// </summary>    
    public void FadeInOut(bool FadeInOut,bool sibiling = false, Action action = null)
    {
        if (!sibiling)
        {
            fade.transform.parent = this.transform;
            fade.transform.SetSiblingIndex(0);
        } else
        {
            fade.transform.parent = Base_Canvas.instance.transform;
            fade.transform.SetAsLastSibling();
        }
        StartCoroutine(FadeInOut_Coroutine(FadeInOut, action));
    }
    IEnumerator FadeInOut_Coroutine(bool FadeInOut, Action action= null)
    {

        if(FadeInOut == false)
        {
            fade.raycastTarget = true;
        }
        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInOut ? 1.0f : 0.0f;
        float end = FadeInOut ? 0.0f : 1.0f;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / fadeDutration;
            float LerpPos = Mathf.Lerp(start, end, percent);
            fade.color = new Color(0,0,0,LerpPos);
            yield return null;
        }

        if(action != null) action?.Invoke();
        fade.raycastTarget = false;
    }

    public void TextCheck()
    {
        levelText.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
        allatkText.text = StringMethod.ToCurrencyString(Base_Mng.Player.AllCombatPower());
    }
}
