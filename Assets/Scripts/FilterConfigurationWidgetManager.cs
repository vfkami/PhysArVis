using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;
using System.Linq;

public class FilterConfigurationWidgetManager : MonoBehaviour
{
    private static string Categoric = "categorical";
    private static string Numeric = "numeric";

    private GameObject[] _filtrosGameObject;
    private Filtro[] _filtros;
    private string[] _labelFiltros;
    private string[] _tipoFiltros;
    private string[][] _informacaoFiltros;
    private string _uri;
    private string[] _uriSubVisualization;
    
    public TMP_Dropdown dpdSeletorFiltros;

    public GameObject ancoraFiltros;
    public GameObject templateCategorico;
    public GameObject templateNumerico;


    public void SetLabelsFiltro(string[] labels)
    {
        if (_filtrosGameObject != null)
            _filtrosGameObject.Where(go => go != null).ToList()
                .ForEach(go => Destroy(go));

        _labelFiltros = labels;
        _filtrosGameObject = new GameObject[labels.Length];

        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("Selecione");
        newOptions.Add(option);

        foreach (string label in labels)
        {
            option = new TMP_Dropdown.OptionData(label);
            newOptions.Add(option);
        }

        dpdSeletorFiltros.ClearOptions();
        dpdSeletorFiltros.AddOptions(newOptions);
    }

    // accept only: categoric, numeric
    public void SetTipoFiltros(string[] tipos)
    {
        _tipoFiltros = tipos;
    }

    public void SetInfoFiltros(string[][] infos)
    {
        _informacaoFiltros = infos;
    }

    public string[] GetLabelAtributosCategoricos(int index)
    {
        if (!_tipoFiltros[index].Equals(Categoric))
        {
            Debug.LogError(
                $"O atributo passado no índice " + index + " não corresponde a um atributo categórico");
            return new string[0];

        }

        return _informacaoFiltros[index];
    }

    public int GetIndexAtributoPorNome(string nomeAtributo)
    {
        try
        {
            return Array.FindIndex(_labelFiltros, f => f.Contains(nomeAtributo));
        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning($"{ex.Message} - Atributo {nomeAtributo} não encontrado!");
            return -1;
        }
    }
    public Vector2 GetRangeNumerico(int index)
    {
        if (!_tipoFiltros[index].Equals(Numeric))
        {
            Debug.LogError(
                $"O atributo passado no índice " + index + " não corresponde a um atributo categórico");
            return new Vector2(0, 0);
        }

        var extent = _informacaoFiltros[index];

        float.TryParse(extent[0], out float min);
        float.TryParse(extent[1], out float max);

        return new Vector2(min, max);
    }

    public void OnFiltroSelectorValueChanged()
    {
        _filtrosGameObject.Where(go => go != null).ToList()
            .ForEach(go => go.SetActive(false));

        var index = dpdSeletorFiltros.value - 1;
        if (index < 0) return;

        if (_filtrosGameObject[index] == null)
        {        
            if (_tipoFiltros[index].Contains("categoric"))
            {
                _filtrosGameObject[index] = Instantiate(original: templateCategorico,
                parent: ancoraFiltros.transform,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity
                );

                _filtrosGameObject[index].name = _labelFiltros[index] + "_cat_" + index;
                _filtrosGameObject[index].transform.localPosition = Vector3.zero;
                _filtrosGameObject[index].GetComponent<CategoricFilterConfiguration>().
                    SetOptions(GetLabelAtributosCategoricos(index));
            }
            else
            {
                _filtrosGameObject[index] = Instantiate(original: templateNumerico,
                parent: ancoraFiltros.transform,
                position: new Vector3(0, 0, 0),
                rotation: Quaternion.identity
                );

                _filtrosGameObject[index].name = _labelFiltros[index] + "_num_" + index;
                _filtrosGameObject[index].transform.localPosition = Vector3.zero;
                _filtrosGameObject[index].GetComponent<NumericFilterConfiguration>().
                    SetRange(GetRangeNumerico(index));
            }
        }

        _filtrosGameObject[index].SetActive(true);
    }

    public bool HasFiltroValido()
    {
        return _filtrosGameObject.Any(filtro => filtro != null);
    }

