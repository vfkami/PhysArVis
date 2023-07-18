using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoLinha : MonoBehaviour
{
    //TODO: Adicionar a opção de ocultar os pontos

    //variáveis visuais:
    public float posX, posY, posZ;
    public Material corPrefab;
    public GameObject referenciaPrefabLinha;
    public GameObject referenciaPrefabPonto;

    //variáveis da base:
    public float yValor, zValor;
    public string xValor, grupoValor, corValor;

    public GameObject parentLinha;
    public GameObject linha;
    public GameObject ponto;
    public GameObject proximoPonto;

    public void SetAtributosGameObject(float x, float y)
    {
        posX = x;
        posY = y;
        transform.localPosition = new Vector3(x, y, 0);
        return;
    }

    // Cria o elemento visual linha. Para isso é criado um GameObject vazio
    // que será utilizado como pivot da linha em si. Após isso, é criado a
    // linha (cubo) e aplicado a devida configuração.
    public void CriaLinha(Material cor)
    {
        parentLinha = new GameObject("linhaParent");
        parentLinha.transform.SetParent(this.transform);
        parentLinha.transform.localPosition = Vector3.zero;

        referenciaPrefabLinha = GameObject.CreatePrimitive(PrimitiveType.Cube);
        linha = Instantiate(
            original: referenciaPrefabLinha,
            parent: parentLinha.transform,
            position: Vector3.zero,
            rotation: Quaternion.identity
        );
        linha.name = "Linha";
        linha.transform.localPosition = new Vector3(0, 0.5F, 0);
        linha.GetComponent<Renderer>().material = cor;

        Destroy(referenciaPrefabLinha);
    }

    // Cria o elemento visual ponto.
    public void CriaPonto(Material cor)
    {
        referenciaPrefabPonto = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ponto = Instantiate(
            original: referenciaPrefabPonto,
            parent: this.transform,
            position: Vector3.zero,
            rotation: Quaternion.identity
        );

        ponto.name = "Ponto";
        ponto.GetComponent<Renderer>().material = cor;
        ponto.transform.localPosition = Vector3.zero;
        ponto.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);

        Destroy(referenciaPrefabPonto);
    }

    public void SetCorGameObject(Material cor)
    {
        corPrefab = cor;
        GetComponent<Renderer>().material = corPrefab;
    }

    // Define o proximo ponto. É o ponto que será utilizado para escalar
    // a linha deste GameObject de forma que os pontos se toquem
    public void SetProximoPonto(GameObject go)
    {
        proximoPonto = go;
    }

    // Realiza a escala no linhaParent para que o gameobject conecte visualmente
    // ao proximo ponto;
    public void ConectaProximoPonto()
    {
        if (proximoPonto == null)
            throw new System.Exception("Próximo ponto não definido ainda");

        var novaEscala = Utils.CalculaDistorcaoLinha(gameObject, proximoPonto);
        parentLinha.transform.localScale = novaEscala;
    }

    // Função para retornar a escala da linha para dos valores padrão
    public void RedefineEscalaLinha()
    {
        linha.transform.localScale = new Vector3(0.1F, 1F, 0.1F);
    }

    public void SetAtributosBase(string x, float y, float z, string grupo, string cor)
    {
        xValor = x;
        yValor = y;
        zValor = z;
        grupoValor = grupo;
        corValor = cor;
    }

    public void SetAtributosBase(string x, float y, string cor)
    {
        xValor = x;
        yValor = y;
        corValor = cor;
    }

    public void SetAtributosBase(string x, float y)
    {
        xValor = x;
        yValor = y;
    }

}
