using System.Collections.Generic;

public class TweenImmediate : Tween
{
    public override void Play(List<CardViewer> cardViewers, CardArea cardArea, float duration, float delay)
    {
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
