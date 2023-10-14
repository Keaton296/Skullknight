using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossBar : MonoBehaviour
{
    public StatBar statBar;
    public TMP_Text title;
    public static BossBar Instance;
    private void Awake()
    {
        Instance = this; 
    }
}
