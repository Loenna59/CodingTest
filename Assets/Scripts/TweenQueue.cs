using System.Collections.Generic;

public class TweenQueue : Tween
{
    private struct TweenData
    {
        private LTSeq m_sequence;
        public LTSeq Sequence => m_sequence;
        
        private CardArea m_destination;
        public CardArea Destination => m_destination;

        public TweenData(LTSeq sequence, CardArea destination)
        {
            m_sequence = sequence;
            m_destination = destination;
        }
    }
    
    private bool m_isTweening;
    private Queue<TweenData> m_queue;
    
    public TweenQueue() : base()
    {
        m_isTweening = false;
        m_queue = new();
    }
    
    public override void Play(List<CardViewer> cardViewers, CardArea cardArea, float duration, float delay)
    {
        m_queue.Enqueue(new TweenData(LeanTween.sequence(), cardArea));
        
        if (m_isTweening)
        {
            return;
        }
        TryDequeue();
        
        //Local function
        void TryDequeue()
        {
            if (m_queue.Count == 0)
            {
                m_isTweening = false;
                return;
            }
            
            m_isTweening = true;
            TweenData data = m_queue.Dequeue();
            
            CreateSequence(data.Sequence);
            PlayInternal(cardViewers, data.Destination, duration, delay).append(duration - delay).append(TryDequeue);
        }
    }
}
