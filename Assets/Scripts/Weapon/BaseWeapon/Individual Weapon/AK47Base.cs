
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AK47Base :GunBase
{

    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<int, WeaponSkillUnlockSO> SkillUnlock;
    public override void OnInit()
    {
        base.OnInit();

        foreach (var lv in SkillUnlock.Keys)
        {
            if (GunData.WeaponLevel < lv) continue;
            SkillUnlock[lv].ApplySkill(this);
        }
    }

    public void OnDestroy()
    {
        foreach (var lv in SkillUnlock.Keys)
        {
            if (GunData.WeaponLevel < lv) continue;
            SkillUnlock[lv].RemoveSkill(this);
        }
    }

}