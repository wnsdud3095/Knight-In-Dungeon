using UnityEngine;

public class ScreenOutlinCtrl : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D[] colliders;

    void Start()
    {
        MakeOutLine();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    public void MakeOutLine()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float cam_height = cam.orthographicSize * 2f;
        float cam_width = cam_height * cam.aspect;

        colliders[0].size = new Vector2(0.1f, cam_height); // Left
        colliders[0].offset = new Vector2(-cam_width / 2, 0);

        colliders[1].size = new Vector2(0.1f, cam_height); // Right
        colliders[1].offset = new Vector2(cam_width / 2, 0);

        colliders[2].size = new Vector2(cam_width, 0.1f); // Bottom
        colliders[2].offset = new Vector2(0, -cam_height / 2);

        colliders[3].size = new Vector2(cam_width, 0.1f); // Top
        colliders[3].offset = new Vector2(0, cam_height / 2);

    }
}
