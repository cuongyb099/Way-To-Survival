using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class IntroScene : MonoBehaviour
{
    [SerializeField] protected Animation introAnimation;
    public UnityEvent OnIntroEnd;
    
    private void Start()
    {
        LoadMainMenu().Forget();
    }

    private async UniTaskVoid LoadMainMenu()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntilValueChanged(introAnimation, anim => anim.isPlaying == false, cancellationToken: cancelToken);
        OnIntroEnd?.Invoke();
    }
}