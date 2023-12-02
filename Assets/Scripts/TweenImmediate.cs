using System.Collections.Generic;

public class TweenImmediate : Tween
{
    public override void Play(List<CardViewer> cardViewers, CardArea cardArea, float duration, float delay, bool isEnableDuplicatedCall = false)
    {
        if (!isEnableDuplicatedCall)
        {
            if (m_currentArea == cardArea)
            {
                return;
            }
        }
        m_currentArea = cardArea;
        
        if (Sequence != null)
        {
            if (LeanTween.isTweening())
            { 
                Cancel(delay);
            }
        }
        
        base.Play(cardViewers, cardArea, duration, delay);
    }
}
