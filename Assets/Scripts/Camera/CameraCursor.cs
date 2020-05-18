using Cinemachine;
using UnityEngine;

public class CameraCursor : MonoBehaviour
{
    const float PAN_SPEED_FACTOR = 2f;

    [Header("Pan")]
    [SerializeField] float keyboardPanSpeed = 10f;

    [Header("Zoom")]
    [SerializeField] float zoomMin = 0.5f;
    [SerializeField] float zoomMax = 15f;

    bool isDragging = false;
    Vector3 offset = new Vector3();
    Vector3 mouseOriginPoint = new Vector3();
    CinemachineVirtualCamera vCam;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
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
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        Vector3 rotatedTranslation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z) * translation;

        if (rotatedTranslation != Vector3.zero)
            transform.position = ClampPosition(transform.position + rotatedTranslation);
    }

    private void MousePan()
    {
        if (Input.GetMouseButton(1))
        {
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!isDragging)
            {
                isDragging = true;
                mouseOriginPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            isDragging = false;
        }

        if (isDragging)
        {
            transform.position = mouseOriginPoint - offset;
        }
    }
    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (!vCam) GetVirtualCamera();
            float zoom = vCam.m_Lens.OrthographicSize - scroll * vCam.m_Lens.OrthographicSize;
            vCam.m_Lens.OrthographicSize = Mathf.Clamp(zoom, zoomMin, zoomMax);
        }
    }

    private void GetVirtualCamera()
    {
        vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
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

    private Vector3 ClampPosition(Vector3 newPos)
    {
        newPos.x = Mathf.Clamp(newPos.x, 0, 20);
        newPos.y = 0;
        newPos.z = Mathf.Clamp(newPos.z, 0, 20);

        return newPos;
    }
}
