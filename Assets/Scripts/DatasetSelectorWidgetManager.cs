using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DatasetSelectorWidgetManager : MonoBehaviour
{
    public RequisitionManager requisitionManager;

    public TMP_Dropdown dpdDataset;
    public TMP_Text txtConteudoDataset;

    public void AtualizaOpcoesDropdownDataset(string[] data)
    {
        dpdDataset.ClearOptions();


        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("Selecione");
        newOptions.Add(option);

        foreach (var label in data)
        {
            option = new TMP_Dropdown.OptionData(label);
            newOptions.Add(option);
        }

        dpdDataset.options = newOptions;
        txtConteudoDataset.text = "Connection Established. Select the dataset";
    }

    public void AtualizaOpcoesDisponiveis()
    {
        requisitionManager.GetDatasetsDisponiveis();
    }


    public void GetDatasetByDropodownOptions()
    {
        int dropdownIndex = dpdDataset.value - 1;
        string nomeDataset = dpdDataset.options[dpdDataset.value].text;

        if (dropdownIndex < 0) return;

        requisitionManager.GetDatasetPorNome(nomeDataset);
        DatasetManager.SetNomeDataset(nomeDataset);
    }

    public void AtualizaTextoCanvas(string text)
    {
        txtConteudoDataset.text = text;
    }

}
