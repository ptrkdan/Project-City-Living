using System.Collections.Generic;
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
    List<Vector3> buildVector = new List<Vector3>();

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
    public bool TryAddBuildable(Buildable buildable, Vector3 position, Vector3 buildOrigin)
    {
        bool isSuccess = false;

        Vector3 gridPosition = GetGridPosition(position);
        if (IsGridPositionAvailable(gridPosition))
        {
            AddBuildable(buildable, gridPosition);
            isSuccess = true;
        }

        return isSuccess;
    }

    public bool TryAddBuildablePath(BuildablePath buildable, Vector3 posNext, bool reset = false)
    {
        bool isSuccess = true;
        Vector3 gridPosNext = GetGridPosition(posNext);

        if (reset) buildVector.Clear();
        
        if (IsBacktracking(gridPosNext))
        {
            int lastIndex = buildVector.Count - 1;
            RemoveBuildable(buildVector[lastIndex]);
            buildVector.RemoveAt(lastIndex);
            return false;
        }

        if (IsGridCoordAligned(gridPosNext) && IsGridPositionAvailable(gridPosNext))
        {
            AddBuildable(buildable, gridPosNext);
            buildVector.Add(gridPosNext);
        }

        return isSuccess;
    }

    private void AddBuildable(Buildable buildable, Vector3 gridPosition)
    {
        buildables[(int)gridPosition.x, (int)gridPosition.z]
          = Instantiate(buildable, gridPosition, buildable.transform.rotation, buildableList.transform);
    }

    public bool RemoveBuildable(Vector3 gridPosition)
    {
        bool isSuccess = false;
        if (!IsGridPositionAvailable(gridPosition))
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

    private bool IsBacktracking(Vector3 position)
    {
        int lastIndex = buildVector.Count - 1;
        if (buildVector.Count < 2) return false; // If the build vector has one or less, then cannot backtrack
        return (buildVector[lastIndex - 1] == position);
    }
    private bool IsGridCoordAligned(Vector3 position)
    {
        int lastIndex = buildVector.Count - 1;
        if (buildVector.Count < 1) return true; // If the build vector is empty, then nothing has been built yet.
        return (buildVector[lastIndex].x == position.x || buildVector[lastIndex].z == position.z);
    }

    #endregion
}
