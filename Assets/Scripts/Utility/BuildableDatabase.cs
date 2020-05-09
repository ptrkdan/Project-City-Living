using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildableDatabase
{
    private static List<BuildableConfig> _buildables;
    private static bool _isDatabaseLoaded = false;

    public static void LoadDatabase()
    {
        if (_isDatabaseLoaded) return;
        _isDatabaseLoaded = true;
        LoadDatabaseForce();
    }

    public static void LoadDatabaseForce()
    {
        ValidateDatebase();
        BuildableConfig[] resources = Resources.LoadAll<BuildableConfig>(@"BuildableConfigs");
        foreach (var buildable in resources)
        {
            if (!_buildables.Contains(buildable)) _buildables.Add(buildable);
        }
    }

    public static void ClearDatabase()
    {
        _isDatabaseLoaded = false;
        _buildables.Clear();
    }

    public static BuildableConfig GetBuildableConfig(int id)
    {
        BuildableConfig config = null;

        ValidateDatebase();

        var buildable = _buildables.SingleOrDefault(x => x.id == id);
        if (buildable != null) config = ScriptableObject.Instantiate(buildable);

        return config;
    }

    private static void ValidateDatebase()
    {
        if (_buildables == null) _buildables = new List<BuildableConfig>();
        if (!_isDatabaseLoaded) LoadDatabase();
    }
}
