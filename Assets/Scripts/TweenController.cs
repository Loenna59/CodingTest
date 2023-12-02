using System;
using System.Collections.Generic;

public class TweenController
{
    public enum TweenType
    {
        Immediately,
        Queueing,
    }
    
    private TweenImmediate m_tweenImmediate;
    private TweenQueue m_tweenQueue;
    
    // 현재 처리중인 Tween
    private Tween m_currentTween;

    public TweenController()
    {
        CreateTween();
    }
    
    private void CreateTween()
    {
        m_tweenImmediate ??= new TweenImmediate();
        m_tweenQueue ??= new TweenQueue();
    }
    
    public void Play(List<CardViewer> cardViewers, CardArea destArea, float duration, float delay, bool isEnableDuplicatedCall, TweenType tweenType)
    {
        switch (tweenType)
        {
            case TweenType.Immediately:
                if (m_currentTween != null)
                {
                    if (m_currentTween is not TweenImmediate)
                    {
                        m_currentTween.Cancel(delay);
                    }
                }
                m_currentTween = m_tweenImmediate;
                break;
            case TweenType.Queueing:
                if (m_currentTween != null)
                {
                    if (m_currentTween is not TweenQueue)
                    {
                        m_currentTween.Cancel(delay);
                    }
                }
                m_currentTween = m_tweenQueue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tweenType), tweenType, null);
        }
        m_currentTween.Play(cardViewers, destArea, duration, delay, isEnableDuplicatedCall);
    }
}
