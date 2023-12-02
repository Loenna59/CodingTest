using UnityEngine;
using UnityEngine.UI;

public class CardViewer : MonoBehaviour
{
    private static GameObject s_prefab;
    
    public Text NameText;
    public Text LevelText;
    public Image Image;

    private float m_heightInWorld;
    public float HeightInWorld => m_heightInWorld;
    
    public static void LoadPrefab()
    {
        // ResourceMgr에서 해야 하는 부분
        if (s_prefab != null)
        {
            return;
        }
        s_prefab = Resources.Load<GameObject>("Prefab/Card");
    }

    public static CardViewer Create(RectTransform parent, CodingTest.Card card)
    {
        if (card == null)
        {
            Debug.LogError("Card data 이상");
            return default(CardViewer);
        }

        return Create(parent, card.Name, card.Level, card.Image);
    }

    public static CardViewer Create(RectTransform parent, string name, int level, string image)
    {
        LoadPrefab();
        GameObject obj = GameObject.Instantiate(s_prefab, parent);
        if (obj == null)
        {
            Debug.LogError("Prefab이 존재하지 않습니다.");
            return default(CardViewer);
        }

        CardViewer viewer = obj.GetComponent<CardViewer>();
        if (viewer == null)
        {
            Debug.LogError("해당 Component가 존재하지 않습니다.");
            return default(CardViewer);
        }

        viewer.Set(name, level, image);

        return viewer;
    }

    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        m_heightInWorld = corners[1].y - corners[3].y;
    }

    private void Set(string name, int level, string image)
    {
        NameText.text = name;
        LevelText.text = $"Lv {level}";

        Image.sprite = Resources.Load<Sprite>($"Images/{image}");
    }
}
