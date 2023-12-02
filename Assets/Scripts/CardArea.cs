using UnityEngine;

public class CardArea : MonoBehaviour
{
    // CardAreaInspector를 구현하여 Type에 따라
    // Field 제한 동작을 추가하면 더 좋다.
    protected enum CardArrangeType
    {
        INTERVAL_FIXED,            // 고정 간격, m_fixedInterval 참조
        INTERVAL_BY_MAX_COUNT,     // Area에 최대 들어갈 수 있는 카드수에 따른 위치 지정
        INTERVAL_JUSTIFICATION,    // 좌우 끝 정렬을 통한 카드 위치 지정
    }
    
    [SerializeField] 
    private RectTransform m_moveAreaTransform;
    
    [SerializeField] 
    private CardArrangeType m_type;

    [SerializeField] 
    [Range(0, 20)]
    [Tooltip("INTERVAL_BY_MAX_COUNT")]
    private int m_maxCardCount = 15;
    
    [Range(0, 70)]
    [Tooltip("INTERVAL_FIXED")]
    [SerializeField] private float m_fixedInterval = 65.0f;

    private Vector2 m_start;
    private Vector2 m_end;

    private float HeightInWorld => m_start.y - m_end.y;

    private float FixedIntervalInWorld
    {
        get
        {
            if (ObjectUtility.IsNullOrDestroyed(m_moveAreaTransform))
            {
                return m_fixedInterval;
            }

            float height = m_moveAreaTransform.sizeDelta.y;
            
            return height > 0 ? m_fixedInterval * HeightInWorld / height : m_fixedInterval;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        Vector3[] corners = new Vector3[4];
        m_moveAreaTransform.GetWorldCorners(corners);
        
        m_start = corners[1];
        m_end = corners[3];
    }

    public Vector2 GetPosition(float cardSize, int totalCount, int index)
    {
        float y = 0;
        switch (m_type)
        {
            case CardArrangeType.INTERVAL_FIXED:
                y = m_start.y - FixedIntervalInWorld * index;
                break;
            case CardArrangeType.INTERVAL_BY_MAX_COUNT:
                y = m_start.y - (HeightInWorld - cardSize) / (m_maxCardCount - 1) * index;
                break;
            case CardArrangeType.INTERVAL_JUSTIFICATION:
                y = m_start.y - (HeightInWorld - cardSize * 0.5f) * index / totalCount;
                break;
        }
        
        return new Vector2((m_start.x + m_end.x) * 0.5f, y - cardSize * 0.5f);
    }
}