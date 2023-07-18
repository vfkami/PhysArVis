using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationRenderer : MonoBehaviour
{
    public string _base64string;
    private void Start()
    {
        RenderOfBase64(_base64string);
    }

    public void RenderOfBytes(byte[] data)
    {
        Texture2D tex = new Texture2D(900, 465);
        tex.LoadImage(data);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 100f);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    public void RenderOfBase64(string base64str)
    {
        _base64string = base64str;
        byte[] Bytes = Convert.FromBase64String(_base64string);
        Texture2D tex = new Texture2D(900, 465);
        tex.LoadImage(Bytes);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 100f);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}
