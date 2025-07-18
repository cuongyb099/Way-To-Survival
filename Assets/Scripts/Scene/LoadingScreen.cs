using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tech.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingScreen : SingletonPersistent<LoadingScreen>
{
    private static List<Func<UniTask>> _loadingTasks = new List<Func<UniTask>>();
    
    public bool AutoLoadOnStart;
    public float FillTimeEachTask = 0.5f;
    public float WaitTimeToUnload = 0.75f;
    public float WaitTimeOnStart = 0.5f;
    public float CurrentProgress { get; protected set; }
    public Ease FillEase = Ease.Linear;
    
    public UnityEvent<string> OnUpdateTaskTitle;
    public UnityEvent OnStartLoading;
    public UnityEvent OnDone;
    public UnityEvent<float> OnProgressChange;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        ResetScene();
    }

    private void Start()
    {
        if(!AutoLoadOnStart) return;
        
        StartLoading().Forget();
    }

    public async UniTaskVoid StartLoading()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        OnStartLoading?.Invoke();
        await UniTask.WaitForSeconds(WaitTimeOnStart, cancellationToken: cancelToken);
        
        while (_loadingTasks.Count == 0)
        {
            await UniTask.Yield(cancelToken);
        }
        
        float progressEachTask = (float) 1 / _loadingTasks.Count;
        foreach (var task in _loadingTasks)
        {
            SetTaskTitle(task.Method.Name);
            await task();
            await NotifyProgressChange(CurrentProgress + progressEachTask, cancelToken);
            OnUpdateTaskTitle?.Invoke(string.Empty);
        }
        
        await UniTask.Yield(cancelToken);
        
        ResetScene();
        
        OnDone?.Invoke();
        
        SceneManager.UnloadSceneAsync(SceneConstant.Loading);
        
        await UniTask.WaitForSeconds(WaitTimeToUnload, cancellationToken: 
            AddressablesManager.Instance.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }

    protected UniTask NotifyProgressChange(float targetProgress, CancellationToken cancelToken)
    {
        DOVirtual.Float(CurrentProgress, targetProgress, FillTimeEachTask, (progress) =>
        {
            CurrentProgress = progress;
            OnProgressChange?.Invoke(progress);
        }).SetEase(FillEase);

        return UniTask.WaitForSeconds(FillTimeEachTask, cancellationToken: cancelToken);
    }
    
    public static void AddLoadingTask(Func<UniTask> task)
    {
        _loadingTasks.Add(task);
    }
    
    public void SetTaskTitle(string title)
    {
        OnUpdateTaskTitle?.Invoke(title);
    }
    
    private static void ResetScene()
    {
        _loadingTasks.Clear();
    }
}