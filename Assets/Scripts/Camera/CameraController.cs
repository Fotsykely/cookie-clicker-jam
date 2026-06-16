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

    // État initial déterministe, après l'enregistrement des GameEventListener (OnEnable)
    void Start() => Apply(ViewMode.SideView2D);

    private void Toggle() =>
        Apply(viewMode.Current == ViewMode.SideView2D ? ViewMode.TopDown3D : ViewMode.SideView2D);

    private void Apply(ViewMode mode)
    {
        viewMode.Current = mode;                      // 1. état (source de vérité)
        bool is2D = mode == ViewMode.SideView2D;
        sideViewCam.Priority = is2D ? Active : Idle;  // 2. ses propres vcams → le Brain blende
        topDownCam.Priority = is2D ? Idle : Active;
        onViewModeChanged.Raise();                    // 3. notifie le player (et UI/anim plus tard)
    }
}
