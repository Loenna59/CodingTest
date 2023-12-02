using System.Collections.Generic;

public class Tween
{
    protected LTSeq m_sequence;
    protected List<int> m_uniqueIds;
    protected CardArea m_currentArea; //중복 위치 이동 방지

    public Tween()
    {
        m_uniqueIds = new();
    }

    public void Cancel(float delay)
    {
        m_sequence.current.totalDelay = 0f;
            
        for (int i = 0, count = m_uniqueIds.Count; i < count; i++)
        {
            int uniqueId = m_uniqueIds[i];
            LeanTween.delayedCall(
                i * delay, 
                () => LeanTween.cancel(uniqueId)
            );
        }
            
        m_uniqueIds.Clear();
    }

    public virtual void Play(List<CardViewer> cardViewers, CardArea destArea, float duration, float delay, bool isEnableDuplicatedCall = false)
    {
        CreateSequence(LeanTween.sequence());
        PlayInternal(cardViewers, destArea, duration, delay);
    }

    protected virtual LTSeq PlayInternal(List<CardViewer> cardViewers, CardArea destArea, float duration, float delay)
    {
        int index = 0;
        foreach (var cardViewer in cardViewers)
        {
            if (ObjectUtility.IsNullOrDestroyed(cardViewer))
            {
                continue;
            }
            
            LTDescr descr = LeanTween.moveLocal(cardViewer.gameObject, destArea.GetPosition(cardViewer.Height, cardViewers.Count, index), duration);
            m_uniqueIds.Add(descr.uniqueId);
            m_sequence.append(descr).append(delay - duration);
            index++;
        }

        return m_sequence;
    }

    protected LTSeq CreateSequence(LTSeq seq)
    {
        seq.reset();
        seq.current.totalDelay = 0f;
        m_sequence = seq;
        m_uniqueIds.Clear();
        return m_sequence;
    }
}
