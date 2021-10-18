using UnityEngine;
using UnityEngine.SceneManagement;


public class EndLevelDoor : MonoBehaviour
{
    [SerializeField] private int _coinsToNextLevel;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openedDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (player != null && player.Coins >= _coinsToNextLevel)
        {
            _spriteRenderer.sprite = _openedDoor;
            Invoke(nameof(LoadNextScene), 0.5f);
        }
    } 

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
