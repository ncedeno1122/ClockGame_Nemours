using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewIconBank", menuName = "ScriptableObjects/IconBank", order = 1)]
public class IconBankSO : ScriptableObject
{
    public Sprite PendulumIcon, HandsIcon, ChimeIcon, CuckooIcon, InvalidIcon;
}
