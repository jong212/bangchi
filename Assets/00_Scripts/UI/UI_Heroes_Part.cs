using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Heroes_Part : MonoBehaviour
{
    [SerializeField] private Image slider,characterImage,rarityImage;
    [SerializeField] private TextMeshProUGUI levelText, count;

    public void Initialize(Character_Scriptable data)
    {
        rarityImage.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        characterImage.sprite = Utils.Get_Atlas(data.CharacterName);
        characterImage.SetNativeSize();
        RectTransform rect = characterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);
    }
}
