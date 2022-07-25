using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CodingTest : MonoBehaviour
{
    private Table table;
    public class Card
    {
        public string Name, Image;
        public int Level;
    }

    public class Table
    {
        public List<Card> Cards;
    }

    void Awake()
    {
        table = LitJson.JsonMapper.ToObject<Table>(Resources.Load<TextAsset>("Table").text); 
        Debug.Log($"Card Count : {table.Cards.Count}");
        foreach (var card in table.Cards)
            Debug.Log($"- Card : {card.Name} (Lv. {card.Level})");
    }

    public void OnButton1Click()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    
    public void OnButton2Click()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    
    public void OnButton3Click()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    
    public void OnButton4Click()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
}
