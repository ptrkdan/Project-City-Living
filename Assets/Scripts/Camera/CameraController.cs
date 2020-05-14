using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float PAN_SPEED_FACTOR = 2f;

    [Header("Pan")]
    [SerializeField] float keyboardPanSpeed = 10f;
    [SerializeField] float positionLerpTime = 0.2f;

    [Header("Rotation")]
    [SerializeField] float rotationLerpTime = 0.1f;

    [Header("Zoom")]
    [SerializeField] float zoomMin = 0.5f;
    [SerializeField] float zoomMax = 15f;

    CameraState _targetCameraState = new CameraState();
    CameraState _interpolatingCameraState = new CameraState();

    bool _isDragging = false;
    Vector3 offset = new Vector3();
    Vector3 mouseOriginPoint = new Vector3();

    private void OnEnable()
    {
        _targetCameraState.SetFromTransform(transform);
        _interpolatingCameraState.SetFromTransform(transform);
    }

    private void LateUpdate()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        KeyboardPan();
        MousePan();
        Zoom();
    }

    private void KeyboardPan()
    {
        Vector3 direction = GetInputDirection();
        Vector3 translation = direction * keyboardPanSpeed * PAN_SPEED_FACTOR * Time.deltaTime;

        _targetCameraState.Translate(translation);

        var positionLerpPercent = 1f - Mathf.Exp(Mathf.Log(1f - 0.99f) / positionLerpTime * Time.deltaTime);
        var rotationLerpPercent = 1f - Mathf.Exp(Mathf.Log(1f - 0.99f) / rotationLerpTime * Time.deltaTime);
        _interpolatingCameraState.LerpTowards(_targetCameraState, positionLerpPercent, rotationLerpPercent);
        _interpolatingCameraState.UpdateTransform(transform);
    }

    private void MousePan()
    {
        if (Input.GetMouseButton(1))
        {
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _targetCameraState.Transform;
            if (!_isDragging)
            {
                _isDragging = true;
                mouseOriginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector3 newPos = mouseOriginPoint - offset;
            _targetCameraState.SetPosition(newPos);
            var positionLerpPercent = 1f - Mathf.Exp(Mathf.Log(1f - 0.99f) / positionLerpTime * Time.deltaTime);
            var rotationLerpPercent = 1f - Mathf.Exp(Mathf.Log(1f - 0.99f) / rotationLerpTime * Time.deltaTime);
            _interpolatingCameraState.LerpTowards(_targetCameraState, positionLerpPercent, rotationLerpPercent);
            _interpolatingCameraState.UpdateTransform(transform);
        }
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoom = Camera.main.orthographicSize - scroll * Camera.main.orthographicSize;
        Camera.main.orthographicSize = Mathf.Clamp(zoom, zoomMin, zoomMax);
    }

    private Vector3 GetInputDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        return direction;
    }
}
