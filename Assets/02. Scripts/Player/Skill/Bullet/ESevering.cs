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
        StartCoroutine(ComboSequence());
    }
    private IEnumerator ComboSequence()
    {
        while (m_current_comb_index < m_max_combo)
        {
            string ani_name = $"Combo{m_current_comb_index + 1}";
            m_animator.Play(ani_name);

            yield return StartCoroutine(EnableColliders(m_current_comb_index));

            m_current_comb_index++;

            yield return new WaitForSeconds(0.85f); // 콤보 간 딜레이
        }

        // 콤보 종료 후 비활성화
        transform.gameObject.SetActive(false);
    }

    private IEnumerator EnableColliders(int comboIndex)
    {
        HashSet<float> triggered_points = new HashSet<float>(); // 중복 실행 방지
        BoxCollider2D[] colliders = m_col_groups[comboIndex]; // 현재 콤보의 콜라이더 그룹

        int col_num = colliders.Length; // 콜라이더 개수 동적 할당

        if (col_num == 0) yield break; // 만약 배열이 비어있으면 종료

        float[] trigger_times = new float[col_num]; // 트리거 시간을 저장할 배열

        // 콜라이더 개수에 따라 트리거 타이밍 자동 분배
        for (int i = 0; i < col_num; i++)
        {
            trigger_times[i] = 1f / (col_num + 1) * (i + 1); // 예: 3개면 0.25, 0.5, 0.75
        }

        int currentIndex = 0;

        while (true)
        {
            AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
            float n_time = info.normalizedTime;

            if (n_time >= 1f)
            {
                colliders[^1].enabled = false; // 마지막 콜라이더 비활성화
                yield break;
            }

            // 현재 트리거 타이밍에 도달하면 콜라이더 활성화
            if (currentIndex < col_num && n_time >= trigger_times[currentIndex] 
                && !triggered_points.Contains(trigger_times[currentIndex]))
            {
                if (currentIndex > 0) colliders[currentIndex - 1].enabled = false; // 이전 콜라이더 비활성화
                colliders[currentIndex].enabled = true;
                triggered_points.Add(trigger_times[currentIndex]);
                currentIndex++;
            }

            yield return null;
        }
    }
}
