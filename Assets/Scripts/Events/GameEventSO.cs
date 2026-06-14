using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "SO/GameEvent")]
public class GameEventSO : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void Register(GameEventListener listener) => listeners.Add(listener);
    public void Unregister(GameEventListener listener) => listeners.Remove(listener);
}
