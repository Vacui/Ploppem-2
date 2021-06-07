﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utils;

public class EnemySpawnerData : MonoBehaviour {

    public static EnemySpawnerData Instance { get; private set; }

    private EntityManager entityManger;

    [Header("Rendering")]
    [SerializeField] private Mesh mesh;
    public Mesh Mesh => mesh;
    [SerializeField] private Material material;
    public Material Material => material;
    [SerializeField] private Gradient colorGradient;
    public Gradient ColorGradient => colorGradient;

    [Header("Stats")]
    [SerializeField] private float spawnFrequency;
    public float SpawnFrequency => spawnFrequency;
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    [SerializeField] private AnimationCurve moveSpeedCurve;
    public AnimationCurve MoveSpeedCurve => moveSpeedCurve;
    [SerializeField] private float directionChangeFrequency;
    public float DirectionChangeFrequency => directionChangeFrequency;
    [SerializeField] private float lifetime;
    public float Lifetime => lifetime;

    [Header("Limits")]
    [SerializeField] private float borderTop;
    [SerializeField] private float borderRight;
    [SerializeField] private float borderBottom;
    [SerializeField] private float borderLeft;

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