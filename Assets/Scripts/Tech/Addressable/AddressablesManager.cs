using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tech.Logger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
	private readonly Dictionary<object, AsyncOperationHandle> _dicAsset = new ();
	public static AddressablesManager Instance { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
        if (Instance) return;
		Instance = new GameObject("Addressables").AddComponent<AddressablesManager>();
		DontDestroyOnLoad(Instance.gameObject);
	}
	
	private void Awake()
	{
		Addressables.InitializeAsync();
	}
	
	public async UniTask<T> LoadAssetAsync<T>(object key, Action onFailed = null, CancellationToken token = default) where T : class
    {
        if (_dicAsset.TryGetValue(key, out var value))
        {
            return value.Result as T;
        }
        try
        {
            AsyncOperationHandle<T> opHandle;
            
            if (key is IEnumerable enumerable)
            {
                opHandle = Addressables.LoadAssetAsync<T>(enumerable);
            }
            else
            {
                opHandle = Addressables.LoadAssetAsync<T>(key);
            }

            await opHandle.ToUniTask(cancellationToken: token);

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _dicAsset.Add(key, opHandle);
                return (T)opHandle.Result;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        LogCommon.LogWarning($"Load Asset Failed: {key}");
        onFailed?.Invoke();
        return null;
    }
    public async UniTask<List<T>> LoadAssetsAsync<T>(object key, Action onFailed = null, CancellationToken token = default)
    {
        if (_dicAsset.TryGetValue(key, out var value))
        {
            return value.Result as List<T>;
        }
        try
        {
            var opHandle = Addressables.LoadAssetsAsync<T>(key, null);

            await opHandle.ToUniTask(cancellationToken: token);
            
            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _dicAsset.Add(key, opHandle);
                return (List<T>)opHandle.Result;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        LogCommon.LogWarning($"Load Asset Failed: {key}");
        onFailed?.Invoke();
        return null;
    }
    public void ReleaseInstance(object key)
    {
        if (!_dicAsset.Remove(key, out var value)) return;
        
        Addressables.ReleaseInstance(value);
    }

    public void Release(object key)
    {
        if (!_dicAsset.Remove(key, out var value)) return;

        Addressables.Release(value);
    }
    
    public bool TryGetAssetInCache<T>(string key, out T result) where T : class
    {
        if (_dicAsset.TryGetValue(key, out var opHandle))
        {
            result = opHandle.Result as T;
            return true;
        }
        result = null;
        return false;
    }

    public async UniTask<GameObject> InstantiateAsync(object key, Transform parent = null, 
        bool worldSpace = true, Action<GameObject> onComplete = null, CancellationToken token = default)
    {
        if (token == CancellationToken.None)
        {
            token = this.GetCancellationTokenOnDestroy();
        }
        
        var opHandle = Addressables.InstantiateAsync(key, parent, worldSpace);
        
        if(onComplete != null){
            opHandle.Completed += (handle) => onComplete?.Invoke(handle.Result);
        }

        await opHandle.ToUniTask(cancellationToken: token);

        _dicAsset[key] = opHandle;
        return opHandle.Result;
    }

    public AsyncOperationHandle<GameObject> OriginInstantiateAsync(object key, Transform parent = null, 
        bool worldSpace = false, CancellationToken token = default)
    {
        if (token == CancellationToken.None)
        {
            token = this.GetCancellationTokenOnDestroy();
        }
        
        var opHandle = Addressables.InstantiateAsync(key, parent, worldSpace);
        _dicAsset[key] = opHandle;
        opHandle.WithCancellation(token);
        return opHandle;
    }
}
