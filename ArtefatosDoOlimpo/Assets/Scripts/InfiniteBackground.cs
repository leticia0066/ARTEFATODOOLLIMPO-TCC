using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private Transform cameraTransform;

    [Header("Velocidade do Parallax")]
    [Range(0f, 1f)]
    [SerializeField] private float parallax = 0.3f;

    private float largura;

    private Transform fundo1;
    private Transform fundo2;
    private Transform fundo3;

    private Vector3 ultimaPosicaoCamera;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        fundo1 = transform.GetChild(0);

        GameObject fundo2Obj = Instantiate(fundo1.gameObject, transform);
        GameObject fundo3Obj = Instantiate(fundo1.gameObject, transform);

        fundo2 = fundo2Obj.transform;
        fundo3 = fundo3Obj.transform;

        SpriteRenderer sprite = fundo1.GetComponent<SpriteRenderer>();

        if (sprite == null)
        {
            Debug.LogError("O fundo precisa ter um SpriteRenderer!");
            return;
        }

        largura = sprite.bounds.size.x;

        fundo1.position = new Vector3(
            fundo1.position.x,
            fundo1.position.y,
            fundo1.position.z
        );

        fundo2.position = new Vector3(
            fundo1.position.x + largura,
            fundo1.position.y,
            fundo1.position.z
        );

        fundo3.position = new Vector3(
            fundo1.position.x + largura * 2f,
            fundo1.position.y,
            fundo1.position.z
        );

        ultimaPosicaoCamera = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        float movimentoCamera =
            cameraTransform.position.x - ultimaPosicaoCamera.x;

        transform.position += new Vector3(
            movimentoCamera * parallax,
            0f,
            0f
        );

        ultimaPosicaoCamera = cameraTransform.position;

        ReposicionarFundos();
    }

    void ReposicionarFundos()
    {
        float cameraX = cameraTransform.position.x;

        float distancia1 = Mathf.Abs(cameraX - fundo1.position.x);
        float distancia2 = Mathf.Abs(cameraX - fundo2.position.x);
        float distancia3 = Mathf.Abs(cameraX - fundo3.position.x);

        if (distancia1 > largura * 1.5f)
        {
            fundo1.position = NovaPosicao(fundo1);
        }

        if (distancia2 > largura * 1.5f)
        {
            fundo2.position = NovaPosicao(fundo2);
        }

        if (distancia3 > largura * 1.5f)
        {
            fundo3.position = NovaPosicao(fundo3);
        }
    }

    Vector3 NovaPosicao(Transform fundo)
    {
        float maiorX = Mathf.Max(
            fundo1.position.x,
            fundo2.position.x,
            fundo3.position.x
        );

        float menorX = Mathf.Min(
            fundo1.position.x,
            fundo2.position.x,
            fundo3.position.x
        );

        if (fundo.position.x < cameraTransform.position.x)
        {
            return new Vector3(
                maiorX + largura,
                fundo.position.y,
                fundo.position.z
            );
        }
        else
        {
            return new Vector3(
                menorX - largura,
                fundo.position.y,
                fundo.position.z
            );
        }
    }
}