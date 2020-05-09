using UnityEditor;

public class BuildableUtility
{
    [MenuItem("Assets/Create/Resources/BuildableConfig")]
    public static void CreateBuildableConfig()
    {
        ScriptableObjectUtility.CreateAsset<BuildableConfig>();
    }
}
