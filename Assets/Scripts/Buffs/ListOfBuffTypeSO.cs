using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "SpecificListSO", menuName = "Item/Buff/new EffectsListSO")]
public class ListOfBuffTypeSO : ScriptableObject
{
    public List<BaseBuffSO> Buffs;
    public BaseBuffSO ChooseRandomBuff()
    {
        return Buffs[Random.Range(0, Buffs.Count)];
	}
}
