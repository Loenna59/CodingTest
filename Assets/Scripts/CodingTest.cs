using UnityEngine;

using System.Collections.Generic;

public class CodingTest : MonoBehaviour
{
    private Table table;
    
    [SerializeField]
    private CardArea m_area1;
    
    [SerializeField]
    private CardArea m_area2;
    
    [SerializeField]
    private List<CardViewer> m_cardViewers;
    
    private RectTransform m_cardDeckRectTransform;
    private TweenController m_tweenController;
    
    [SerializeField]
    [Range(0f, 2f)]
    [Tooltip("카드 한장 당 Area 이동 시간")]
    private float m_duration = 0.5f;

    [SerializeField]
    [Range(0f, 2f)]
    [Tooltip("카드 간의 이동 시간 간격")]
    private float m_cardMovingIntervalTime = 0.1f;
    
    [SerializeField] 
    [Tooltip("같은 버튼 연타에 대한 동작 적용 허용")]
    private bool m_isEnableDuplicatedCall = false;
    
    public class Card
    {
        public string Name, Image;
        public int Level;
    }

    public class Table
    {
        public List<Card> Cards;
    }

    private void Awake()
    {
        table = LitJson.JsonMapper.ToObject<Table>(Resources.Load<TextAsset>("Table").text); 
        // Debug.Log($"Card Count : {table.Cards.Count}");
        
        CreateCardDeck();

        CreateCardViewerFromTable();
        
        // Test For Call Twice
        //CreateCardViewerFromTable();

        m_tweenController = new TweenController();
    }

    private void CreateCardDeck()
    {
        if (m_cardDeckRectTransform != null)
        {
            return;
        }
        GameObject cardDeckObj = new GameObject("CardArea");
        m_cardDeckRectTransform = cardDeckObj.AddComponent<RectTransform>();
        
        m_cardDeckRectTransform.SetParent(transform);
        m_cardDeckRectTransform.localScale = Vector3.one;
        m_cardDeckRectTransform.anchorMin = Vector2.zero;
        m_cardDeckRectTransform.anchorMax = Vector2.one;
        m_cardDeckRectTransform.offsetMin = Vector2.zero;
        m_cardDeckRectTransform.offsetMax = Vector2.zero;
    }

    private void CreateCardViewerFromTable()
    {
        if (m_cardViewers == null)
        {
            m_cardViewers = new List<CardViewer>();
        }
        
        if (m_cardViewers.Count > 0)
        {
            foreach (var cardViewer in m_cardViewers)
            {
                if (cardViewer == null)
                {
                    continue;
                }
                
                cardViewer.gameObject.SafeDestroy();
            }
            m_cardViewers.Clear();
        }
        
        foreach (var card in table.Cards)
        {
            if (card == null)
            {
                continue;
            }
            CardViewer viewer = CardViewer.Create(m_cardDeckRectTransform, card);
            m_cardViewers.Add(viewer);
        }
    }

    public void OnButton1Click()
    {
        m_tweenController.Play(m_cardViewers, m_area1, m_duration, m_cardMovingIntervalTime, m_isEnableDuplicatedCall, TweenController.TweenType.Immediately);
    }
    
    public void OnButton2Click()
    {
        m_tweenController.Play(m_cardViewers, m_area2, m_duration, m_cardMovingIntervalTime, m_isEnableDuplicatedCall, TweenController.TweenType.Immediately);
    }
    
    public void OnButton3Click()
    {
        m_tweenController.Play(m_cardViewers, m_area1, m_duration, m_cardMovingIntervalTime, m_isEnableDuplicatedCall, TweenController.TweenType.Queueing);
    }
    
    public void OnButton4Click()
    {
        m_tweenController.Play(m_cardViewers, m_area2, m_duration, m_cardMovingIntervalTime, m_isEnableDuplicatedCall, TweenController.TweenType.Queueing);
    }
}
