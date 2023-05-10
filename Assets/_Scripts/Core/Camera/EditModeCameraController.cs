using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditModeCameraController : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera m_Camera;

    #region Movement
    [SerializeField, Header("Movement Settings")]
    private float m_MaxSpeed = 0.0f;
    [SerializeField]
    private float m_Acceleration = 10.0f;
    [SerializeField]
    private float m_Damping = 15.0f;
    [SerializeField]
    private int m_FocusIterations = 10;
    private int m_CurrentIteration = 0;
    private Vector3 m_FocusPosition = Vector3.zero;

    private float m_CurrSpeed = 0.0f;
    [SerializeField]
    private bool m_UseScreenEdge = false;
    [SerializeField, Range(0.0f, 0.2f)]
    private float m_EdgeTolerance = 0.05f;
    #endregion

    #region Zoom
    [SerializeField, Header("Zoom Settings")]
    private float m_StepSize = 2.0f;
    [SerializeField]
    private float m_ZoomDamping = 7.0f;

    private float m_ZoomOrthoSize = 0.0f;
    [SerializeField]
    private float m_MinZoomSize = 5;
    [SerializeField]
    private float m_MaxZoomSize = 8;
    #endregion

    Vector3 m_PositionDelta = Vector3.zero;
    Vector3 m_LastPosition = Vector3.zero;
    Vector3 m_TargetPosition = Vector3.zero;

    Vector3 m_HorizontalVelocity = Vector3.zero;

    object m_CurrTweenID = 0;

    private void OnEnable()
    {
        m_LastPosition = transform.position;
        GameManager.Instance.gameInputController.GetCameraControlsActions().ZoomCamera.performed += ZoomCamera;
        EditModeController.OnFocusObject += OnFocusObjectChanged;

        if (m_Camera)
        {
            m_ZoomOrthoSize = m_Camera.m_Lens.OrthographicSize;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetCameraControlsActions().ZoomCamera.performed -= ZoomCamera;
        EditModeController.OnFocusObject -= OnFocusObjectChanged;
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyboardInput();
        if (m_UseScreenEdge)
        {
            CheckScreenEdgeInput();
        }
        UpdateVelocity();
        UpdatePosition();
        UpdateZoom();
    }

    private void CheckScreenEdgeInput()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseDirection = Vector3.zero;

        if (mousePos.x < m_EdgeTolerance * Screen.width)
        {
            mouseDirection += Vector3.left;
        }
        else if (mousePos.x > (1.0f - m_EdgeTolerance) * Screen.width)
        {
            mouseDirection += Vector3.right;
        }

        if (mousePos.y < m_EdgeTolerance * Screen.height)
        {
            mouseDirection += Vector3.down;
        }
        else if (mousePos.y > (1.0f - m_EdgeTolerance) * Screen.height)
        {
            mouseDirection += Vector3.up;
        }

        m_TargetPosition += mouseDirection.normalized;
    }

    void UpdateVelocity()
    {
        m_HorizontalVelocity = (transform.position - m_LastPosition) / Time.deltaTime;
        m_HorizontalVelocity.z = 0;
        m_LastPosition = transform.position;
    }

    void GetKeyboardInput()
    {
        Vector2 cameraMovementInput = GameManager.Instance.gameInputController.cameraMovementAction.ReadValue<Vector2>();
        Vector3 inputVector = new Vector3(cameraMovementInput.x, cameraMovementInput.y, 0);
        Vector3 inputNorm = inputVector.normalized;

        if (inputNorm.sqrMagnitude > 0.1f)
        {
            m_TargetPosition += inputNorm;
        }
    }

    void UpdatePosition()
    {
        m_PositionDelta = Vector3.zero;
        if (m_TargetPosition.sqrMagnitude > 0.1f)
        {
            m_CurrSpeed = Mathf.Lerp(m_CurrSpeed, m_MaxSpeed, Time.deltaTime * m_Acceleration);
            m_PositionDelta += m_TargetPosition * m_CurrSpeed * Time.deltaTime;
        }
        else
        {
            m_HorizontalVelocity = Vector3.Lerp(m_HorizontalVelocity, Vector3.zero, Time.deltaTime * m_Damping);
            m_PositionDelta += m_HorizontalVelocity * Time.deltaTime;
        }

        if (m_CurrentIteration > 0)
        {
            if ((m_FocusPosition - transform.position).sqrMagnitude > 1.0f)
            {
                m_PositionDelta += (m_FocusPosition - transform.position).normalized * m_CurrSpeed * Time.deltaTime;
            }
            else
            {
                m_PositionDelta += (m_FocusPosition - transform.position) * m_CurrSpeed * Time.deltaTime;
            }
            m_CurrentIteration--;
        }

        transform.position += m_PositionDelta;

        m_TargetPosition = Vector3.zero;
    }

    private void ZoomCamera(InputAction.CallbackContext context)
    {
        float zoomValue = -context.ReadValue<Vector2>().y / 100.0f;

        if (Mathf.Abs(zoomValue) > 0.1f)
        {
            m_ZoomOrthoSize = Mathf.Clamp(m_ZoomOrthoSize + zoomValue * m_StepSize, m_MinZoomSize, m_MaxZoomSize);
        }
    }

    private void UpdateZoom()
    {
        if (!m_Camera)
        {
            Debug.LogError("Camera not setup for zooming");
            return;
        }

        var lensSettings = m_Camera.m_Lens;
        lensSettings.OrthographicSize = Mathf.Lerp(lensSettings.OrthographicSize, m_ZoomOrthoSize, m_ZoomDamping * Time.deltaTime);
        m_Camera.m_Lens = lensSettings;
    }

    private void OnFocusObjectChanged(GameObject obj)
    {
        Vector3 position = obj.transform.position;
        position.z = transform.position.z;
        m_FocusPosition = position;
        m_CurrentIteration = m_FocusIterations;
    }
}
