using UnityEngine;

public class NoJobScene2 : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] scaledVertices;

    public float scaleAmplitude = 0.2f; 
    public float scaleFrequency = 1f;  
    public float speed = 1f;           

    private void Start()
    {
        // Obtener la malla y almacenar sus vértices originales
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        scaledVertices = new Vector3[originalVertices.Length];
    }

    private void Update()
    {
        // Iterar sobre cada vértice para aplicar el escalado
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Calcular el factor de escala basado en una onda sinusoidal
            float scale = 1f + Mathf.Sin(Time.time * speed + vertex.magnitude * scaleFrequency) * scaleAmplitude;

            scaledVertices[i] = vertex * scale;
        }

        // Aplicar los cambios a la malla
        mesh.vertices = scaledVertices;
    }
}
