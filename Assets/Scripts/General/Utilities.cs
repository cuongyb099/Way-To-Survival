using Tech.Logger;
using UnityEngine;

public static class Utilities
{
    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        var fileToLoad = Resources.Load<T>(path);

        if (fileToLoad) return fileToLoad;
        
        LogCommon.Log("Fail To Load Resource");
        
        return null;
    }
}
