using System.Collections;
using System.Collections.Generic;
using Tech.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using Tech.Singleton;
using UnityEditor;
using Random = UnityEngine.Random;

public class DamagePopUpGenerator : Singleton<DamagePopUpGenerator>
{
    private GameObject PopUpPrefab;
    private GameObject CritPopUpPrefab;
    private GameObject CashPopUpPrefab;
    protected override void Awake()
    {
	    base.Awake();
	    LoadComponentAsync();
    }

    private async void LoadComponentAsync()
    {
	    PopUpPrefab = 
		    await AddressablesManager.Instance.LoadAssetAsync<GameObject>("Assets/Prefab/UI/Popup/NonCritPopup.prefab");
	    CritPopUpPrefab =
		    await AddressablesManager.Instance.LoadAssetAsync<GameObject>(
			    "Assets/Prefab/UI/Popup/CritDamagePopup.prefab");
	    CashPopUpPrefab =
		    await AddressablesManager.Instance.LoadAssetAsync<GameObject>("Assets/Prefab/UI/Popup/CashPopup.prefab");
    }

    public void CreateDamagePopUp(Vector3 position,string text, bool crit = false)
    {
	    GameObject obj = ObjectPool.Instance.SpawnObject(crit?CritPopUpPrefab:PopUpPrefab, position+Random.insideUnitSphere, Quaternion.identity,PoolType.UIPopUp);
	    DamagePopUpController popup = obj.GetComponent<DamagePopUpController>();

        popup.Text.text = text;
    }
	public void CreateDamagePopUp(Vector3 position,int damage, bool crit = false)
	{
		GameObject obj = ObjectPool.Instance.SpawnObject(crit?CritPopUpPrefab:PopUpPrefab, position+Random.insideUnitSphere, Quaternion.identity,PoolType.UIPopUp);
		DamagePopUpController popup = obj.GetComponent<DamagePopUpController>();

		popup.Text.text = damage.ToString();
	}
	public void CreateDamagePopUp(Vector3 position, DamageInfo damageInfo)
	{
		GameObject obj = ObjectPool.Instance.SpawnObject(damageInfo.IsCrit?CritPopUpPrefab:PopUpPrefab, position+Random.insideUnitSphere, Quaternion.identity,PoolType.UIPopUp);
		DamagePopUpController popup = obj.GetComponent<DamagePopUpController>();
            
		popup.Text.text = ((int)damageInfo.Damage).ToString();
	}
	public void CreateCashPopUp(Vector3 position, string text)
	{
		GameObject obj = ObjectPool.Instance.SpawnObject(CashPopUpPrefab, position+Random.insideUnitSphere, Quaternion.identity,PoolType.UIPopUp);
		DamagePopUpController popup = obj.GetComponent<DamagePopUpController>();

		popup.Text.text = text;

	}
}
