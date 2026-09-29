using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Câmera")]
    public Transform cameraTransform;

    [Header("Movimento")]
    [Range(0f, 1f)]
    public float seguirX = 1f;

    [Range(0f, 1f)]
    public float seguirY = 1f;

    private float distanciaX;
    private float distanciaY;

    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("BackgroundFollow: Main Camera não foi definida!");
            return;
        }

        // Guarda a distância inicial entre o fundo e a câmera
        distanciaX = transform.position.x - cameraTransform.position.x;
        distanciaY = transform.position.y - cameraTransform.position.y;
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        float novoX =
            cameraTransform.position.x * seguirX
            + distanciaX;

        float novoY =
            cameraTransform.position.y * seguirY
            + distanciaY;

        transform.position = new Vector3(
            novoX,
            novoY,
            transform.position.z
        );
    }
}