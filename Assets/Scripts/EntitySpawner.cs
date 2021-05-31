using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class EntitySpawner : MonoBehaviour {

    [SerializeField] private Mesh entityMesh;
    [SerializeField] private Material entityMaterial;

    private float spawnTimer;
    private float spawnTimerFrequency = .1f;
    private int currentEntities;
    private int maxEntities = 100;

    EntityManager entityManager;

    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

    private float topLimit;
    private float rightLimit;
    private float bottomLimit;
    private float leftLimit;

    private int3[] moveDirectionsArray;

    private void Awake() {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        bottomLimit = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        leftLimit = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        rightLimit = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        topLimit = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);

        moveDirectionsArray = new int3[8];
        moveDirectionsArray[0] = new int3(0, +1, 0); // Up
        moveDirectionsArray[1] = new int3(+1, +1, 0); // Up-Right
        moveDirectionsArray[2] = new int3(+1, 0, 0); // Right
        moveDirectionsArray[3] = new int3(+1, -1, 0); // Down-Right
        moveDirectionsArray[4] = new int3(0, -1, 0); // Down
        moveDirectionsArray[5] = new int3(-1, -1, 0); // Down-Left
        moveDirectionsArray[6] = new int3(-1, 0, 0); // Left
        moveDirectionsArray[7] = new int3(-1, +1, 0); // Up-Left
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f) {
            return;
        }

        if (currentEntities >= maxEntities) {
            return;
        }

        spawnTimer = spawnTimerFrequency;

        currentEntities++;

        // Spawn
        SpawnEnemy();
    }

    private void SpawnEnemy() {


        EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(RenderBounds),
                typeof(WorldRenderBounds),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(ChunkWorldRenderBounds),
                typeof(Enemy),
                typeof(DirectionComponent),
                typeof(MoveSpeedComponent),
                typeof(MoveLimitsComponent)
            );

        Entity spawnedEntity = entityManager.CreateEntity(entityArchetype);

        entityManager.SetSharedComponentData(spawnedEntity, new RenderMesh {
            mesh = entityMesh,
            material = entityMaterial,
        });

        entityManager.SetComponentData(spawnedEntity, new Translation {
            Value = new float3(UnityEngine.Random.Range(leftLimit, rightLimit), UnityEngine.Random.Range(bottomLimit, topLimit), 0)
        });

        entityManager.SetComponentData(spawnedEntity, new DirectionComponent {
            Value = moveDirectionsArray[UnityEngine.Random.Range(0, moveDirectionsArray.Length - 1)]
        });

        entityManager.SetComponentData(spawnedEntity, new MoveSpeedComponent {
            Value = 1f
        });

        entityManager.SetComponentData(spawnedEntity, new MoveLimitsComponent {
            Top = topLimit,
            Right = rightLimit,
            Bottom = bottomLimit,
            Left = leftLimit
        });
    }

    private Mesh CreateQuadMesh() {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-.5f, -.5f);
        vertices[0] = new Vector3(-.5f, +.5f);
        vertices[0] = new Vector3(+.5f, +.5f);
        vertices[0] = new Vector3(+.5f, -.5f);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;

        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private void OnDrawGizmosSelected() {

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        float bottom = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        float left = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        float right = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        float top = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);

        Gizmos.DrawLine(new Vector3(left, bottom), new Vector3(right, bottom));
        Gizmos.DrawLine(new Vector3(right, bottom), new Vector3(right, top));
        Gizmos.DrawLine(new Vector3(right, top), new Vector3(left, top));
        Gizmos.DrawLine(new Vector3(left, top), new Vector3(left, bottom));
    }
}