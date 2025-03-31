using UnityEngine;
using System.Collections.Generic;

public class PlayerCtrl : MonoBehaviour
{ 
    private Rigidbody2D m_rigid;
    private SpriteRenderer m_sprite_renderer;
    private SkillManager m_skill_manager;

    public JoyStickCtrl joyStick { get; private set; }
    public Animator Animator { get; private set; }

    private float m_move_speed = 3f;

    

    void Start()
    {
        m_skill_manager = GameObject.Find("GameManager").GetComponent<SkillManager>();
        joyStick = GameObject.Find("TouchPanel").GetComponent<JoyStickCtrl>();
        m_rigid = GetComponent<Rigidbody2D>();
        m_sprite_renderer= GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {      
        Vector2 input_vector = joyStick.GetInputVector();

        m_rigid.linearVelocity = new Vector2(input_vector.x * m_move_speed, input_vector.y * m_move_speed);
        if (input_vector.x > 0)
        {
            m_sprite_renderer.flipX = false;
        }
        else if (input_vector.x < 0)
        {
            m_sprite_renderer.flipX = true;
        }
        Animator.SetBool("IsMove", input_vector.sqrMagnitude > 0 );

        m_skill_manager.UseSkills();
    }
}
