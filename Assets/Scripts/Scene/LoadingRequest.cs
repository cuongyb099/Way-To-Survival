using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingRequest : MonoBehaviour
{
    public List<string> Scenes;
    public UnityEvent OnBeforeLoading;
    
    public void StartLoading()
    {
        OnBeforeLoading.Invoke();
        SceneManager.LoadSceneAsync(SceneConstant.Loading);
        
        LoadingScreen.AddLoadingTask(async () =>
        {
            var task = new List<UniTask>();
            
            for (var i = 0; i < Scenes.Count; i++)
            {
                task.Add(SceneManager.LoadSceneAsync(Scenes[i], LoadSceneMode.Additive).ToUniTask());
            }

            await UniTask.WhenAll(task);
        });    
    }
}