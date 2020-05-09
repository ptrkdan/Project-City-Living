using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField] BuildableConfig config;

    public BuildableConfig Config { get => config; }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
