using TMPro;
using UnityEngine;

//TODO: Adicionar label do atributo cor na cena;
//TODO: Adicionar BarChart com Eixo Z

public class BarChartManager : MonoBehaviour
{
    public Transform VariaveisVisuaisParent;
    public Material[] TemplateMaterials;

    public TextMeshPro ChartNameLabel;
    public TextMeshPro oLabelEixoX;
    public TextMeshPro oLabelEixoY;
    public TextMeshPro oLabelEixoZ;

    private int _qtdObjetos;
    private GameObject[] _elementosVisuais;
    private const int _TamanhoEixoX = 10;

    public void CriaBarChartSimples(
        string[] eixoX,
        float[] eixoY,
        string[] cor,
        string labelEixoX,
        string labelEixoY,
        string labelCor,
        string chartName)
    {
        if(!Utils.ArraysSaoDoMesmoTamanho(eixoX, eixoY, cor))
        {
            Debug.LogError("Os parâmetros não são do mesmo tamanho. Verifique e tente novamente!");
            return;
        }

        _qtdObjetos = eixoX.Length;

        float[] eixoXNormalizado = Utils.CalculaPosicaoBarras(_qtdObjetos, _TamanhoEixoX);
        float[] eixoYNormalizado = Utils.NormalizaValoresComMultiplicador(eixoY, _TamanhoEixoX);
        int[] corNormalizado = Utils.ConverteCategoriasParaNumerico(cor);

        _elementosVisuais = new GameObject[_qtdObjetos];
        float espessura = Utils.CalculaEspessuraGameObject(_qtdObjetos, _TamanhoEixoX);
        GameObject empty = new GameObject();

        for (int i = 0; i < _qtdObjetos; i++)
        {
            //Instancia GO vazio
            _elementosVisuais[i] = Instantiate(original: empty,
                parent: VariaveisVisuaisParent,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity);
            _elementosVisuais[i].AddComponent<Barra>();
            _elementosVisuais[i].name = $"Row {i}";

            //Define a localposition do objeto
            _elementosVisuais[i].transform.localPosition = new Vector3(
                eixoXNormalizado[i], eixoYNormalizado[i], 0);

            //Adiciona informacoes da base de dados ao objeto
            _elementosVisuais[i].GetComponent<Barra>().SetAtributosBase(
                eixoX[i], eixoY[i], cor[i]); 

            //Cria barra com valores necessários pro Unity
            _elementosVisuais[i].GetComponent<Barra>().SetAtributosGameObject(
               eixoXNormalizado[i], espessura, eixoYNormalizado[i], TemplateMaterials[corNormalizado[i]]);

        }

        // Define label dos eixos
        oLabelEixoX.text = labelEixoX;
        oLabelEixoY.text = labelEixoY;

        if (chartName == "")
            ChartNameLabel.text = labelEixoX + " X " + labelEixoY + " X " + labelCor;
        else
            ChartNameLabel.text = chartName;
    }

    public void CriaBarChartSimples(
        string[] eixoX,
        float[] eixoY,
        string labelEixoX,
        string labelEixoY,
        string chartName,
        Material cor
        )
    {
        if (!Utils.ArraysSaoDoMesmoTamanho(eixoX, eixoY))
        {
            Debug.LogError("Os parâmetros não são do mesmo tamanho. Verifique e tente novamente!");
            return;
        }

        _qtdObjetos = eixoX.Length;

        float[] EixoXNormalizado = Utils.CalculaPosicaoBarras(_qtdObjetos, _TamanhoEixoX);
        float[] EixoYNormalizado = Utils.NormalizaValoresComMultiplicador(eixoY, _TamanhoEixoX);

        _elementosVisuais = new GameObject[_qtdObjetos];
        float espessura = Utils.CalculaEspessuraGameObject(_qtdObjetos, _TamanhoEixoX);
        GameObject empty = new GameObject();

        for (int i = 0; i < _qtdObjetos; i++)
        {
            //Instancia GO vazio
            _elementosVisuais[i] = Instantiate(original: empty,
                parent: VariaveisVisuaisParent,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity);
            _elementosVisuais[i].AddComponent<Barra>();
            _elementosVisuais[i].name = $"Row {i}";

            //Define a localposition do objeto
            _elementosVisuais[i].transform.localPosition = new Vector3(
                EixoXNormalizado[i], EixoYNormalizado[i], 0);

            //Adiciona informacoes da base de dados ao objeto
            _elementosVisuais[i].GetComponent<Barra>().SetAtributosBase(
                eixoX[i], eixoY[i]);

            //Cria barra com valores necessários pro Unity
            _elementosVisuais[i].GetComponent<Barra>().SetAtributosGameObject(
               EixoXNormalizado[i], espessura, EixoYNormalizado[i], cor);

        }

        // Define label dos eixos
        oLabelEixoX.text = labelEixoX;
        oLabelEixoY.text = labelEixoY;

        if (chartName == "")
            ChartNameLabel.text = labelEixoX + " X " + labelEixoY;
        else
            ChartNameLabel.text = chartName;
    }


    // remove on deploy
    void Start()
    {
        string[] x = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o" };
        float[] y = new float[] { 6.3F, 2.5F, 3.8F, 7.5F, 4.3F, 1.9F, 9.2F, 8.2F, 8.4F, 4.9F, 9.2F, 3.5F, 2.3F, 7.0F, 6.6F };
        string[] cor = new string[] {"azul", "vermelho", "verde", "laranja",
            "azul", "marrom", "marrom", "preto", "vermelho", "lilas",
            "vermelho", "laranja", "preto", "branco", "preto"
        };

        CriaBarChartSimples(x, y, cor, "Marca", "Custo", "País", "Custo por Marca e País");
    }
}
