using UnityEditor;

public class BuildableUtility
{
    [MenuItem("Assets/Create/Resources/BuildableConfig")]
    public static void CreateBuildableConfig()
    {
        ScriptableObjectUtility.CreateAsset<BuildableConfig>();
    }

    [MenuItem("Assets/Create/Resources/BuildablePathConfig")]
    public static void CreateBuildablePathConfig()
    {
        ScriptableObjectUtility.CreateAsset<BuildablePathConfig>();
    }
}
