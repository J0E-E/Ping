using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// The WSMessageRoutingManager class is responsible for routing WebSocket messages to their corresponding handlers
/// based on the message type. It facilitates message decoding and dispatching within the application.
/// </summary>
public class WSMessageRoutingManager : Manager
{
    // Dictionary to store script targets for handlers. 
    private Dictionary<string, MonoBehaviour> _handlerScripts = new();
    
    // Dictionary to store message handlers generated from config
    private Dictionary<string, Action<string>> _handlerActions = new();

    // ScriptableObject to facilitate modular configuration.
    [SerializeField] private WSMessageRoutingConfig routingConfig;

    public void InitializeHandlers()
    {
        foreach (var entry in routingConfig.entries)
        {
            // scriptName and methodName must be defined.
            if (entry.scriptName == null || string.IsNullOrEmpty(entry.methodName)) continue;

            if (!_handlerScripts.TryGetValue(entry.scriptName, out var script))
            {
                Type scriptType = Type.GetType(entry.scriptName);
                if (scriptType == null)
                {
                    Debug.LogWarning($"Script type '{entry.scriptName}' not found.");
                    continue;
                }
                
                MonoBehaviour foundScript = FindFirstObjectByType(scriptType) as MonoBehaviour;
                if (foundScript == null)
                {
                    Debug.LogWarning($"Instance of '{entry.scriptName}' not found in scene.");
                    continue;
                }

                _handlerScripts[entry.scriptName] = foundScript;
                script = foundScript;
            }

            // target inherits from MonoBehavior, methodName should match a method in that script.
            MethodInfo method = script.GetType().GetMethod(entry.methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            // if method not found or if the message parameter is missing or of wrong type, do not continue. 
            if (method == null || method.GetParameters().Length != 1 ||
                method.GetParameters()[0].ParameterType != typeof(string))
            {
                Debug.LogWarning($"Invalid handler for '{entry.messageType}': {entry.methodName}");
                continue;
            }
            
            // Create delegate action from the found method.
            Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), script, method);
            _handlerActions[entry.messageType] = action;
            Debug.Log($"WSMessageRoutingManager - Initialized: {entry.scriptName}.{entry.methodName}");
        }
    }

    public void RouteMessage(byte[] bytes)
    {
        // get the BaseMessage from the message. This helps understand which message Type it is for routing. 
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        var baseMessage = JsonUtility.FromJson<BaseMessage>(message);

        // find the handler of that type and invoke the method for it. 
        if (_handlerActions.TryGetValue(baseMessage.type, out Action<string> handler))
        {
            handler.Invoke(message);
        }
        else
        {
            Debug.LogWarning($"No handler found for message type '{baseMessage.type}'");
        }
    }
}
