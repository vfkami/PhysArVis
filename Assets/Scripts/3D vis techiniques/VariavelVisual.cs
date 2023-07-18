using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariavelVisual : MonoBehaviour
{
    //variáveis visuais:
    public float posX, posY, posZ;
    public Material corPrefab;
    public GameObject referenciaPrefab;

    //variáveis da base:
    public float xValor, yValor, zValor;
    public string grupoValor, corValor;


    public void setAtributosGameObject(float x, float y, float z, GameObject prefab, Material cor)
    {
        posX = x;
        posY = y;
        posZ = z;
        referenciaPrefab = prefab;
        corPrefab = cor;

        GameObject variavelVisual = Instantiate(
            original: referenciaPrefab,
            parent: this.transform,
            position: new Vector3(0, 0, 0),
            rotation: Quaternion.identity
        );

        variavelVisual.transform.localPosition = new Vector3(0, 0, 0);
        variavelVisual.GetComponent<Renderer>().material = cor;
        variavelVisual.name = "VariavelVisual";
        return;
    }

    public void setAtributosGameObject(float x, float y, GameObject prefab, Material cor)
    {
        posX = x;
        posY = y;

        referenciaPrefab = prefab;
        corPrefab = cor;

        GameObject variavelVisual = Instantiate(
            original: referenciaPrefab,
            parent: this.transform,
            position: new Vector3(0, 0, 0),
            rotation: Quaternion.identity
        );

        variavelVisual.transform.localPosition = new Vector3(0, 0, 0);
        variavelVisual.GetComponent<Renderer>().material = cor;
        variavelVisual.name = "VariavelVisual";
        return;
    }


    // Seta os atributos da base para consulta no prefab posteriormente
    public void setAtributosBase(float x, float y, float z, string grupo, string cor)
    {
        xValor = x;
        yValor = y;
        zValor = z;
        grupoValor = grupo;
        corValor = cor;
    }
    public void setAtributosBase(float x, float y, string grupo, string cor)
    {
        xValor = x;
        yValor = y;
        grupoValor = grupo;
        corValor = cor;
    }

}
