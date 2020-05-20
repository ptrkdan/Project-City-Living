using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Instance;
    public static InputController instance;
    #endregion

    #region Fields
    bool buildReset = false;
    Buildable selectedBuildable;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        #region Singleton Check
        if (instance) return;
        instance = this;
        #endregion
    }

    private void Update()
    {
        TryAddBuildable();
    }
    #endregion

    #region Buildable Methods
    public void SelectBuildable(Buildable buildable)
    {
        selectedBuildable = buildable;
        TerrainController.instance.EnableGrid();
    }

    private void TryAddBuildable()
    {
        bool isSuccess;
        Ray ray;
        if (Input.GetMouseButton(0) && selectedBuildable != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (selectedBuildable is BuildablePath)
                {
                    isSuccess = TerrainController.instance.TryAddBuildablePath((BuildablePath)selectedBuildable, hit.point, buildReset);
                }
                else
                {
                    isSuccess = TerrainController.instance.TryAddBuildable(selectedBuildable, hit.point, hit.point);
                }

                if (!isSuccess)
                {
                    // Display error message
                }
                buildReset = false;
            }

            if (!(selectedBuildable is BuildablePath))
            {
                DeselectBuildable();
            }
        }
        
        // Origin release
        if (Input.GetMouseButtonUp(0))
        {
            buildReset = true;
        }

        // Right-click cancel
        if (Input.GetMouseButton(1) && selectedBuildable)
        {
            DeselectBuildable();
        } 
    }

    private void DeselectBuildable()
    {
        buildReset = true;
        selectedBuildable = null;
        TerrainController.instance.DisableGrid();
    }
    #endregion
}
