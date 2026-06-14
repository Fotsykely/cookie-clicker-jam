using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEventSO gameEvent;
    public UnityEvent response;

    void OnEnable() => gameEvent.Register(this);
    void OnDisable() => gameEvent.Unregister(this);

    public void OnEventRaised() => response.Invoke();
}
