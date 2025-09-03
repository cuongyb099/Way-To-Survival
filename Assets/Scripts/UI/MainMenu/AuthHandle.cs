using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Tech.Singleton;
using UnityEngine.Networking;

public class AuthHandle : Singleton<AuthHandle>
{
    private string GoogleAPI = "1023234686500-h59gbrfnhfrcu5jjbjenrprfogg1as3h.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    Firebase.Auth.FirebaseAuth auth;
    public FirebaseUser User { get; private set; }

    private string imageUrl;

    protected override void Awake()
    {
        base.Awake();
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = GoogleAPI,
            RequestEmail = true
        };
    }

    private void Start()
    {
        InitFirebase();
    }

    void InitFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginGoogle()
    {
        
        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
                MessagePopup.Instance.ShowMessage("Login Canceled");
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
                MessagePopup.Instance.ShowMessage("Login Failed " + task.Exception);
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        MessagePopup.Instance.ShowMessage("Login Faulted In Auth " + authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        User = auth.CurrentUser;
                        PlayerDataPersistent.Instance.UserID = User.UserId;
                        PlayerDataPersistent.Instance.Load();
                        MessagePopup.Instance.ShowMessage($"Login Success as {User.DisplayName}");

                        //StartCoroutine(LoadImage(CheckImageUrl(User.PhotoUrl.ToString())));
                    }
                });
            }
        });
    }
    
    public void LogOutGoogle()
    {
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        User = null;
        PlayerDataPersistent.Instance.UserID = null;
    }
    
    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
    }

    // IEnumerator LoadImage(string imageUri)
    // {
    //     UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
    //     yield return www.SendWebRequest();
    //
    //     if (www.result == UnityWebRequest.Result.Success)
    //     {
    //         Texture2D texture = DownloadHandlerTexture.GetContent(www);
    //         // Use the loaded texture here
    //         Debug.Log("Image loaded successfully");
    //         //PlayerDataPersistent.Instance.PlayerData.ImgSprite  = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
    //     }
    //     else
    //     {
    //         Debug.Log("Error loading image: " + www.error);
    //     }
    // }
    //Face
    public void LoginFace()
    {
        
    }
    public void LogOutFace()
    {
        
    }
    //Anonymous
    public void LoginAnonymous()
    {
        
    }
    public void LogOutAnonymous()
    {
        
    }
    
}