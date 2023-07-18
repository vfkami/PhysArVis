using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionBehavior : MonoBehaviour
{

    private ARTrackedImageManager m_TrackedImageManager;
    public int qtdRastreada;

    void Awake()
    {
        m_TrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    public void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        qtdRastreada = m_TrackedImageManager.trackables.count;
        string texto = $"Quantidade Rastreada: {qtdRastreada}";
        
        ListAllImages();


        if (m_TrackedImageManager.trackables.count > 0)
        {
            //Debug.Log(texto);
            return;
        }

        //Debug.Log("Nenhuma imagem rastreada!");
    }

    void ListAllImages()
    {
        /*Debug.Log(
            $"There are {m_TrackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in m_TrackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
        }*/
    }
}
