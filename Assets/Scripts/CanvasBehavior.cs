using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    public GameObject UIElements;
    
    public GameObject DatasetManager;
    public GameObject SettingManager;
    public GameObject AxisConfigurationWidget;
    public GameObject FilterConfigurationWidgetManager;



    private void Start()
    {
        DatasetManager.SetActive(false);
        SettingManager.SetActive(false);
        AxisConfigurationWidget.SetActive(false);
        FilterConfigurationWidgetManager.SetActive(false);
    }
    public void LigarDesligarCanvas()
    {
        bool active = UIElements.gameObject.activeSelf;
        UIElements.gameObject.SetActive(!active); 
    }

    public void LigarDataset()
    {
        if (DatasetManager.activeSelf)
        {
            DatasetManager.SetActive(false);
            return;
        }

        DatasetManager.SetActive(true);
        SettingManager.SetActive(false);
        AxisConfigurationWidget.SetActive(false);
        FilterConfigurationWidgetManager.SetActive(false);
    }

    public void LigarSettings()
    {
        if (SettingManager.activeSelf) 
        { 
            SettingManager.SetActive(false);
            return;
        }
        DatasetManager.SetActive(false);
        SettingManager.SetActive(true);
        AxisConfigurationWidget.SetActive(false);
        FilterConfigurationWidgetManager.SetActive(false);
    }

    public void LigarAxis()
    {
        if (AxisConfigurationWidget.activeSelf) 
        { 
            AxisConfigurationWidget.SetActive(false);
            return;
        }

        DatasetManager.SetActive(false);
        SettingManager.SetActive(false);
        AxisConfigurationWidget.SetActive(true);
        FilterConfigurationWidgetManager.SetActive(false);
    }

    public void LigarFiltro()
    {
        if (FilterConfigurationWidgetManager.activeSelf) 
        { 
            FilterConfigurationWidgetManager.SetActive(false);
            return;
        }

        DatasetManager.SetActive(false);
        SettingManager.SetActive(false);
        AxisConfigurationWidget.SetActive(false);
        FilterConfigurationWidgetManager.SetActive(true);
    }

}
