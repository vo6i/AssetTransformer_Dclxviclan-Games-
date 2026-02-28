using UnityEngine;
using System.IO;
using System.Globalization;

public class FaceMeshBuilder : MonoBehaviour
{
    public string fileName = "face_points.txt";
    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // Для меша лица лучше использовать более 400 точек
        vertices = new Vector3[468]; 
    }

    void Update()
    {
        string path = Path.Combine(Application.dataPath, fileName);
        if (!File.Exists(path)) return;

        string[] lines = File.ReadAllLines(path);
        if (lines.Length < 468) return;

        for (int i = 0; i < 468; i++)
        {
            string[] c = lines[i].Split(',');
            // Умножаем на 2-5 для масштаба
            vertices[i] = new Vector3(
                float.Parse(c[0], CultureInfo.InvariantCulture) * 3,
                float.Parse(c[1], CultureInfo.InvariantCulture) * 3,
                float.Parse(c[2], CultureInfo.InvariantCulture) * 3
            );
        }

        mesh.vertices = vertices;

        // Если это первый запуск, нужно задать топологию
        if (mesh.vertexCount > 0 && mesh.GetTopology(0) != MeshTopology.Points)
        {
            int[] indices = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) indices[i] = i;
            mesh.SetIndices(indices, MeshTopology.Points, 0);
        }
        
        mesh.RecalculateBounds();
    }
}
