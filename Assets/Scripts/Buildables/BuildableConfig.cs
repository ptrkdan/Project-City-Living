using UnityEngine;

public class BuildableConfig : ScriptableObject
{
    public int id;

    public int cost;

    [TextArea]
    public string description;
}
