using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ViewModeSO viewMode;
    [SerializeField] private GameEventSO onViewModeChanged;
    [SerializeField] private CinemachineCamera sideViewCam;
    [SerializeField] private CinemachineCamera topDownCam;

    private InputSystem_Actions inputActions;
    private const int Active = 20;
    private const int Idle = 10;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.ToggleView.performed += _ => Toggle();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start() => Apply(ViewMode.SideView2D);

    private void Toggle() =>
        Apply(viewMode.Current == ViewMode.SideView2D ? ViewMode.TopDown3D : ViewMode.SideView2D);

    private void Apply(ViewMode mode)
    {
        viewMode.Current = mode;  
        bool is2D = mode == ViewMode.SideView2D;
        sideViewCam.Priority = is2D ? Active : Idle; 
        topDownCam.Priority = is2D ? Idle : Active;
        onViewModeChanged.Raise();                   
        // Debug.Log($"[CameraController] Vue → {mode}"); 
    }
}
