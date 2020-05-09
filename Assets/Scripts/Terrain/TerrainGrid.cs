using UnityEngine;

public class TerrainGrid : MonoBehaviour
{
    #region Fields
    public Material terrainGridMaterial;

    private Vector2 terrainDimension;
    #endregion

    #region Unity Methods
    private void Start()
    {
        terrainDimension = TerrainController.instance.Dimension;
    }

    private void Update()
    {
        GetRaycastToTerrain();

    }
    #endregion

    public void DrawMesh(bool drawMesh) => GetComponent<MeshRenderer>().enabled = drawMesh;

    private void GetRaycastToTerrain()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            SetGridMaskPosition(hit.point);
        }
    }

    private void SetGridMaskPosition(Vector3 position)
    {
        Vector4 newPosition = new Vector4(position.x / terrainDimension.x, position.z / terrainDimension.y);
        terrainGridMaterial.SetVector("_MaskPosition", newPosition);
    }
}
