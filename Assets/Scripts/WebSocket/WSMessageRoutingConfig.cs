
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a routing entry for WebSocket message handling.
/// </summary>
/// <remarks>
/// This class defines the mapping between a specific message type and the target MonoBehaviour
/// and method to invoke. It is used in message routing configurations to determine how
/// incoming WebSocket messages should be processed.
/// </remarks>
[System.Serializable]
public class WSRoutingEntry
{
    public string messageType;
    public string scriptName;
    public string methodName;
}

/// <summary>
/// Represents a configuration for routing WebSocket messages to their appropriate handlers.
/// </summary>
/// <remarks>
/// This class is used to define and store a set of routing entries which map WebSocket message types
/// to specific methods within target MonoBehaviours. The routing configuration is serialized as a
/// ScriptableObject for easy management within the Unity Editor.
/// </remarks>
[CreateAssetMenu(fileName = "WSRoutingConfig", menuName = "WebSocketManager/WSMessageRoutingConfig")]
public class WSMessageRoutingConfig : ScriptableObject
{
    public List<WSRoutingEntry> entries;
}
