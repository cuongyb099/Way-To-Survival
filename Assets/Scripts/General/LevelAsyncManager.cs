using System.Collections;
using Tech.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;

public class LevelAsyncManager : SingletonPersistent<LevelAsyncManager>
{
    [SerializeField] private CanvasFadeHelper fadeHelper;
    [Header("Loading Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Animator loadingRotating;
    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        loadingRotating.enabled = true;
        fadeHelper.FadeIn(() =>
        {
            
            StartCoroutine(LoadLevelASync(sceneName));
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
        fadeHelper.FadeOut();
        yield return null;
    }
    public void SwitchScene(string scene)
    {
        LoadScene(scene);
    }

    public void SwitchToMap1()
    {
        LoadScene("Map1");
    }
    public void SwitchToMainMenu()
    {
        LoadScene("MainMenu");
    }
}
