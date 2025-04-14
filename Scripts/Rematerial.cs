using UnityEngine;

public class Rematerial : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private GameObject[] objectsToRematerial;
    [SerializeField] private Color backgroundCameraColor;
    public void ReMaterial()
    {
        foreach(var o in objectsToRematerial)
        {
            Camera.main.backgroundColor = backgroundCameraColor;
            o.GetComponent<Renderer>().material = material;
        }
    }
}
