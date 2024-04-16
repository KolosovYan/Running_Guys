using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Entity_Creator : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private Transform container;
    [SerializeField] private Entity_Controller prefab;

    [Header("Settings")]

    [SerializeField] private int objectsCount;
    [SerializeField] private int lineLength;
    [SerializeField] private float offset;

    [Header("Spawn Borders")]

    [SerializeField] private Transform LeftUpperPoint;
    [SerializeField] private Transform RightDownPoint;

    private List<Entity_Controller> entitys;
    private float spawnPosX;
    private float spawnPosZ;
    private float stepX;
    private float stepZ;
    private float lineIndex;

    public void MovePoints(Vector3[] points)
    {
        float step = (float)points.Length / entitys.Count;

        for (int i = 0; i < entitys.Count; i++)
        {
            int index = Mathf.RoundToInt(i * step) % points.Length;

            if (index < points.Length)
                entitys[i].SetDestinyPoint(points[index]);

        }
    }

    public void CreateNewEntity(Vector3 pos)
    {
       Entity_Controller ec = Instantiate(prefab, container);
        ec.transform.localPosition = pos + new Vector3(Random.Range(0, 0.2f), 0, Random.Range(0, 0.2f));
        entitys.Add(ec);
    }

    public void Entity_Destroyed(Entity_Controller ent)
    {
        entitys.Remove(ent);
    }

    private void Start()
    {
        entitys = new List<Entity_Controller>();
        SetStartPositions();
        CreateStartEntitys();
    }

    private void SetStartPositions()
    {
        lineIndex = (float)objectsCount / lineLength;
        lineIndex = Mathf.CeilToInt(lineIndex);

        stepX = ((RightDownPoint.position.x - LeftUpperPoint.position.x) - (offset * 2)) / lineLength;
        stepZ = ((LeftUpperPoint.position.z - RightDownPoint.position.z) - (offset * 2)) / lineIndex;

        spawnPosX = LeftUpperPoint.position.x + offset;
        spawnPosZ = LeftUpperPoint.position.z - offset;
    }

    private void CreateStartEntitys()
    {
        int counter = 0;
        float currentPosX = spawnPosX;
        float currentPosZ = spawnPosZ;

        for (int i = 0; i < objectsCount; i++)
        {
            Entity_Controller ec = Instantiate(prefab, container);

            ec.SetCreator(this);
            ec.transform.position = new Vector3(currentPosX, 0f, currentPosZ);
            entitys.Add(ec);

            currentPosX += stepX;
            counter++;

            if (counter == lineLength)
            {
                currentPosZ -= stepZ;
                currentPosX = spawnPosX;
                counter = 0;
            }
        }
    }
}
