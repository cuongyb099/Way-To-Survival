using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tech.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class LoadingAsyncManager : SingletonPersistent<LoadingAsyncManager>
{
    [SerializeField] private CanvasFadeHelper fadeHelper;
    [Header("Loading Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Animator loadingRotating;
    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    
    public static LoadingConfig Config = new LoadingConfig();
    public static Action OnLoadDone;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        ResetScene();
    }

    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(SceneManager.GetActiveScene().name == SceneConstant.Loading)
            SwitchToMainMenu();
    }
    
    public void LoadScene(string sceneName)
    {
        AddLoadingTask(()=>LoadLevelASync(sceneName).ToUniTask());
        LoadData();
    }
    public void LoadData()
    {
        loadingScreen.SetActive(true);
        loadingRotating.enabled = true;

        fadeHelper.FadeIn(() =>
        {
            StartLoading();
        });
    }
    
    IEnumerator LoadLevelASync(string sceneName)
    {
        //Load
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }

        yield return null;
    }

    private static List<Func<UniTask>> _loadingTasks = new List<Func<UniTask>>();
    
    public static void AddLoadingTask(Func<UniTask> task)
    {
        if(!_loadingTasks.Contains(task))
            _loadingTasks.Add(task);
    }

    public void ChangeLoadingDescription(string description)
    {
        loadingText.SetText(description);
    }
    


    public async UniTaskVoid StartLoading()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        while (_loadingTasks.Count == 0)
        {
            await UniTask.Yield(cancelToken);
        }
        
        await UniTask.Yield(cancelToken);
        
        //await SetLoadingProgress(Config.BeginProgress, Config.BeginFillTime);

        float progressEachTask = (Config.EndProgress - Config.BeginProgress) / _loadingTasks.Count;
        foreach (var task in _loadingTasks)
        {
            ChangeLoadingDescription(task.Method.Name);
            await task();
            await SetLoadingProgress(loadingSlider.value + progressEachTask, Config.FillTimeEachTask);
        }
        
        await UniTask.Yield(cancelToken);
        
        //await SetLoadingProgress(1f, Config.EndFillTime);
        
        OnLoadDone?.Invoke();
        ResetScene();
        
        fadeHelper.FadeOut(() => {         
            loadingSlider.value = 0f;
            ChangeLoadingDescription(string.Empty);
        });
    }

    protected UniTask SetLoadingProgress(float progress, float duration)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        
        DOVirtual.Float(loadingSlider.value, progress, duration, (curProgress) =>
        {
            loadingSlider.value = curProgress;
        });

        return UniTask.WaitForSeconds(duration, cancellationToken: cancelToken);
    }
    
    private static void ResetScene()
    {
        _loadingTasks.Clear();
        OnLoadDone = null;
        Config.Reset();
    }
    public void SwitchScene(string scene)
    {
        LoadScene(scene);
    }

    public void SwitchToMap1()
    {
        LoadScene(SceneConstant.Map1);
    }
    public void SwitchToMainMenu()
    {
        LoadScene(SceneConstant.MainMenu);
    }
}