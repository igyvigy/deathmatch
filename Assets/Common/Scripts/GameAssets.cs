using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i
    {
        get
        {
            return Singleton<GameAssets>.Instance;
        }
    }
    public Material mBlue;
    public Material mRed;
    public RectTransform pfDamagePopup;
    public HealthBar pfHealthBar;
}
