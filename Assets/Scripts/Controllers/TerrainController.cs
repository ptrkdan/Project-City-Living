using UnityEngine;

public class TerrainController : MonoBehaviour
{
    #region Instance
    public static TerrainController instance;
    #endregion

    #region Constants
    private const float GRID_Y_OFFSET = 0.5f;
    #endregion

    #region Fields
    Vector2Int dimension = new Vector2Int();
    TerrainGrid terrainGrid;
    GameObject buildableList;
    Buildable[,] buildables;
    #endregion

    #region Properties
    public Vector2Int Dimension { get => dimension; private set => dimension = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        #region Singleton Check
        if (instance) return;
        instance = this;
        #endregion

        dimension = new Vector2Int((int)transform.localScale.x, (int)transform.localScale.z);
        terrainGrid = gameObject.GetComponentInChildren<TerrainGrid>();

        buildableList = GameObject.FindGameObjectWithTag("BuildableList");
        buildables = new Buildable[dimension.x, dimension.y];
    }
    #endregion

    #region Terrain Grid Methods
    public void EnableGrid() => terrainGrid.DrawMesh(true);

    public void DisableGrid() => terrainGrid.DrawMesh(false);
    #endregion

    #region Buildable Methods
    public bool AddBuildable(Buildable buildable, Vector3 position)
    {
        bool isSuccess = false;

        Vector3 gridPosition = GetGridPosition(position);
        if (IsGridPositionAvailable(gridPosition))
        {
            buildables[(int)gridPosition.x, (int)gridPosition.z] 
                = Instantiate(buildable, gridPosition, buildable.transform.rotation, buildableList.transform);
            isSuccess = true;
        }

        return isSuccess;
    }

    public bool RemoveBuildable(Vector3 position)
    {
        bool isSuccess = false;
        Vector3 gridPosition = GetGridPosition(position);
        if (IsGridPositionAvailable(gridPosition))
        {
            buildables[(int)gridPosition.x, (int)gridPosition.z].Destroy();
            buildables[(int)gridPosition.x, (int)gridPosition.z] = null;
            isSuccess = true;
        }

        return isSuccess;
    }

    private Vector3 GetGridPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), GRID_Y_OFFSET, Mathf.Round(position.z));
    }

    private bool IsGridPositionAvailable(Vector3 position)
    {
        return buildables[(int)position.x, (int)position.z] == null;
    }
    #endregion
}
