using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

public class JobScript1 : MonoBehaviour
{
    public Transform objList;
    List<Transform> transformList;
    bool listDone = false;

    public float amplitude = 1f; // Altura máxima del movimiento
    public float frequency = 1f; // Velocidad del movimiento

    public Transform target; // El GameObject al que mirará
    public float minDistance = 5f; // Distancia mínima antes de apartarse
    public float avoidSpeed = 7f; // Velocidad a la que se aparta



    TransformAccessArray transformArray;
    NativeList<float3> startPostions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformList = new List<Transform>();
        startPostions = new NativeList<float3>(Allocator.TempJob);
        for (int i = 0; i < objList.childCount; i++)
        {
            Transform child1 = objList.GetChild(i);
            for (int j = 0; j < child1.childCount; j++)
            {
                transformList.Add(child1.GetChild(j));
                startPostions.Add(transformList[transformList.Count - 1].position);
            }
        }
        transformArray = new TransformAccessArray(transformList.ToArray());
        listDone = true;
    }

    private void OnDestroy()
    {
        startPostions.Dispose();
        transformArray.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        if (!listDone) return;

        ObjControllerJob job = new ObjControllerJob()
        {
            playerPos = target.position,
            deltaTime = Time.deltaTime,
            distanciaLimite = minDistance,
            speed = avoidSpeed,
            frequency = frequency,
            amplitude = amplitude,
            startPositions = startPostions,
            time = Time.time
        };

        JobHandle jobHandle = job.Schedule(transformArray);
        jobHandle.Complete();
    }
}

[BurstCompile]
public struct ObjControllerJob : IJobParallelForTransform
{
    [ReadOnly] public float3 playerPos;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public float distanciaLimite;
    [ReadOnly] public float speed;
    [ReadOnly] public float frequency;
    [ReadOnly] public float amplitude;
    [ReadOnly] public NativeList<float3> startPositions;
    [ReadOnly] public float time;
    public void Execute(int index, TransformAccess transform)
    {
        // Movimiento sinusoidal en el eje Y
        float newY = startPositions[index].y + math.sin(time * frequency) * amplitude;
        float3 newPosition = new float3(transform.position.x, newY, transform.position.z);

        float3 direction = math.normalize(playerPos - (float3)transform.position);
        Quaternion targetRotation = quaternion.LookRotation(direction, new float3(0,1,0));
        transform.rotation = targetRotation;

        // Move away from player
        float distance = math.distance(playerPos, transform.position);
        float3 movedPos = newPosition - direction * deltaTime * speed;
        transform.position = math.select(newPosition, movedPos, distance <= distanciaLimite);
    }
}
