using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_InputField _ifEnderecoIp;
    public TMP_InputField _ifQtdBarras;


    FiducialMarkerManager _fmm;
    RequisitionManager _rm;

    private void Start()
    {
        _rm = GameObject.Find("SceneManager").GetComponent<RequisitionManager>();
        _fmm = GameObject.Find("FiducialMarkersManager").GetComponent<FiducialMarkerManager>();
    }


    public void SetEnderecoIP()
    {
        string endereco = _ifEnderecoIp.text;
        if (IsIPValido(endereco))
            _rm.SetEnderecoServidor(endereco);
        else
            Debug.LogError("Endereço definido incorretamente. Tente novamente!");
    }

    /*
    public void SetQtdBarras()
    {
        string textoQtdMarcadores = _ifQtdBarras.text;
        int qtdMarcadores;

        if (int.TryParse(textoQtdMarcadores, out qtdMarcadores))
            _fmm.SetNumeroMarcadores(qtdMarcadores);
    }
    */

    public void SetQtdBarras(int qtdMarcadores)
    {
        if (_fmm == null)
            _fmm = GameObject.Find("FiducialMarkersManager").GetComponent<FiducialMarkerManager>();

        _fmm.SetNumeroMarcadores(qtdMarcadores);
    }

    public bool IsIPValido(string ipAddress)
    {
        // Separa o endereço IP em 4 partes
        string[] parts = ipAddress.Split('.');

        // Verifica se existem 4 partes
        if (parts.Length != 4)
            return false;
        

        // Verifica se cada parte é um número inteiro
        int num;
        foreach (string part in parts)
        {
            if (!int.TryParse(part, out num))
            {
                return false;
            }

            // Verifica se o número está dentro do intervalo válido
            if (num < 0 || num > 255)
            {
                return false;
            }
        }

        return true;
    }



}
