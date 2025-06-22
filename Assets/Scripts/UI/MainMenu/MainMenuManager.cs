using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject GameTitle;
    public static MainMenuManager Instance { get; private set; } = null;
    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        SceneManager.LoadSceneAsync(gameObject.scene.name + " UI", LoadSceneMode.Additive);
    }
}
