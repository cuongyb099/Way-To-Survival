using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using ObjectPool = Tech.Pooling.ObjectPool;

[TaskCategory("AI")]
public class RandomSkinSelector : BaseEnemyBehavior
{
    public SharedBool RandomAllBodyPart;
    private SkinnedMeshRenderer[] _currentSkin;
    private EntitySkinCtrl _skinCtrl;
    public override void OnAwake()
    {
        base.OnAwake();
        _skinCtrl = GetComponent<EntitySkinCtrl>();
        _currentSkin = new SkinnedMeshRenderer[_skinCtrl.BodySkinParts.Length];
    }

    public override void OnStart()
    {
        var skins = _skinCtrl.BodySkinParts;
        if(skins.Length <= 0) return;
        
        var index = 0;
        
        if (RandomAllBodyPart.Value)
        {
            for (int i = 0; i < skins.Length; i++)
            {
                index = Random.Range(0, skins[i].SkinParts.Length);
                if (_currentSkin[i])
                {
                    _currentSkin[i].gameObject.SetActive(false);
                }
                _currentSkin[i] = skins[i].SkinParts[index];
                _currentSkin[i].gameObject.SetActive(true);
            }
            return;
        }
        
        var minLength = skins.Min(x => x.SkinParts.Length);
        index = Random.Range(0, minLength);

        for (int i = 0; i < skins.Length; i++)
        {
            if (_currentSkin[i])
            {
                _currentSkin[i].gameObject.SetActive(false);
            }
            _currentSkin[i] = skins[i].SkinParts[index];
            _currentSkin[i].gameObject.SetActive(true);
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}