    public string GetFiltrosConfigurados()
    {
        if (!HasFiltroValido()) return "";       

        _filtros = new Filtro[_labelFiltros.Length];

        for (int i = 0; i < _labelFiltros.Length; i++)
        {
            CategoricFilterConfiguration catConf;
            NumericFilterConfiguration numConf;

            if(_filtrosGameObject[i] != null)
            {
                _filtrosGameObject[i].TryGetComponent<CategoricFilterConfiguration>(out catConf);
                _filtrosGameObject[i].TryGetComponent<NumericFilterConfiguration>(out numConf);

                if (catConf != null)
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], catConf.GetValores());

                    var filtroStringfy = 
                        String.Join("," , _filtros[i].values.ToList().Select(str => "\"" + str + "\"").ToList());

                    _filtros[i].uri = $"{{\"field\": \" {_filtros[i].nome} \"," +
                        $"\"oneOf\": [ { filtroStringfy} ]}}";
                      
                    continue;
                }
                else if (numConf != null)
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], numConf.GetValores());
                    
                    _filtros[i].uri = $"{{\"field\": \" {_filtros[i].nome} \", " +
                        $"\"range\":[ {_filtros[i].values[0] }, {_filtros[i].values[1] } ]}}";

                    if (Convert.ToBoolean(_filtros[i].values[2]))
                        _filtros[i].uri = $"{{\"not\": {_filtros[i].uri} }}";

                    continue;
                }
                else
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], new string[0]);
                    _filtros[i].uri = "";
                }
            }
        }


        string uriUnified = string.Join(",",
            _filtros.Where(go => go != null && !string.IsNullOrEmpty(go.uri))
                    .ToList()
                    .Select(go => go.uri)
                    .ToArray());

        _uri = $"[" +
               $"{uriUnified}" +
               $"]";

        return _uri;
    }

    public string[] GetFiltrosConfiguradosParaSubvisualizacao(string nomeAtributo)
    {
        _filtros = new Filtro[_labelFiltros.Length];

        for (int i = 0; i < _labelFiltros.Length; i++)
        {
            CategoricFilterConfiguration catConf;
            NumericFilterConfiguration numConf;

            if (_filtrosGameObject[i] != null)
            {
                _filtrosGameObject[i].TryGetComponent<CategoricFilterConfiguration>(out catConf);
                _filtrosGameObject[i].TryGetComponent<NumericFilterConfiguration>(out numConf);

                if (catConf != null && !_filtrosGameObject[i].name.Contains(nomeAtributo))
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], catConf.GetValores());

                    var filtroStringfy =
                            String.Join(",", _filtros[i].values.ToList().Select(str => "\"" + str + "\"").ToList());

                    _filtros[i].uri = $"{{\"field\": \" {_filtros[i].nome} \"," +
                            $"\"oneOf\": [ { filtroStringfy} ]}}";

                    continue;
                }
                else if (numConf != null)
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], numConf.GetValores());

                    _filtros[i].uri = $"{{\"field\": \" {_filtros[i].nome} \", " +
                        $"\"range\":[ {_filtros[i].values[0] }, {_filtros[i].values[1] } ]}}";

                    if (Convert.ToBoolean(_filtros[i].values[2]))
                        _filtros[i].uri = $"{{\"not\": {_filtros[i].uri} }}";

                    continue;
                }
                else
                {
                    _filtros[i] = new Filtro(_labelFiltros[i], new string[0]);
                    _filtros[i].uri = "";
                }
            }
        }

        string uriUnified = string.Join(",",
            _filtros.Where(go => go != null && !string.IsNullOrEmpty(go.uri))
            .ToList()
            .Select(go => go.uri)
            .ToArray());

        // TODO: Refatorar depois
        int indexFiltro = GetIndexFiltroPorNome(nomeAtributo);
        string[] values; 


        if (indexFiltro > 0)
        {
            values = _filtrosGameObject[indexFiltro].GetComponent<CategoricFilterConfiguration>().GetValores();
        }
        else
        {
            int indexAtributo = GetIndexAtributoPorNome(nomeAtributo);
            values = GetLabelAtributosCategoricos(indexAtributo);
        }

        List<string> _uriFiltrosParaSubVisualizacao = new List<string>();

        foreach (var label in values)
        {
            string subVisualizationFilterUri = $"{{\"field\": \" {nomeAtributo} \"," +
               $"\"oneOf\": [\"{label}\"]}}";

            string finalUri = uriUnified.Equals("") ? $"[{subVisualizationFilterUri}]" : $"[{uriUnified},{subVisualizationFilterUri}]";

            _uriFiltrosParaSubVisualizacao.Add(finalUri);
        }

        return _uriFiltrosParaSubVisualizacao.ToArray();
    }

    private int GetIndexFiltroPorNome(string nomeAtributo)
    {
        try
        {
            return Array.FindIndex(_filtrosGameObject, f => f.name.Contains(nomeAtributo));
        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning("Nenhum filtro configurado com o atributo selecionado");
            return -1;
        }
    }

    public void DebugAtributosSelecionados()
    {
        string json = GetFiltrosConfigurados();        
        Debug.Log(json);
    }
}
public class Filtro
{
    public string uri;
    public string nome;
    public string[] values;
    public new string ToString => $"{nome}: {string.Join(",", values)}";

    public Filtro (string name, string[] valores){
        this.nome = name;
        values = valores;
    }
}