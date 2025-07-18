using UnityEngine;

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
        
    }
}
