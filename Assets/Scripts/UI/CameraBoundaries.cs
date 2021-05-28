using UnityEngine;

namespace UI {
    [DisallowMultipleComponent, RequireComponent(typeof(PolygonCollider2D)), RequireComponent(typeof(Camera))]
    public class CameraBoundaries : MonoBehaviour {

        private PolygonCollider2D myPolygonCollider2D;
        private Camera myCamera;

        private void Awake() {
            myPolygonCollider2D = GetComponent<PolygonCollider2D>();
            myCamera = GetComponent<Camera>();
        }

        private void Start() {
            CreateCameraBoundaries(myCamera.pixelWidth, myCamera.pixelHeight);
        }

        private void CreateCameraBoundaries(int width, int height) {
            if (myCamera == null) {
                return;
            }

            if (myPolygonCollider2D == null) {
                return;
            }

            myPolygonCollider2D.pathCount = 1;
            myPolygonCollider2D.SetPath(0, new Vector2[4] {
                myCamera.ScreenToWorldPoint(new Vector2(0, 0)),
                myCamera.ScreenToWorldPoint(new Vector2(width, 0)),
                myCamera.ScreenToWorldPoint(new Vector2(width, height)),
                myCamera.ScreenToWorldPoint(new Vector2(0, height))
            });
        }
    }
}