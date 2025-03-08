using System;
using UnityEngine;

namespace Essential
{
    public static class Log
    {
        public static void Info(string message, object context = null)
        {
            if (context != null)
            {
                Debug.Log($"{context.GetType().Name}: {message}");
                return;
            }
            
            Debug.Log(message);
        }

        public static void Info(string message, Color color, object context = null)
        {
            if (context != null)
            {
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{context.GetType().Name}: {message}</color>");
                return;
            }
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>" + message + "</color>");
        }

        public static void Error(string message, object context = null)
        {
            if (context != null)
            {
                Debug.LogError($"{context}: {message}");
                return;
            }
            
            Debug.LogError(message);
        }

        public static void Warning(string message, object context = null)
        {
            if (context != null)
            {
                Debug.LogWarning($"{context}: {message}");
                return;
            }
            
            Debug.LogWarning(message);
        }

        public static void Exception(Exception e)
        {
            Debug.LogException(e);
        }
    }
}