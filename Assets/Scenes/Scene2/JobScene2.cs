using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
public class JobScene2 : MonoBehaviour
{
    private Mesh mesh;
    private NativeArray<float3> vertices;
    private NativeArray<float3> scaledVertices;

    public float scaleAmplitude = 0.2f;
    public float scaleFrequency = 1f;
    public float speed = 1f;

    void Start()
    {
        // Obtener la malla y los vértices originales
        mesh = GetComponent<MeshFilter>().mesh;        

        // Crear NativeArrays para los vértices
        vertices = new NativeArray<float3>(mesh.vertexCount, Allocator.Persistent);

        using (var dataArray = Mesh.AcquireReadOnlyMeshData(mesh))
        {
            dataArray[0].GetVertices(vertices.Reinterpret<Vector3>());
        }

        scaledVertices = new NativeArray<float3>(mesh.vertexCount, Allocator.Persistent);
    }

    void Update()
    {
        PulseJob pulseJob = new PulseJob
        {
            vertices = vertices,
            scaledVertices = scaledVertices,
            time = Time.time * speed,
            scaleAmplitude = scaleAmplitude,
            scaleFrequency = scaleFrequency
        };
        JobHandle pulseJobHandle = pulseJob.Schedule(vertices.Length, 64);
        pulseJobHandle.Complete();

        mesh.SetVertices(scaledVertices);
    }

    private void OnDestroy()
    {
        // Liberar memoria de NativeArrays
        vertices.Dispose();
        scaledVertices.Dispose();
    }
}

[BurstCompile]
struct PulseJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> vertices;
    [ReadOnly] public float time;
    [ReadOnly] public float scaleAmplitude;
    [ReadOnly] public float scaleFrequency;

    [WriteOnly] public NativeArray<float3> scaledVertices;

    public void Execute(int index)
    {
        float3 vertex = vertices[index];

        float scale = 1f + Mathf.Sin(time + math.length(vertex) * scaleFrequency) * scaleAmplitude;
        scaledVertices[index] = vertex * scale; 
    }
}
