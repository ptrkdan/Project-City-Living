using UnityEngine;

public class CameraState
{
    public float yaw;
    public float pitch;
    public float roll;
    public float x;
    public float y;
    public float z;

    public Vector3 Transform => new Vector3(x, y, z);

    public void SetFromTransform(Transform t)
    {
        pitch = t.eulerAngles.x;
        yaw = t.eulerAngles.y;
        roll = t.eulerAngles.z;

        x = t.position.x;
        y = t.position.y;
        z = t.position.z;
    }

    public void Translate(Vector3 translation)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

        x += rotatedTranslation.x;
        // y += rotatedTranslation.y;
        z += rotatedTranslation.z;
    }

    public void LerpTowards(CameraState target, float positionLerpPercent, float rotationLerpPercent)
    {
        pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPercent);
        yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPercent);
        roll = Mathf.Lerp(roll, target.roll, rotationLerpPercent);

        x = Mathf.Lerp(x, target.x, positionLerpPercent);
        //y = Mathf.Lerp(y, target.y, positionLerpPercent);
        z = Mathf.Lerp(z, target.z, positionLerpPercent);
    }

    public void UpdateTransform(Transform t)
    {
        t.eulerAngles = new Vector3(pitch, yaw, roll);
        t.position = new Vector3(x, y, z);
    }
}
