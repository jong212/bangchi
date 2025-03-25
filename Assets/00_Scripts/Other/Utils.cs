using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.U2D;

public class Utils : MonoBehaviour
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Sprite Get_Atlas(string temp)
    {
        return atlas.GetSprite(temp);
    }
    public static string String_color_Rarity(Rarity rare)
    {
     switch (rare)
        {
            case Rarity.Common: return "<color=#FFFFFF>";
            case Rarity.UnCommon: return "<color=#00FF00>";
            case Rarity.Rare: return "<color=#00D8FF>";
            case Rarity.Hero: return "<color=#B750C3>";
            case Rarity.Legendary: return "<color=#FF9000>";
        }
        return "<color=#FFFFFF>";
    }
}
