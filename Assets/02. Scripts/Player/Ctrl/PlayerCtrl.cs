using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private JoyStickCtrl m_joy_stick;
    private Rigidbody2D m_rigid;
    private SpriteRenderer m_sprite_renderer;
    [SerializeField]
    private Camera m_camera;

    private float m_move_speed = 3f;

    void Start()
    {
        m_joy_stick = GameObject.Find("TouchPanel").GetComponent<JoyStickCtrl>();
        m_rigid = GetComponent<Rigidbody2D>();
        m_sprite_renderer= GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        Vector2 input_vector = m_joy_stick.GetInputVector();
        m_rigid.linearVelocity = new Vector2(input_vector.x * m_move_speed, input_vector.y * m_move_speed);
        if(input_vector.x> 0 )
        {
            m_sprite_renderer.flipX= false;
        }
        else
        {
            m_sprite_renderer.flipX = true;

        }
    }
}
