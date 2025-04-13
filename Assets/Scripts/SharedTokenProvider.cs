using System;
using UnityEngine;

public static class SharedTokenProvider
{
    private static string _sharedToken = "";
    
    public static void SetToken(string token)
    {
        _sharedToken = token;
        Debug.Log("Token set in SharedTokenProvider: " + (string.IsNullOrEmpty(_sharedToken) ? "empty" : "valid"));
    }
    
    public static string GetToken()
    {
        if (string.IsNullOrEmpty(_sharedToken))
        {
            Debug.LogWarning("Attempting to get token before it's been set");
        }
        return _sharedToken;
    }
}