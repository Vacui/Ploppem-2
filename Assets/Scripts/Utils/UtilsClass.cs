using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Utils {
    public static class UtilsClass {
        // source: https://www.codegrepper.com/code-examples/csharp/how+to+clear+console+through+script+unity
        // Commented to avoid build error
        public static void ClearLogConsole() {
            //Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            //Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            //MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            //clearConsoleMethod.Invoke(new object(), null);
        }

        //source: https://stackoverflow.com/a/5320727
        public static bool In<T>(this T val, params T[] values) where T : struct {
            return values.Contains(val);
        }

        public static void AddBlankRange(this List<string> list, int count) {
            if (count <= 0) return;

            for (int i = 0; i < count; i++) {
                list.Add("");
            }
        }

        public static void SetObjectsActive(GameObject[] gameObjects, bool active) {
            if (gameObjects == null || gameObjects.Length <= 0) return;

            foreach (GameObject gameObject in gameObjects) {
                SetObjectActive(gameObject, active);
            }
        }

        public static void SetObjectActive(GameObject gameObject, bool active) {
            if (gameObject == null) return;

            gameObject?.SetActive(active);
        }

        public static List<Vector2Int> GatherNeighbours(int x = 0, int y = 0, int radius = 1, bool avoidCenter = false, bool avoidCorners = false) {
            if (radius <= 0) return null;

            List<Vector2Int> neighbours = new List<Vector2Int>();
            for (int xT = -radius; xT < radius + 1; xT++) {
                for (int yT = -radius; yT < radius + 1; yT++) {
                    if (!avoidCenter || (xT != 0 || yT != 0)) {
                        if (!avoidCorners || (Mathf.Abs(xT) != Mathf.Abs(yT))) {
                            neighbours.Add(new Vector2Int(x + xT, y + yT));
                        }
                    }
                }
            }
            return neighbours;
        }

        public static int RandomWithExceptions(int start, int end, List<int> exceptions) {
            if (exceptions != null) {
                int result;
                int limit = Mathf.Max(start, end) - Mathf.Min(start, end);
                int tests = 0;
                bool tryAgain = true;
                while (tests < limit && tryAgain) {
                    result = UnityEngine.Random.Range(start, end);
                    if (!exceptions.Contains(result)) return result;
                    tests++;
                }
                throw new KeyNotFoundException();
            } else {
                return UnityEngine.Random.Range(start, end);
            }
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ();
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetTouchWorldPositionWithZ(Camera worldCamera, out bool valid) {

            valid = false;
            Vector3 worldPosition = Vector3.zero;

            for (int i = 0; i < Input.touchCount && !valid; i++) {
                if (Input.GetTouch(i).phase != TouchPhase.Began) {
                    continue;
                }
                worldPosition = worldCamera.ScreenToWorldPoint(Input.GetTouch(i).position);
                valid = true;
            }

            return worldPosition;
        }
        public static Vector3 GetTouchWorldPositionWithZ(out bool valid) {
            Vector3 worldPosition = GetTouchWorldPositionWithZ(Camera.main, out valid);
            return worldPosition;
        }
        public static Vector3 GetTouchWorldPosition(out bool valid) {
            Vector3 vec = GetTouchWorldPositionWithZ(out valid);
            vec.z = 0f;
            return vec;
        }

        // source: https://stackoverflow.com/a/129395
        public static T DeepClone<T>(T obj) {
            using (var ms = new System.IO.MemoryStream()) {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        // source: https://unitycodemonkey.com/utils.php
        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static Vector3 RemoveZ(this Vector3 vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }

        public static Color SetColorAlpha(Color color, float alpha) {
            color.a = alpha;
            return color;
        }

        public static Color ToColor(this float4 color) {
            return new Color(color.x, color.y, color.z, color.w);
        }
    }
}