using TMPro;
using UnityEngine;

public class Barra : MonoBehaviour
{
    //variáveis visuais:
    public float posX, posY, posZ;
    public Material corPrefab;
    public GameObject referenciaPrefab;

    //variáveis da base:
    public float yValor, zValor;
    public string xValor, grupoValor, corValor;

    //altura minima da barra caso valor seja 0
    public const float AlturaMinima = 0.5F;

    public void SetAtributosGameObject(float x, float grossuraBarra, float y, Material cor)
    {
        posX = x;
        posY = y;
        corPrefab = cor;

        referenciaPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        
        GameObject barra = Instantiate(
            original: referenciaPrefab,
            parent: this.transform,
            position: new Vector3(0, 0, 0),
            rotation: Quaternion.identity
        ) ;

        barra.transform.localPosition = new Vector3(0.5F, 0.5F, 0);
        barra.GetComponent<Renderer>().material = cor;
        barra.name = "Barra";

        transform.localPosition = new Vector3(x, 0, 0);
        transform.localScale = new Vector3(grossuraBarra, y + AlturaMinima, 1);

        Destroy(referenciaPrefab);
    }

    //TODO: Seta os atributos da base de dados no GameObject
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
