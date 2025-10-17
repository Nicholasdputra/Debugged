using UnityEngine;

public class MainMenuEnemyTeleporter : MonoBehaviour
{
    public GameObject teleportTo;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Teleporting entity: " + collision.gameObject.name);
        TeleportEntity(collision.gameObject.transform);
    }

    void TeleportEntity(Transform entity)
    {
        entity.position = new Vector2(teleportTo.transform.position.x, entity.position.y);
    }
}
