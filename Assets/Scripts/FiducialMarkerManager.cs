using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiducialMarkerManager : MonoBehaviour
{
    private static int NumeroMarcadores = 6;

    private Sprite[] sprites = new Sprite[NumeroMarcadores];
    private int _indexUltimoSpriteAdicionado;

    public GameObject modeloMarcador;
    public GameObject[] fiducialMarkers = new GameObject[NumeroMarcadores];

    public AugmentedImageBehavior augmentedImageBehavior;

    private void Start()
    {
        _indexUltimoSpriteAdicionado = 0;
    }

    public void SetTextoMarcadorPorIndex(string text, int index)
    {
        fiducialMarkers[index].GetComponent<FiducialMarkerController>().SetTexto(text);
    }

    public void SetSpriteMarcadorPorIndex(Sprite sprite, int index)
    {
        try
        {
            fiducialMarkers[index].GetComponent<FiducialMarkerController>().SetImage(sprite);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void SetTextoTodosMarcadores(string[] texts)
    {
        int count = 0;

        if (texts.Length > NumeroMarcadores)
        {
            Debug.LogWarning(
                $"A quantidade de texto passado excede a quantidade de marcadores na cena. Os textos excedentes serão ignorados");
            
            count = NumeroMarcadores;
        }
        else if (texts.Length < NumeroMarcadores)
        {
            Debug.LogWarning(
                $"A quantidade de texto passado é menor que a quantidade de marcadores na cena. Haverão marcadores sem texto");
            
            count = texts.Length;
        }

        for(int i=0; i < count; i++)
            SetTextoMarcadorPorIndex(texts[i], i);

        return;        
    }

    public void SetTextoTodosMarcadores(Sprite[] sprites)
    {
        int count = 0;

        if (sprites.Length > NumeroMarcadores)
        {
            Debug.LogWarning(
                $"A quantidade de texto passado excede a quantidade de marcadores na cena. Os textos excedentes serão ignorados");

            count = NumeroMarcadores;
        }
        else if (sprites.Length < NumeroMarcadores)
        {
            Debug.LogWarning(
                $"A quantidade de texto passado é menor que a quantidade de marcadores na cena. Haverão marcadores sem texto");

            count = sprites.Length;
        }

        for (int i = 0; i < count; i++)
            SetSpriteMarcadorPorIndex(sprites[i], i);

        return;
    }

    public void ResetListaSubvisualizacao()
    {
        _indexUltimoSpriteAdicionado = 0;
        sprites = new Sprite[NumeroMarcadores];
    }

    public void AddNovaSubVisualizacao(Sprite sp, int index)
    {
        if(_indexUltimoSpriteAdicionado >= NumeroMarcadores || index >= NumeroMarcadores)
        {
            Debug.LogError("Lista de Sprites cheia. Os proximos não serão adicionados");
            return;
        }

        SetSpriteMarcadorPorIndex(sp, index);
    }

    public void SetNumeroMarcadores(int value)
    {
        if (value == NumeroMarcadores) return;

        //NumeroMarcadores = value;
        //AtualizarQuantidadeMarcadores();
    }

    public void AtualizarQuantidadeMarcadores()
    {
        foreach (GameObject go in fiducialMarkers) Destroy(go);

        fiducialMarkers = new GameObject[NumeroMarcadores];
        
        for(int i=0; i < NumeroMarcadores; i++)
        {
            fiducialMarkers[i] = Instantiate(original: modeloMarcador, position: new Vector3(0, 0, 0), rotation: Quaternion.identity, parent: transform);
            fiducialMarkers[i].transform.localPosition = new Vector3((i * 2) - (NumeroMarcadores) , 0, 0);
        }

    }

    public void setZoomedImageSprite(Sprite sp)
    {
        augmentedImageBehavior.setSprite(sp);
    }

    


}
