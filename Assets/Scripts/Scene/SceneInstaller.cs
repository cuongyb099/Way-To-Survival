using System.Collections.Generic;
using System.Threading;
using BehaviorDesigner.Runtime.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneInstaller
{
    public static string LoadingPrefabAddress = "Loading";
    
    public static void InstallMainMenu(params string[] unloadScenes)
    {
        LoadSceneWithLoadingScreen(SceneConstant.MainMenu, SceneConstant.MainMenu + " UI");
    }
    
    public static void InstallEndlessMode(params string[] unloadScenes)
    {
        LoadSceneWithLoadingScreen(SceneConstant.Map1, SceneConstant.EndlessMode + " UI");
    }

    public static void LoadSceneWithLoadingScreen(params string[] scenes)
    {
        LoadingScreen.AddLoadingTask(async () =>
        {
            var task = new List<UniTask>();

            for (var i = 0; i < scenes.Length; i++)
            {
                task.Add(SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive).ToUniTask());
            }

            await UniTask.WhenAll(task); 
        });
        
        SceneManager.LoadSceneAsync(SceneConstant.Loading);
    }
}