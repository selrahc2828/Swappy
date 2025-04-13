using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapeScroll : MonoBehaviour
{
    public RectTransform content;
    public float itemHeight = 100f; // hauteur de chaque item
    public int visibleItemCount = 5; // combien sont visibles
    public List<string> data = new List<string>(); // les données à afficher

    private List<RectTransform> items = new List<RectTransform>();
    private int topIndex = 0;

    void Start()
    {
        // Création initiale des items visibles
        for (int i = 0; i < visibleItemCount + 2; i++) // +2 pour éviter les glitchs
        {
            GameObject go = new GameObject("Item " + i, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(content, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(content.rect.width, itemHeight);

            Text txt = go.GetComponent<Text>();
            txt.alignment = TextAnchor.MiddleCenter;
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.text = GetData(i);

            items.Add(rt);
        }
    }

    void Update()
    {
        float scrollPos = content.anchoredPosition.y;

        // Si le premier élément est sorti par le haut
        if (scrollPos > itemHeight)
        {
            // déplace le premier tout en bas
            RectTransform first = items[0];
            items.RemoveAt(0);
            items.Add(first);

            int newIndex = (topIndex + items.Count) % data.Count;
            first.anchoredPosition -= new Vector2(0, items.Count * itemHeight);
            first.GetComponent<TextMeshProUGUI>().text = GetData(newIndex);

            topIndex = (topIndex + 1) % data.Count;
            content.anchoredPosition -= new Vector2(0, itemHeight);
        }

        // Tu peux faire pareil pour le bas si tu veux un vrai aller-retour fluide
    }

    string GetData(int index)
    {
        if (data.Count == 0) return "";
        return data[index % data.Count];
    }
}
