using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class CodingTest : MonoBehaviour
{
    private Table table;
    
    [SerializeField]
    private RectTransform area1;
    
    [SerializeField]
    private RectTransform area2;
    
    private GameObject cardPrefab;
    private List<CardViewer> cardViewers;
    private RectTransform cardArea;

    private Queue<Tween> queue;
    private Tween currentTween;
    private bool isTweening;
    
    public class Card
    {
        public string Name, Image;
        public int Level;
    }

    public class Table
    {
        public List<Card> Cards;
    }

    public class Tween
    {
        public LTSeq Sequence;
        public Transform Destination;
        public float Duration;

        public int UniqueId => Sequence.tween.uniqueId;

        public void Init()
        {
            Sequence.reset();
            Sequence.current.totalDelay = 0f;
        }

        public void Append(GameObject target, float toPosY, float delay)
        {
            Sequence.append(target.LeanMove(new Vector2(Destination.position.x, toPosY), Duration)).append(delay);
        }
    }

    void Awake()
    {
        isTweening = false;
        queue = new();
        cardPrefab = Resources.Load<GameObject>("Prefab/Card");
        cardViewers = new();
        table = LitJson.JsonMapper.ToObject<Table>(Resources.Load<TextAsset>("Table").text); 
        // Debug.Log($"Card Count : {table.Cards.Count}");

        GameObject cardAreaGameObject = new GameObject("CardArea");
        cardArea = cardAreaGameObject.AddComponent<RectTransform>();
        cardArea.SetParent(transform);
        cardArea.localScale = Vector3.one;
        cardArea.anchorMin = Vector2.zero;
        cardArea.anchorMax = Vector2.one;
        cardArea.offsetMin = Vector2.zero;
        cardArea.offsetMax = Vector2.zero;
        
        foreach (var card in table.Cards)
        {
            // Debug.Log($"- Card : {card.Name} (Lv. {card.Level})");
            GameObject instance = GameObject.Instantiate(cardPrefab, cardArea);
            CardViewer cardViewer = instance.GetComponent<CardViewer>();
            cardViewer.Set(card.Name, card.Level, card.Image);
            
            cardViewers.Add(cardViewer);
        }
    }

    public void OnButton1Click()
    {
        PlayTween(area1, true);
    }
    
    public void OnButton2Click()
    {
        PlayTween(area2, true);
    }
    
    public void OnButton3Click()
    {
        PlayTween(area1, false);
    }
    
    public void OnButton4Click()
    {
        PlayTween(area2, false);
    }

    private void PlayTween(Transform to, bool immediately)
    {
        if (immediately && LeanTween.isTweening())
        {
            if (currentTween != null)
            {
                LeanTween.cancel(currentTween.UniqueId);
            }

            isTweening = false;
            queue.Clear();
        }

        queue.Enqueue(
            new Tween
            {
                Sequence = LeanTween.sequence(),
                Destination = to,
                Duration = 0.75f
            }
        );

        if (!isTweening)
        {
            Dequeue();
        }
    }

    private void Dequeue()
    {
        if (queue.Count == 0)
        {
            currentTween = null;
            isTweening = false;
            return;
        }
        
        Tween next = queue.Dequeue();
        isTweening = true;
        
        // 같은 버튼 연속 클릭 방지
        if (next.Destination == currentTween?.Destination)
        {
            Dequeue();
            return;
        }
        
        currentTween = next;
        currentTween.Init();
        
        foreach (var (card, index) in cardViewers.Select((viewer, index) => (viewer.gameObject, index)))
        {
            currentTween.Append(card, Screen.height - 50f * (index + 2), -0.5f);
        }
        
        currentTween.Sequence.append(0.5f).append(Dequeue);
    }
    
}
