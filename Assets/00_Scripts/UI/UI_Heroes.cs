using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heroes : UI_Base
{
    public Transform Content;
    public GameObject Part;

    Dictionary<string, Character_Scriptable> alignCharacter = new Dictionary<string, Character_Scriptable>();



    public override bool Init()
    {
        MainUI.instance.FadeInOut(true, true, null);
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");
        for (int i = 0; i < Data.Length; i++)
        {
            alignCharacter.Add(Data[i].CharacterName, Data[i]);
        }

        var sort_dictionary = alignCharacter.OrderBy(x => x.Value.Rarity);

        foreach(var data in sort_dictionary)
        {
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            go.Initialize(data.Value);
        }
        return base.Init();
    }
    public override void DisableObj()
    {
        MainUI.instance.FadeInOut(false, true, () =>
        {
            MainUI.instance.FadeInOut(true, false, null);
            base.DisableObj();

        });
    }
}
