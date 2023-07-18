using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineChartManager : MonoBehaviour
{
    private GameObject[] _elementosVisuais;
    private int _qtdObjetos;

    public Transform parentVariavelVisual;
    public Material[] materiais;
    public TextMeshPro oLabelEixoX;
    public TextMeshPro oLabelEixoY;
    //public TextMeshPro ZAxisLabel;

    public const int TamanhoEixoX = 10;

    public void CriaLineChartUnico(
        string[] eixoX,
        float[] eixoY,
        string labelEixoX,
        string labelEixoY)
    {
        if (!Utils.ArraysSaoDoMesmoTamanho(eixoX, eixoY))
        {
            Debug.LogError("Os parâmetros não são do mesmo tamanho. Verifique e tente novamente!");
            return;
        }

        _qtdObjetos = eixoX.Length;
        _elementosVisuais = new GameObject[_qtdObjetos];

        float[] eixoXNormalizado = Utils.CalculaPosicaoBarras(_qtdObjetos, TamanhoEixoX);
        float[] eixoYNormalizado = Utils.NormalizaValoresComMultiplicador(eixoY, TamanhoEixoX);
        //int[] CorNormalizado = Utils.ConverteCategoriasParaNumerico(cor);

        _elementosVisuais = new GameObject[_qtdObjetos];

        GameObject empty = new GameObject();

        for (int i = 0; i < _qtdObjetos; i++)
        {
            _elementosVisuais[i] = Instantiate(original: empty,
                parent: parentVariavelVisual,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity
            );

            _elementosVisuais[i].name = "Row " + i;
            
            _elementosVisuais[i].AddComponent<PontoLinha>();
            _elementosVisuais[i].GetComponent<PontoLinha>().SetAtributosBase(eixoX[i], eixoY[i]);
            _elementosVisuais[i].GetComponent<PontoLinha>().SetAtributosGameObject(
                eixoXNormalizado[i], eixoYNormalizado[i]);

            _elementosVisuais[i].GetComponent<PontoLinha>().CriaPonto(materiais[0]);


            if (i != _qtdObjetos -1)
                _elementosVisuais[i].GetComponent<PontoLinha>().CriaLinha(materiais[0]);

            if (i != 0)
            {
                _elementosVisuais[i - 1].GetComponent<PontoLinha>().SetProximoPonto(_elementosVisuais[i]);
                _elementosVisuais[i - 1].GetComponent<PontoLinha>().ConectaProximoPonto();
                _elementosVisuais[i - 1].GetComponent<PontoLinha>().RedefineEscalaLinha();
            }

        }

        oLabelEixoX.text = labelEixoX;
        oLabelEixoY.text = labelEixoY;
        //ZAxisLabel.text = labelEixoZ;

        Destroy(empty);
    }


    

    // Start is called before the first frame update
    void Start()
    {
        string[] X = new string[] { "azul", "vermelho", "verde", "laranja",
            "azul", "marrom", "marrom", "preto", "vermelho", "lilas", "2019", "abbv",
            "alka", "kjdasn", "ldkasjd"};

        float[] Y = new float[] { 2, 3, 4, 3, 4, 5, 2, 3, 4, 2, 2, 3, 5, 1, 5};

        //string[] COR = new string[] {"azul", "vermelho", "verde", "laranja",
        //"azul", "marrom", "marrom", "preto", "vermelho", "lilas"
        //};

        CriaLineChartUnico(X, Y, "Producao", "Custo");

        Vector2[][] matriz = new Vector2[2][];

        matriz[0] = new Vector2[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 3) };

    }


    

}
