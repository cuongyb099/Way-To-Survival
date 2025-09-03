
using UnityEngine;

[CreateAssetMenu(fileName = "AddBuffSkillSO", menuName = "Buff/WeaponUpgrade/new AddBuffSkillSO")]
public class AddBuffSkillSO : WeaponSkillUnlockSO
{
    [field:SerializeField] public BaseBuffSO Buff { get; private set; }
    public override void ApplySkill(WeaponBase weapon)
    {
        Buff.AddStatusEffect(GameManager.Instance.Player.Stats);
    }

    public override void RemoveSkill(WeaponBase weapon)
    {
        GameManager.Instance.Player.Stats.RemoveEffect(Buff);
    }
}
