using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AxisConfigurationWidgetManager : MonoBehaviour
{
    public TMP_Dropdown xAxisSelector;
    public TMP_Dropdown yAxisSelector;
    public TMP_Dropdown colorSelector;
    public TMP_Dropdown subVisualizationSelector;

    private void SetDropdownLabels(TMP_Dropdown dropdown, string[] labels)
    {
        List<TMP_Dropdown.OptionData> opcoes = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("Selecione");
        opcoes.Add(option);

        foreach (string label in labels)
        {
            option = new TMP_Dropdown.OptionData(label);
            opcoes.Add(option);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(opcoes);
    }

    public void SetLabelsSubVisualization(string[] labels)
    {
        SetDropdownLabels(subVisualizationSelector, labels);
    }

    public void SetLabelsAtributoEixoX(string[] labels)
    {
        SetDropdownLabels(xAxisSelector, labels);
    }


    public void SetLabelsAtributoEixoY(string[] labels)
    {
        SetDropdownLabels(yAxisSelector, labels);

    }

    public void SetLabelsAtributoCor(string[] labels)
    {
        SetDropdownLabels(colorSelector, labels);
    }

    public void SetEixoX()
    {
        string eixoX = GetAtributoSelecionadoEixoX();
        DatasetManager.SetNomeEixoX(eixoX);
    }

    public void SetEixoY()
    {
        string eixoY = GetAtributoSelecionadoEixoY();
        DatasetManager.SetNomeEixoY(eixoY);
    }

    public void SetCor()
    {
        string cor = GetAtributoSelecionadoCor();
        DatasetManager.SetNomeCor(cor);
    }

    public int GetIndexAtributoSelecionadoSubVisualizacao()
    {
        return subVisualizationSelector.value - 1;
    }

    public string[] GetAtributosSelecionados()
    {
        string eixoX = GetAtributoSelecionadoEixoX();
        string eixoY = GetAtributoSelecionadoEixoY();

        return new string[] { eixoX, eixoY };
    }

    public void SetAtributoSubVisualizacao()
    {
        string atributo = GetAtributoSelecionadoSubVisualization();
        int index = atributo != null ? subVisualizationSelector.value -1 : -1;

        DatasetManager.SetNomeAtributoSubVisualizacao(index, atributo);
    }
    
    public void DebugAtributosSelecionados()
    {
        Debug.Log(GetAtributosSelecionados()) ;
    }

    public string GetAtributoSelecionadoEixoX()
    {
        string value = xAxisSelector.options[xAxisSelector.value].text;
        if (value.Contains("Selecione") || value == "" ) return "null";

        return value;
    }

    public string GetAtributoSelecionadoEixoY()
    {
        string value = yAxisSelector.options[yAxisSelector.value].text;
        if (value.Equals("Selecione") || value == "") return "null";

        return value;
    }

    public string GetAtributoSelecionadoCor()
    {
        string value = colorSelector.options[colorSelector.value].text;
        if (value.Equals("Selecione") || value == "") return "null";

        return value;
    }

    public string GetAtributoSelecionadoSubVisualization()
    {
        string value = subVisualizationSelector.options[subVisualizationSelector.value].text;
        if (value.Contains("Selecione") || value == "") return "null";

        return value;
    }
}