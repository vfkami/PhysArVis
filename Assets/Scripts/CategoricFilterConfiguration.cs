using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoricFilterConfiguration : MonoBehaviour
{
    public Toggle[] toggles;
    private string[] textoLabels;

    public void SetOptions(string[] labels)
    {
        textoLabels = labels;

        if (labels.Length > toggles.Length)
        {
            Debug.LogWarning(
                $"A quantidade de atributos excede o máximo aceitado. Só serão exibidas 10 opções.");
        }

        for(int i=0; i < toggles.Length; i++)
        {
            if (i < labels.Length)
                toggles[i].GetComponentInChildren<TextMeshProUGUI>().text = labels[i];
            else
                toggles[i].gameObject.SetActive(false);
        }
    }

    public string[] GetValores()
    {
        List<string> valoresSelecionados = new List<string>();

        for (int i = 0; i < toggles.Length; i++)

            if (toggles[i].gameObject.activeSelf && toggles[i].isOn)              
                valoresSelecionados.Add(textoLabels[i]);

        return valoresSelecionados.ToArray();
    }
}
