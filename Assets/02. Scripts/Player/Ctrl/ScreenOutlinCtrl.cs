using UnityEngine;

public class ScreenOutlinCtrl : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D[] m_outline_colliders;

    [SerializeField]
    private BoxCollider2D m_outline_box;

    public Camera Cam { get; private set; }
    public float CamHeight { get; private set; }
    public float CamWidth { get; private set; }

    void Start()
    {
        MakeOutLine();
        Cam = Camera.main;
        CamHeight = Cam.orthographicSize * 2f;
        CamWidth = CamHeight * Cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    public void MakeOutLine()
    {
        m_outline_colliders[0].size = new Vector2(0.1f, CamHeight); // Left
        m_outline_colliders[0].offset = new Vector2(-CamWidth / 2, 0);

        m_outline_colliders[1].size = new Vector2(0.1f, CamHeight); // Right
        m_outline_colliders[1].offset = new Vector2(CamWidth / 2, 0);

        m_outline_colliders[2].size = new Vector2(CamWidth, 0.1f); // Bottom
        m_outline_colliders[2].offset = new Vector2(0, -CamHeight / 2);

        m_outline_colliders[3].size = new Vector2(CamWidth, 0.1f); // Top
        m_outline_colliders[3].offset = new Vector2(0, CamHeight / 2);

    }
    public void MakeBox()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float cam_height = cam.orthographicSize * 2f;
        float cam_width = cam_height * cam.aspect;

        m_outline_box.size = new Vector2(cam_width, cam_height);
    }

}
