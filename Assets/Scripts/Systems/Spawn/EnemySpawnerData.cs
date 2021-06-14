using System;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerData : MonoBehaviour {

    public static EnemySpawnerData Instance { get; private set; }

    private EntityManager entityManger;

    [Header("Limits")]
    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

    [Header("Movement")]
    [SerializeField] private AnimationCurve spawnFrequencyCurve;
    public AnimationCurve SpawnFrequencyCurve => spawnFrequencyCurve;
    [SerializeField] private AnimationCurve moveSpeedCurve;
    public AnimationCurve MoveSpeedCurve => moveSpeedCurve;
    [SerializeField] private AnimationCurve directionChangeFrequencyCurve;
    public AnimationCurve DirectionChangeFrequencyCurve => directionChangeFrequencyCurve;

    [Header("Death")]
    [SerializeField] private AnimationCurve lifetimeCurve;
    public AnimationCurve LifetimeCurve => lifetimeCurve;
    [SerializeField] private float deathDuration;
    public float DeathDuration => deathDuration;

    [Header("Rendering")]
    [SerializeField] private Mesh mesh;
    public Mesh Mesh => mesh;
    [SerializeField] private Material material;
    public Material Material => material;
    [SerializeField] private Gradient colorGradient;
    public Gradient ColorGradient => colorGradient;
    [SerializeField] private Color deathColor;
    public Color DeathColor => deathColor;

    private float spawnLimitTop;
    public float SpawnLimitTop => spawnLimitTop;
    private float spawnLimitRight;
    public float SpawnLimitRight => spawnLimitRight;
    private float spawnLimitBottom;
    public float SpawnLimitBottom => spawnLimitBottom;
    private float spawnLimitLeft;
    public float SpawnLimitLeft => spawnLimitLeft;

    private void Awake() {

        Instance = this;

        entityManger = World.DefaultGameObjectInjectionWorld.EntityManager;

        CalculateSpawnLimits();
    }

    private void Start() {
        DOTS_GameHandler.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= OnGameStarted;
    }

    private void CalculateSpawnLimits() {

        Vector3 spawnAreaBottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        spawnLimitBottom = spawnAreaBottomLeftCorner.y + Mathf.Abs(borderBottom);
        spawnLimitLeft = spawnAreaBottomLeftCorner.x + Mathf.Abs(borderLeft);

        Vector3 spawnAreaTopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

        spawnLimitRight = spawnAreaTopRightCorner.x - Mathf.Abs(borderRight);
        spawnLimitTop = spawnAreaTopRightCorner.y - Mathf.Abs(borderTop);

    }

    private void OnGameStarted(object sender, EventArgs args) {
        CalculateSpawnLimits();
    }

    private void OnDrawGizmosSelected() {

        CalculateSpawnLimits();

        Gizmos.DrawLine(new Vector3(spawnLimitLeft, spawnLimitBottom), new Vector3(spawnLimitRight, spawnLimitBottom));
        Gizmos.DrawLine(new Vector3(spawnLimitRight, spawnLimitBottom), new Vector3(spawnLimitRight, spawnLimitTop));
        Gizmos.DrawLine(new Vector3(spawnLimitRight, spawnLimitTop), new Vector3(spawnLimitLeft, spawnLimitTop));
        Gizmos.DrawLine(new Vector3(spawnLimitLeft, spawnLimitTop), new Vector3(spawnLimitLeft, spawnLimitBottom));

    }

}