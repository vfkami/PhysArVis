using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScatterplotManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public Material[] materials;
    public Transform parentVariavelVisual;

    public TextMeshPro oLabelEixoX;
    public TextMeshPro oLabelEixoY;
    public TextMeshPro oLabelEixoZ;

    public const int TamanhoEixoX = 10;

    private GameObject[] _elementosVisuais;
    private int _qtdObjetos;


    /* TODO: Adicionar validacao tamanho array prefabs/material com categorias recebidas grupo/cor
     * TODO: Adicionar Labels de cor/grupo
     * TODO: Adicionar interacao ao clicar com mouse no elemento
     */ 
    public void Cria3DScatterplot(
        float[] eixoX, 
        float[] eixoY, 
        float[] eixoZ, 
        string[] cor, 
        string[] grupo,
        string labelEixoX,
        string labelEixoY,
        string labelEixoZ,
        string labelCor,
        string labelGrupo)
    {
        if (!Utils.ArraysSaoDoMesmoTamanho(eixoX, eixoY, eixoZ, cor, grupo))
        {
            Debug.LogError("Os parâmetros não são do mesmo tamanho. Verifique e tente novamente!");
            return;
        }

        _qtdObjetos = eixoX.Length;
        _elementosVisuais = new GameObject[_qtdObjetos];

        float[] eixoXNormalizado = Utils.NormalizaValoresComMultiplicador(eixoX, TamanhoEixoX);
        float[] eixoYNormalizado = Utils.NormalizaValoresComMultiplicador(eixoY, TamanhoEixoX);
        float[] eixoZNormalizado = Utils.NormalizaValoresComMultiplicador(eixoZ, TamanhoEixoX);
        int[] corNormalizado = Utils.ConverteCategoriasParaNumerico(cor);
        int[] grupoNormalizado = Utils.ConverteCategoriasParaNumerico(grupo);

        GameObject empty = new GameObject();

        for (int i = 0; i < _qtdObjetos; i++)
        {
            //Instancia parent vazio
            _elementosVisuais[i] = Instantiate(original: empty,
                parent: parentVariavelVisual,
                position: new Vector3(0, 0, 0), 
                rotation: Quaternion.identity);

            //Define localizacao do parent
            _elementosVisuais[i].transform.localPosition = new Vector3(
                eixoXNormalizado[i], eixoYNormalizado[i], eixoZNormalizado[i]);

            //Adiciona configurações referente ao GameObject e a base de dados
            _elementosVisuais[i].AddComponent<VariavelVisual>();
            _elementosVisuais[i].GetComponent<VariavelVisual>().setAtributosBase(
                eixoX[i], eixoY[i], eixoZ[i], grupo[i], cor[i]);

            _elementosVisuais[i].GetComponent<VariavelVisual>().setAtributosGameObject(
                eixoXNormalizado[i], eixoYNormalizado[i], eixoZNormalizado[i],
                prefabs[grupoNormalizado[i]], materials[corNormalizado[i]]);
            
            _elementosVisuais[i].name = "Row " + i;
        }

        oLabelEixoX.text = labelEixoX;
        oLabelEixoY.text = labelEixoY;
        oLabelEixoZ.text = labelEixoZ;

        Destroy(empty);
    }

    public void Cria2DScatterplot(
       float[] eixoX,
       float[] eixoY,
       string[] cor,
       string[] grupo,
       string labelEixoX,
       string labelEixoY,
       string labelCor,
       string labelGrupo)
    {
        if (!Utils.ArraysSaoDoMesmoTamanho(eixoX, eixoY, cor, grupo))
        {
            Debug.LogError("Os parâmetros não são do mesmo tamanho. Verifique e tente novamente!");
            return;
        }

        _qtdObjetos = eixoX.Length;
        _elementosVisuais = new GameObject[_qtdObjetos];

        float[] EixoXNormalizado = Utils.NormalizaValoresComMultiplicador(eixoX, TamanhoEixoX);
        float[] EixoYNormalizado = Utils.NormalizaValoresComMultiplicador(eixoY, TamanhoEixoX);
        int[] CorNormalizado = Utils.ConverteCategoriasParaNumerico(cor);
        int[] GrupoNormalizado = Utils.ConverteCategoriasParaNumerico(grupo);

        GameObject empty = new GameObject();

        for (int i = 0; i < _qtdObjetos; i++)
        {
            //Instancia parent vazio
            _elementosVisuais[i] = Instantiate(original: empty,
                parent: parentVariavelVisual,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity);

            //Define localizacao do parent
            _elementosVisuais[i].transform.localPosition = new Vector3(
                EixoXNormalizado[i], EixoYNormalizado[i], 0);

            //Adiciona configurações referente ao GameObject e a base de dados
            _elementosVisuais[i].AddComponent<VariavelVisual>();
            _elementosVisuais[i].GetComponent<VariavelVisual>().setAtributosBase(
                eixoX[i], eixoY[i], grupo[i], cor[i]);

            _elementosVisuais[i].GetComponent<VariavelVisual>().setAtributosGameObject(
                EixoXNormalizado[i], EixoYNormalizado[i],
                prefabs[GrupoNormalizado[i]], materials[CorNormalizado[i]]);

            _elementosVisuais[i].name = "Row " + i;
        }

        oLabelEixoX.text = labelEixoX;
        oLabelEixoY.text = labelEixoY;

        Destroy(empty);
    }

    private void Start()
    {
        float[] X = new float[] { 5, 8, 3, 23, 41, 10, 49, 85, 100, 84, 91, 13, 32, 09, 84 };
        float[] Y = new float[] { 9, 2, 3, 7, 4, 1, 9, 8, 8, 4, 9, 3, 2, 7, 6 };
        float[] Z = new float[] { 6.3F, 2.5F, 3.8F, 7.5F, 4.3F, 1.9F, 9.2F, 8.2F, 8.4F, 4.9F, 9.2F, 3.5F, 2.3F, 7.0F, 6.6F };

        string[] COR = new string[] {"azul", "vermelho", "verde", "laranja", 
            "azul", "marrom", "marrom", "preto", "vermelho", "lilas",
            "vermelho", "laranja", "preto", "branco", "preto"
        };

        string[] GRUPO = new string[] {"a", "h", "c", "d", "g", "f", "c", 
            "e", "f", "b", "a", "f", "e", "b", "h"
        };

        if (gameObject.name.Contains("3D"))
            Cria3DScatterplot(X, Y, Z, COR, GRUPO, "Pontuação", "Desempenho", "Custo", "Marca", "País");

        else
            Cria2DScatterplot(X, Y, COR, GRUPO, "Pontuação", "Desempenho", "Marca", "País");
    }


    

}
