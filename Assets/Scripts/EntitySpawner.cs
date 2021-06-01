using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class EntitySpawner : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private int maxEntities;
    [SerializeField] private float spawnTimerFrequency = 0.1f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float directionChangeTime;
    [SerializeField] private float deathTime;

    [Header("Enemy visuals")]
    [SerializeField] private Mesh entityMesh;
    [SerializeField] private Material entityMaterial;

    private float spawnTimer;
    private int currentEntities;

    EntityManager entityManager;

    [Header("Limits")]
    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

    private float topLimit;
    private float rightLimit;
    private float bottomLimit;
    private float leftLimit;

    private void Awake() {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        bottomLimit = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        leftLimit = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        rightLimit = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        topLimit = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);
    }

    private void Update() {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            SpawnEnemy(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition());
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0) {
            Vector3 touchWorldPosition = Utils.UtilsClass.GetTouchWorldPosition(out bool valid);
            if (valid) {
                SpawnEnemy(touchWorldPosition);
            }
        }
#endif

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f) {
            return;
        }

        if (currentEntities >= maxEntities) {
            return;
        }
        

        // Spawn
        SpawnEnemy();
    }

    private void SpawnEnemy() {
        SpawnEnemy(new float3(UnityEngine.Random.Range(leftLimit, rightLimit), UnityEngine.Random.Range(bottomLimit, topLimit), 0));
    }

    private void SpawnEnemy(float3 worldPosition) {
        currentEntities++;
        spawnTimer = spawnTimerFrequency;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(RenderBounds),
                typeof(WorldRenderBounds),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(ChunkWorldRenderBounds),
                typeof(Enemy),
                typeof(DirectionComponent),
                typeof(DirectionChangeTimerComponent),
                typeof(MoveSpeedComponent),
                typeof(MoveLimitsComponent)
            );

        Entity spawnedEntity = entityManager.CreateEntity(entityArchetype);

        entityManager.SetSharedComponentData(spawnedEntity, new RenderMesh {
            mesh = entityMesh,
            material = entityMaterial,
        });


        worldPosition.x = Mathf.Clamp(worldPosition.x, leftLimit, rightLimit);
        worldPosition.y = Mathf.Clamp(worldPosition.y, bottomLimit, topLimit);
        entityManager.SetComponentData(spawnedEntity, new Translation {
            Value = worldPosition
        });

        entityManager.SetComponentData(spawnedEntity, new DirectionChangeTimerComponent {
            StartValue = directionChangeTime
        });

        entityManager.SetComponentData(spawnedEntity, new MoveSpeedComponent {
            Value = moveSpeed
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