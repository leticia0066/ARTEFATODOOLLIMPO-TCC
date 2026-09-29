using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Configuração")]
    public Transform cameraTransform;
    
    [Header("Velocidade do Parallax")]
    [Range(0f, 1f)]
    public float parallaxEffectMultiplier = 0.25f; // 0 = segue a câmera perfeitamente, 1 = fica parado

    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    void Start()
    {
        // Se você esquecer de arrastar a câmera, o código tenta achar automaticamente
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("ERRO: Nenhuma Main Camera foi encontrada na cena!");
                enabled = false;
                return;
            }
        }

        lastCameraPosition = cameraTransform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        // PROTEÇÃO: Verifica se o SpriteRenderer ou o Sprite estão vazios (evita o erro de NaN)
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogError("ERRO: O objeto '" + gameObject.name + "' precisa de um componente SpriteRenderer com uma imagem (Sprite) colocada nele!");
            enabled = false; // Desativa o script para não travar o jogo
            return;
        }

        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;

        // PROTEÇÃO: Evita divisão por zero
        if (sprite.pixelsPerUnit == 0)
        {
            textureUnitSizeX = texture.width / 100f; 
        }
        else
        {
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }

        // PROTEÇÃO: Garante que o tamanho seja válido
        if (textureUnitSizeX <= 0 || float.IsNaN(textureUnitSizeX))
        {
            textureUnitSizeX = 10f; // Valor padrão seguro
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Calcula o quanto a câmera se moveu
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        // Move o fundo proporcionalmente ao movimento da câmera (Parallax)
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);
        
        lastCameraPosition = cameraTransform.position;

        // Verifica se a câmera passou do limite do tamanho da imagem para fazer a emenda invisível
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
    }
}
