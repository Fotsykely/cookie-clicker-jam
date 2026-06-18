using UnityEngine;

public enum ViewMode { SideView2D, TopDown3D }

[CreateAssetMenu(fileName = "ViewMode", menuName = "SO/ViewMode")]
public class ViewModeSO : ScriptableObject
{
    public ViewMode Current;
}
