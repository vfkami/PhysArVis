using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public string json;
    //{"columns":["sepal_length","sepal_width","petal_length","petal_width","iris"],"rows":150,"meta":[{"name":"sepal_length","type":"numeric","extent":[4.3,7.9]},{"name":"sepal_width","type":"numeric","extent":[2,4.4]},{"name":"petal_length","type":"numeric","extent":[1,6.9]},{"name":"petal_width","type":"numeric","extent":[0.1,2.5]},{"name":"iris","type":"categorical","extent":["Iris-setosa","Iris-versicolor","Iris-virginica"]}]}

    public static float[] NormalizaValores(float[] valores)
    {
        float[] valoresNormalizados = new float[valores.Length];

        for (int i = 0; i < valores.Length; i++)
            valoresNormalizados[i] = (valores[i] - valores.Min()) / (valores.Max() - valores.Min());

        return valoresNormalizados;
    }

    //Normaliza os valores recebidos em um range de {0 - 1} * x
    public static float[] NormalizaValoresComMultiplicador(float[] valores, int x)
    {
        float[] valoresNormalizados = NormalizaValores(valores);
        valoresNormalizados = valoresNormalizados.Select(v => (v * x)).ToArray();

        return valoresNormalizados;
    }

    /* 
     * Converte os valores categoricos em um valor numérico {int} para
     * serem utilizados como indice de um array;
     */
    public static int[] ConverteCategoriasParaNumerico(string[] valores)
    {
        int[] valoresNormalizados = new int[valores.Length];
        Dictionary<string, int> dicionario = new Dictionary<string, int>();

        for (int i = 0; i < valores.Length; i++)
        {
            if (!dicionario.ContainsKey(valores[i]))
                dicionario.Add(valores[i], dicionario.Count);

            valoresNormalizados[i] = dicionario[valores[i]];
        }

        return valoresNormalizados;
    }

    // Verifica se as arrays recebidas são do mesmo tamanho
    public static bool ArraysSaoDoMesmoTamanho(params Array[] arrays)
    {
        return arrays.All(a => a.Length == arrays[0].Length);
    }

    // Utilizado na construção do BarChart, esse método calcula a posição
    // das barras de acordo com a quantidade de barras e o tamanho do eixo X
    public static float[] CalculaPosicaoBarras(int qtdBarras, int tamanhoEixoX)
    {
        float espacamento = CalculaEspacamentoEntreBarras(qtdBarras, tamanhoEixoX);
        float grossuraBarra = CalculaEspessuraGameObject(qtdBarras, tamanhoEixoX);

        float espacoTotalBarra = grossuraBarra + espacamento;
        float[] posicaoBarras = new float[qtdBarras];

        for (int i = 0; i < qtdBarras; i++)
        {
            posicaoBarras[i] = espacoTotalBarra * i;
        }

        return posicaoBarras;
    }

    // Calcula a espessura das barras para que caibam no tamanho do eixo x
    public static float CalculaEspessuraGameObject(int qtdBarras, int tamanhoEixoX)
    {
        float espacoTotal = tamanhoEixoX - (tamanhoEixoX / 10);
        return espacoTotal / qtdBarras;
    }

    // Calcula o espaçamento maximo das barras para que caibam no tamanho do eixo x
    public static float CalculaEspacamentoEntreBarras(int qtdBarras, int tamanhoEixoX)
    {
        float decimo = tamanhoEixoX / 10;
        return decimo / qtdBarras;
    }

    // Calcula o ângulo de rotação (eixo y) para que um objeto X aponte para um objeto Y.
    // Retorna o ângulo de rotação; Funciona apenas para pontos no mesmo eixo Z
    public static Quaternion CalculaAnguloEntreDoisPontos(GameObject x, GameObject y)
    {
        Vector3 direcao = CalculaDirecaoEntreDoisPontos(x, y);
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg - 90;

        return Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    //Calcula um Vector3 com o ponto médio entre dois GameObjects
    public static Vector3 CalculaDirecaoEntreDoisPontos(GameObject x, GameObject y)
    {
        return y.transform.position - x.transform.position;
    }

    // Rotaciona um objeto x no ângulo recebido. Limitacao: Rotaciona apenas no eixo Y 
    public static void RotacionaObjeto(GameObject go, Quaternion angle)
    {
        go.transform.rotation = Quaternion.Slerp(go.transform.rotation, angle, Time.deltaTime * 50);
    }

    /* 
     * Calcula a escala que deve ser aplicada ao Game Object que irá conectar os dois
     * pontos. Para isso, é calculado a hipotenusa de um triângulo 3D. Tutorial aqui:
     * https://www.mathsisfun.com/geometry/pythagoras-3d.html
     */
    public static Vector3 CalculaDistorcaoLinha(GameObject x, GameObject y)
    {
        Quaternion angulo = CalculaAnguloEntreDoisPontos(x, y);

        // O Objeto não pode estar rotacionado
        x.transform.localRotation = new Quaternion(0, 0, 0, 0);
        RotacionaObjeto(x, angulo);

        Vector3 direcao = CalculaDirecaoEntreDoisPontos(x, y);

        float hipotenusa = Mathf.Sqrt(
            Mathf.Pow(direcao.x, 2) + Mathf.Pow(direcao.y, 2) + Mathf.Pow(direcao.z, 2)
        );

        return new Vector3(1, hipotenusa, 1);
    }

    public static Dataset CriaDoJSON(string JSON)
    {
        return JsonUtility.FromJson<Dataset>(JSON);
    }

    public static Sprite RenderOfBytes(byte[] data)
    {
        Texture2D tex = new Texture2D(900, 465);
        tex.LoadImage(data);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rect, new Vector2(0, 0), 100f);
    }
}

