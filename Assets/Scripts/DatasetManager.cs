using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatasetManager : MonoBehaviour
{
    static private Dataset _dataset;

    static private string _nomeDataset;
    static private string _nomeEixoX;
    static private string _nomeEixoY;
    static private string _nomeCor;
    static private string _nomeAtributoSubVisualizacao;
    static private string _nomeAtributoContinuoSubVisualizacao;

    static private int _indexAtributoSubVisualizacao;
    static private int _indexAtributoContinuoSubVisualizacao;


    static private string[] _eixoX;
    static private string[] _eixoY;
    static private string[] _cor;

    static private string _filtrosUri;
    static private string[] _filtrosUriSubVisualizacao;

    public AxisConfigurationWidgetManager _axisWidget;
    public FilterConfigurationWidgetManager _filterWidget;
    public RequisitionManager _requisitionManager;
    private HeightBarBehavior _heightBarBehavior;

    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
    }
    public void SetDataset(string json)
    {

        try
        {
            _dataset = Utils.CriaDoJSON(json);
        }
        catch (ArgumentException ex)
        {
            _dataset = new Dataset()
            {
                columns = new string[0],
                rows = 0,
                meta = new Metadata[0]
            };

            Debug.LogError(ex.Message);
        }

        AtualizaElementosCanvas();
    }

    public static void SetNomeDataset(string nome)
    {
        _nomeDataset = nome;
    }

    public static void SetNomeEixoX(string eixo)
    {
        _nomeEixoX = eixo;
    }

    public static void SetNomeEixoY(string eixo)
    {
        _nomeEixoY = eixo;
        GameObject.Find("SceneManager").GetComponent<DatasetManager>().ConfiguraBarraAltura(eixo);
    }

    public void ConfiguraBarraAltura(string eixo)
    {
        var index = _filterWidget.GetIndexAtributoPorNome(eixo);
        var range = _filterWidget.GetRangeNumerico(index);

        Vector3 values = Vector3.zero;
        if (range != null)
            values = new Vector3(range.x, (float)Math.Round((range.x + range.y) / 2), range.y);

        _heightBarBehavior = FindObjectOfType<HeightBarBehavior>();

        if (_heightBarBehavior != null)
            _heightBarBehavior.DefineTextoEixoY(values);
    }

    public static void SetNomeCor(string cor)
    {
        _nomeCor = cor;
    }

    public static void SetNomeAtributoSubVisualizacao(int indexAtributo, string nomeAtributo)
    {
        _nomeAtributoSubVisualizacao = nomeAtributo;
        _indexAtributoSubVisualizacao = indexAtributo;
    }

    public static void SetNomeAtributoContinuoSubVisualizacao(int indexAtributo, string nomeAtributo)
    {
        _nomeAtributoContinuoSubVisualizacao = nomeAtributo;
        _indexAtributoContinuoSubVisualizacao = indexAtributo;
    }

    public static void SetNomeAtributoContinuoSubVisualizacao(string nomeAtributo)
    {
        _nomeAtributoContinuoSubVisualizacao = nomeAtributo;
    }


    public static void SetNomeAtributoSubVisualizacao(string nomeAtributo)
    {
        _nomeAtributoSubVisualizacao = nomeAtributo;
    }

    public void AtualizaElementosCanvas()
    {
        _filterWidget.gameObject.SetActive(true);
        _axisWidget.gameObject.SetActive(true);

        _filterWidget.SetLabelsFiltro(_dataset.meta
            .Select(i => i.name.ToString()).ToArray());

        _filterWidget.SetTipoFiltros(_dataset.meta
            .Select(i => i.type.ToString()).ToArray());

        _filterWidget.SetInfoFiltros(_dataset.meta
            .Select(i => i.extent.ToArray<string>()).ToArray());

        List<string> categoricLabels = new List<string>();
        List<string> numericLabels = new List<string>();


        for (int i = 0; i < _dataset.meta.Length; i++)
        {
            var column = _dataset.meta[i];

            if (column.type.Equals("categorical"))
            {
                categoricLabels.Add(column.name);
            }
            if (column.type.Equals("numeric"))
            {
                numericLabels.Add(column.name);
            }
        }

        _axisWidget.SetLabelsAtributoEixoX(categoricLabels.ToArray());
        _axisWidget.SetLabelsSubVisualization(categoricLabels.ToArray());
        _axisWidget.SetLabelsAtributoCor(categoricLabels.ToArray());

        _axisWidget.SetLabelsAtributoEixoY(numericLabels.ToArray());

        _filterWidget.gameObject.SetActive(false);
        _axisWidget.gameObject.SetActive(false);
    }

    public void RequestVisualization()
    {
        //Passo 1: Reunir dados da base
        if (string.IsNullOrEmpty(_nomeDataset))
        {
            Debug.LogError("Nenhum dataset selecionado. Escolha um e tente novamente!");
            return;
        }

        if (_dataset == null)
        {
            Debug.LogError("O dataset retornou nulo. Selecione outro dataset ou tente novamente!");
            return;
        }

        // Passo 2: Reunir dados do eixo x e y
        if (string.IsNullOrEmpty(_nomeEixoX) || string.IsNullOrEmpty(_nomeEixoX))
        {
            Debug.LogError("Um dos eixos não foi definido. Use o menu e selecione um dos atributos disponíveis!");
            return;
        }

        // Passo 3: Reunir informacao dos filtros
        _filtrosUri = _filterWidget.GetFiltrosConfigurados();

        // Passo 4: Reunir dados da cor (opcional) - se cor não selecionado, faz requisicao sem cor
        if (string.IsNullOrEmpty(_nomeCor))
        {
            Debug.LogWarning("O atributo referente à cor não foi definido. A requisição será enviada assim mesmo!");

            _requisitionManager.RequestBarChart(
                nomeDataset: _nomeDataset,
                nomeEixoX: _nomeEixoX,
                nomeEixoY: _nomeEixoY,
                filter: _filtrosUri
            );
            return;
        }

        _requisitionManager.RequestBarChart(
            nomeDataset: _nomeDataset,
            nomeEixoX: _nomeEixoX,
            nomeEixoY: _nomeEixoY,
            nomeCor: _nomeCor,
            filter: _filtrosUri
        );

        return;

    }

    public void SendToArduino()
    {
        //Passo 1: Reunir dados da base
        if (string.IsNullOrEmpty(_nomeDataset))
        {
            Debug.LogError("Nenhum dataset selecionado. Escolha um e tente novamente!");
            return;
        }

        if (_dataset == null)
        {
            Debug.LogError("O dataset retornou nulo. Selecione outro dataset ou tente novamente!");
            return;
        }

        // Passo 2: Reunir dados do eixo x e y
        if (string.IsNullOrEmpty(_nomeEixoX) || string.IsNullOrEmpty(_nomeEixoX))
        {
            Debug.LogError("Um dos eixos não foi definido. Use o menu e selecione um dos atributos disponíveis!");
            return;
        }

        // Passo 3: Reunir informacao dos filtros
        _filtrosUri = _filterWidget.GetFiltrosConfigurados();

        // Passo 4: Reunir dados da cor (opcional) - se cor não selecionado, faz requisicao sem cor
        if (string.IsNullOrEmpty(_nomeCor))
        {
            Debug.LogWarning("O atributo referente à cor não foi definido. A requisição será enviada assim mesmo!");

            _requisitionManager.SendVisualizationConfigurationToArduino(
                nomeDataset: _nomeDataset,
                nomeEixoX: _nomeEixoX,
                nomeEixoY: _nomeEixoY,
                filter: _filtrosUri
            );
            return;
        }

        _requisitionManager.SendVisualizationConfigurationToArduino(
            nomeDataset: _nomeDataset,
            nomeEixoX: _nomeEixoX,
            nomeEixoY: _nomeEixoY,
            nomeCor: _nomeCor,
            filter: _filtrosUri
        );

        return;

    }

    public void RequestSubVisualization()
    {
        //Passo 1: Reunir dados da base
        if (string.IsNullOrEmpty(_nomeDataset))
        {
            Debug.LogError("Nenhum dataset selecionado. Escolha um e tente novamente!");
            return;
        }

        if (_dataset == null)
        {
            Debug.LogError("O dataset retornou nulo. Selecione outro dataset ou tente novamente!");
            return;
        }

        // Passo 2: Reunir dados do eixo x e y
        if (string.IsNullOrEmpty(_nomeEixoX) || string.IsNullOrEmpty(_nomeEixoX))
        {
            Debug.LogError("Um dos eixos não foi definido. Use o menu e selecione um dos atributos disponíveis!");
            return;
        }

        //Passo 3: Verificar se todos os dados da subvisualizacao estao preechidos
        if (string.IsNullOrEmpty(_nomeAtributoSubVisualizacao))
        {
            Debug.LogError("O atributo da Sub Visualização não está definido!");
            return;
        }

        _filtrosUriSubVisualizacao = _filterWidget
            .GetFiltrosConfiguradosParaSubvisualizacao(_nomeEixoX);

        //Passo 4: Verificar se o valor dos filtros voltou ok
        if (_filtrosUriSubVisualizacao == null)
        {
            Debug.LogError("Houve algum erro ao gerar a string das subvisualizações. A requisição não será feita!");
            return;
        }
        // Passo 5: Obter label das categorias para enviar para o gráfico

        int indexEixoX = _filterWidget.GetIndexAtributoPorNome(_nomeEixoX);
        string[] atributosCategoricosLabels = _filterWidget.GetLabelAtributosCategoricos(indexEixoX);

        int loopTimes = atributosCategoricosLabels.Length <= _filtrosUriSubVisualizacao.Length ? atributosCategoricosLabels.Length : _filtrosUriSubVisualizacao.Length;

        for (int i = 0; i < loopTimes; i++)
        {
            _requisitionManager.RequestSubVisualization(
            nomeDataset: _nomeDataset,
            nomeEixoX: _nomeEixoX,
            eixoSubVis: _nomeAtributoSubVisualizacao,
            continuoSubVis: _nomeAtributoContinuoSubVisualizacao,
            filter: _filtrosUriSubVisualizacao[i],
            nomeCategoria: atributosCategoricosLabels[i],
            index: i
            );
        }


    }
}

[System.Serializable]
public class Dataset
{
    public string[] columns;
    public int rows;
    public Metadata[] meta;
}

[System.Serializable]
public class Metadata
{
    public string name;
    public string type;
    public string[] extent;
}
