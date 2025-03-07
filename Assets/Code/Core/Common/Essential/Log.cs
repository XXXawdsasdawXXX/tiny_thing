using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Essential
{
    public static class Log
    {
        public static void Info(string message, object context = null)
        {
            if (context is Object unityObject)
            {
                Debug.Log(message, unityObject);
                return;
            }
            
            Debug.Log(message);
        }

        public static void Info(string message, Color color, object context = null)
        {
            if (context is Object unityObject)
            {
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>" + message + "</color>", unityObject);
                return;
            }
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>" + message + "</color>");
        }

        public static void Error(string message, object context = null)
        {
            if (context is Object unityObject)
            {
                Debug.LogError(message, unityObject);
            }
            
            Debug.LogError(message);
        }

        public static void Warning(string message, object context = null)
        {
            if (context is Object unityObject)
            {
                Debug.LogWarning(message, unityObject);
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