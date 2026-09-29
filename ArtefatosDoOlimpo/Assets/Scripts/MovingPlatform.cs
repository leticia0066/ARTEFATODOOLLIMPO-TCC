using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Pontos do movimento")]
    public Transform pointA;
    public Transform pointB;

    [Header("Velocidade")]
    public float speed = 2f;

    private Rigidbody2D rb;

    private Transform alvoAtual;

    private Rigidbody2D playerRb;

    private bool playerEmCima = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("A plataforma precisa de Rigidbody2D!");
            return;
        }

        if (pointA == null || pointB == null)
        {
            Debug.LogError("Defina Point A e Point B!");
            return;
        }

        alvoAtual = pointB;
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        // Guarda a posição atual
        Vector2 posicaoAnterior = rb.position;

        // Move a plataforma até o ponto
        Vector2 novaPosicao = Vector2.MoveTowards(
            posicaoAnterior,
            alvoAtual.position,
            speed * Time.fixedDeltaTime
        );

        // Move a plataforma
        rb.MovePosition(novaPosicao);

        // Descobre quanto ela andou
        Vector2 deslocamento = novaPosicao - posicaoAnterior;

        // =====================================================
        // CARREGA O PLAYER JUNTO
        // =====================================================

        if (playerEmCima && playerRb != null)
        {
            playerRb.position += deslocamento;
        }

        // =====================================================
        // CHEGOU NO PONTO
        // =====================================================

        if (Vector2.Distance(
            novaPosicao,
            alvoAtual.position
        ) < 0.01f)
        {
            if (alvoAtual == pointA)
            {
                alvoAtual = pointB;
            }
            else
            {
                alvoAtual = pointA;
            }
        }
    }

    // =========================================================
    // PLAYER ENTROU NA PLATAFORMA
    // =========================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VerificarPlayer(collision);
    }

    // =========================================================
    // PLAYER CONTINUA NA PLATAFORMA
    // =========================================================

    private void OnCollisionStay2D(Collision2D collision)
    {
        VerificarPlayer(collision);
    }

    // =========================================================
    // PLAYER SAIU / PULOU
    // =========================================================

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D rbPlayer =
            collision.gameObject.GetComponent<Rigidbody2D>();

        if (rbPlayer == playerRb)
        {
            playerRb = null;
            playerEmCima = false;
        }
    }

    // =========================================================
    // VERIFICA SE ESTÁ EM CIMA
    // =========================================================

    private void VerificarPlayer(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D rbPlayer =
            collision.gameObject.GetComponent<Rigidbody2D>();

        if (rbPlayer == null)
            return;

        foreach (ContactPoint2D contato in collision.contacts)
        {
            // Normal apontando para cima =
            // Player está apoiado na plataforma
            if (contato.normal.y > 0.5f)
            {
                playerRb = rbPlayer;
                playerEmCima = true;

                return;
            }
        }
    }
}