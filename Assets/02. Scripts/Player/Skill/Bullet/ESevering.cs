using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ESevering : Severing
{
    private BoxCollider2D[][] m_col_groups = new BoxCollider2D[5][];

    [SerializeField] private BoxCollider2D[] m_combo1_cols;
    [SerializeField] private BoxCollider2D[] m_combo2_cols;
    [SerializeField] private BoxCollider2D[] m_combo3_cols;
    [SerializeField] private BoxCollider2D[] m_combo4_cols;
    [SerializeField] private BoxCollider2D[] m_combo5_cols;

    private int m_current_comb_index = 0; // 현재 진행 중인 콤보 인덱스
    private int m_max_combo = 5; // 최대 콤보 수

    private bool m_is_ani_end = false;

    protected override void Awake()
    {
        base.Awake();
        m_col_groups[0] = m_combo1_cols;
        m_col_groups[1] = m_combo2_cols;
        m_col_groups[2] = m_combo3_cols;
        m_col_groups[3] = m_combo4_cols;
        m_col_groups[4] = m_combo5_cols;
    }

    private void OnEnable()
    {
        m_current_comb_index = 0;
        m_is_ani_end = false;
        StartCoroutine(ComboSequence());
    }
    private IEnumerator ComboSequence()
    {
        while (m_current_comb_index < m_max_combo)
        {
            m_is_ani_end = false;
            string ani_name = $"Combo{m_current_comb_index + 1}";
            m_animator.Play(ani_name);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(EnableColliders(m_current_comb_index));

            yield return new WaitUntil(() => m_is_ani_end);

            m_current_comb_index++;            
        }
        // 콤보 종료 후 비활성화
        transform.gameObject.SetActive(false);
    }

    public void AnimationEnd()
    {
        m_is_ani_end = true;
    }


    private IEnumerator EnableColliders(int comboIndex)
    {
        HashSet<float> triggered_points = new HashSet<float>(); // 중복 실행 방지
        BoxCollider2D[] colliders = m_col_groups[comboIndex]; // 현재 콤보의 콜라이더 그룹

        while (true)
        {
            AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
            float n_time = info.normalizedTime;

            if (n_time >= 0.25f && !triggered_points.Contains(0.25f))
            {
                colliders[0].enabled = true;
                triggered_points.Add(0.25f);
            }

            if (n_time >= 0.5f && !triggered_points.Contains(0.5f))
            {
                colliders[0].enabled = false;
                colliders[1].enabled = true;
                triggered_points.Add(0.5f);
            }

            if (n_time >= 0.7f && !triggered_points.Contains(0.7f))
            {
                colliders[1].enabled = false;
                colliders[2].enabled = true;

                triggered_points.Add(0.7f);
            }
            if (n_time >= 0.9f && !triggered_points.Contains(0.9f))
            {
                colliders[2].enabled = false;
                colliders[3].enabled = true;

                triggered_points.Add(0.9f);
            }

            if (n_time >= 0.97f)
            {
                colliders[3].enabled = false; // 배열의 마지막 콜라이더 비활성화

                foreach (var col in colliders)
                {
                    col.enabled = false;
                }
                yield break;
            }
            yield return null;
        }
    }
}
