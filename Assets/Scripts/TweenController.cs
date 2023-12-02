using System.Collections.Generic;
using UnityEngine;

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
        if (m_tweenImmediate == null)
        {
            m_tweenImmediate = new TweenImmediate();
        }

        if (m_tweenQueue == null)
        {
            m_tweenQueue = new TweenQueue();
        }
    }
    
    public void Play(List<CardViewer> cardViewers, CardArea destArea, float duration, float delay, TweenType tweenType)
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
        }
        m_currentTween.Play(cardViewers, destArea, duration, delay);
    }
}
