using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private ICameraFocusable defaultFocus;

    [Inject]
    private void Construct(ICameraFocusable defaultFocus)
    {
        this.defaultFocus = defaultFocus;
    }

    private void Start()
    {
        FocusTo(defaultFocus, false);
    }
    
    public void FocusTo(ICameraFocusable focus, bool useSmooth = true)
    {
        if (focus == null) return;

        virtualCamera.m_Follow = focus.transform;
        if (!useSmooth) virtualCamera.transform.position = focus.transform.position;
    }
}

public interface ICameraFocusable
{
    Transform transform { get; }
}