using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Instance;
    public static InputController instance;
    #endregion

    #region Fields
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
        if (Input.GetMouseButton(0) && selectedBuildable != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                bool isSuccess = TerrainController.instance.AddBuildable(selectedBuildable, hit.point);
                if (!isSuccess)
                {
                    // Display error message
                }
                else
                {
                    selectedBuildable = null;
                    TerrainController.instance.DisableGrid();
                }
            }
        }
    }
    #endregion
}
