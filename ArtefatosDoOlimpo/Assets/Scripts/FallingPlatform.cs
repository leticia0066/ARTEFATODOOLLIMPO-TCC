using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float fallDelay = 1f;       // Tempo que ela balança/espera antes de cair
    public float fallDuration = 1.5f;  // Tempo que ela passa caindo antes de sumir da tela
    public float respawnTime = 3f;     // Tempo que ela fica sumida antes de reaparecer

    private Rigidbody2D rb;
    private Collider2D colisor;
    private SpriteRenderer sprite;
    private Vector3 startPos;
    private bool estaCaindo = false;   // Evita que o código rode duas vezes seguidas

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Só ativa se for o Player e se a plataforma já não estiver no processo de queda
        if (collision.gameObject.CompareTag("Player") && !estaCaindo)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        estaCaindo = true;

        // 1. Espera o tempo antes de cair
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 2. Espera ela cair um pouco na tela
        yield return new WaitForSeconds(fallDuration);

        // 3. Desliga o visual e o colisor para ela sumir de verdade do jogo
        MudarEstadoPlataforma(false);

        // 4. Espera o tempo de respawn com ela invisível
        yield return new WaitForSeconds(respawnTime);

        // 5. Reseta e faz reaparecer
        Respawn();
    }

    void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f; // Garante que ela não volte girando se tiver batido em algo

        transform.position = startPos;

        // Faz o visual e a colisão voltarem ao normal
        MudarEstadoPlataforma(true);
        estaCaindo = false; // Libera para cair de novo quando pisarem
    }

    // Função auxiliar para ativar/desativar o visual e a colisão de uma vez só
    void MudarEstadoPlataforma(bool ativo)
    {
        if (colisor != null) colisor.enabled = ativo;
        if (sprite != null) sprite.enabled = ativo;
    }
}
