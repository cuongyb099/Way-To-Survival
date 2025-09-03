
using UnityEngine;
using UnityEngine.Localization;

public abstract class WeaponSkillUnlockSO: ScriptableObject
{
    [field: SerializeField] public LocalizedString SkillName;
    [field: SerializeField] public LocalizedString SkillDescription;
    public abstract void ApplySkill(WeaponBase weapon);
    public abstract void RemoveSkill(WeaponBase weapon);
}
