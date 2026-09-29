using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float velocidade = 0.3f;

    private float distanciaInicial;

    void Start()
    {
        if (cameraTransform != null)
        {
            distanciaInicial = transform.position.x - cameraTransform.position.x;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        float novoX =
            cameraTransform.position.x * velocidade
            + distanciaInicial;

        transform.position = new Vector3(
            novoX,
            transform.position.y,
            transform.position.z
        );
    }
}