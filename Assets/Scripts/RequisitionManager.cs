using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequisitionManager : MonoBehaviour
{
    public DatasetManager dataserManager;
    public DatasetSelectorWidgetManager datasetWidget;
    public VisualizationRenderer visualization;
    private FiducialMarkerManager fiducialMarkerManager;
    public TestScenariosManager testScenariosManager;

    public string enderecoServidor = "localhost";
    public string porta = "3000";
    public string portaMiddleware = "5501";

    string respostaJson;

    // TODO: Adicionar validação de conexão com servidor
    void Start()
    {
        //GetDatasetsDisponiveis();
    }

    public void SetEnderecoServidor(string endereco)
    {
        enderecoServidor = "http://" + endereco;
        GetDatasetsDisponiveis();
    }

    public void GetDatasetsDisponiveis()
    {
        string uri = $"{enderecoServidor}:{porta}/info.html";
        StartCoroutine(GetRequest(uri, 1));
    }

    public void GetDatasetPorNome(string datasetName)
    {
        string uri = $"{enderecoServidor}:{porta}/metadata/{datasetName}";
        StartCoroutine(GetRequest(uri, 2));
    }

    public void GetDatasetPorNomeAtributosPreenchidos(string datasetName)
    {
        string uri = $"{enderecoServidor}:{porta}/metadata/{datasetName}";
        StartCoroutine(GetRequest(uri, 7));
    }

    public void RequestBarChart(string nomeDataset, string nomeEixoX, string nomeEixoY, string filter)
    {
        string request = $"chartgen.png?";
        string x = $"x={nomeEixoX}";
        string y = $"&y={nomeEixoY}";
        string chartType = $"&chart=barchartvertical";
        string title = $"&title={nomeEixoX} X {nomeEixoY}";
        string xLabel = $"&xlabel={nomeEixoX}";
        string yLabel = $"&xlabel={nomeEixoY}";
        string filterUri = $"&filter={filter}";

        string uri = $"{enderecoServidor}:{porta}/generate/{nomeDataset}/{request}{x}{y}{chartType}{title}{xLabel}{yLabel}{filterUri}";
        uri = uri.Replace(" ", "");
        StartCoroutine(GetRequest(uri, 5));
    }

    public void RequestBarChart(string nomeDataset, string nomeEixoX, string nomeEixoY, string nomeCor, string filter)
    {
        string request = $"chartgen.png?";
        string x = $"x={nomeEixoX}";
        string y = $"&y={nomeEixoY}";
        string chartType = $"&chart=barchartvertical";
        string title = $"&title={nomeEixoX} X {nomeEixoY}";
        string xLabel = $"&xlabel={nomeEixoX}";
        string yLabel = $"&xlabel={nomeEixoY}";
        string color = $"&color={nomeCor}";

        string filterUri = $"&filter={filter}";

        string uri = $"{enderecoServidor}:{porta}/generate/{nomeDataset}/{request}{x}{y}{color}{chartType}{title}{xLabel}{yLabel}{filterUri}";
        uri = uri.Replace(" ", "");
        StartCoroutine(GetRequest(uri, 5));
    }

    public void SendVisualizationConfigurationToArduino(string nomeDataset, string nomeEixoX, string nomeEixoY, string nomeCor, string filter)
    {
        string request = $"mainPayload";
        string endBody = $"?max=50&bars=6";
        //string filterUri = $"&filter={filter}";

        string uri = $"{enderecoServidor}:{portaMiddleware}/{request}/{nomeDataset}/{nomeEixoX}/{nomeEixoY}/{nomeCor}{endBody}";
        uri = uri.Replace(" ", "");

        Debug.Log(uri);
        StartCoroutine(GetRequest(uri, 999));
    }

    public void SendVisualizationConfigurationToArduino(string nomeDataset, string nomeEixoX, string nomeEixoY, string filter)
    {
        string request = $"mainPayload";
        string endBody = $"?max=50&bars=6&sort=true&random=false";
        //string filterUri = $"&filter={filter}";

        string uri = $"{enderecoServidor}:{portaMiddleware}/{request}/{nomeDataset}/{nomeEixoX}/{nomeEixoY}{endBody}";
        uri = uri.Replace(" ", "");

        Debug.Log(uri);
        StartCoroutine(GetRequest(uri, 999));
    }

    // TODO: Adicionar gerenciador/renderizador de subvisualizacoes
    // TODO: Adicionar gerenciador dos botões virtuais - marcadores
    public void RequestSubVisualization(string nomeDataset, string eixoSubVis, string continuoSubVis, string nomeEixoX, string filter, string nomeCategoria, int index)
    {
        string request = $"chartgen.png?";
        string x = $"x={eixoSubVis}";
        string y = $"&y={continuoSubVis}";
        string chartType = $"&chart=piechart";
        string title = $"&title={nomeCategoria.ToUpper()}_{continuoSubVis}_X_{eixoSubVis}";
        string xLabel = $"&xlabel={eixoSubVis}";
        string filterUri = $"&filter={filter}";

        string uri = $"{enderecoServidor}:{porta}/generate/{nomeDataset}/{request}{x}{y}{chartType}{title}{xLabel}{filterUri}";
        uri = uri.Replace(" ", "");

        StartCoroutine(GetRequest(uri, 6, index));

    }

    IEnumerator GetRequest(string uri, int operacao, int index = -1)
    {
        if (string.IsNullOrEmpty(enderecoServidor))
        {
            Debug.LogError("endereço do servidor não definido");
            throw new System.Exception();
        }

        Debug.Log(uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error: " + webRequest.error);
                    datasetWidget.AtualizaTextoCanvas(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    ResponseCallback(webRequest.downloadHandler, operacao, index);
                    break;
            }
        }
    }

    private void ResponseCallback(DownloadHandler data, int operacao, int index = -1)
    {
        switch (operacao)
        {
            case 1: // GET Lista de Datasets
                string[] splitedData = data.text.Split(',');
                datasetWidget.AtualizaOpcoesDropdownDataset(splitedData);
                break;

            case 2: // GET Metadados Dataset Selecionado 
                datasetWidget.AtualizaTextoCanvas(data.text);
                dataserManager.SetDataset(data.text);
                break;
            case 5: // GET Visualizacao BarChart
                byte[] response = data.data;
                visualization.RenderOfBytes(response);
                break;
            case 6:
                byte[] res = data.data;
                Sprite sprite = Utils.RenderOfBytes(res);
                if (index < 0)
                    Debug.LogWarning("Posição do sprite não definida. Não será possível garantir a ordem correspondente!");

                fiducialMarkerManager = FindObjectOfType<FiducialMarkerManager>();
                if (fiducialMarkerManager != null)
                    fiducialMarkerManager.AddNovaSubVisualizacao(sprite, index);
                
                break;
            case 7:
                datasetWidget.AtualizaTextoCanvas(data.text);
                dataserManager.SetDataset(data.text);
                testScenariosManager.DefineAtributosSelecionados();
                break;
            default:
                respostaJson = data.text;
                break;
        }

    }
}
