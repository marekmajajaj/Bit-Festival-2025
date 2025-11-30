using UnityEngine;

public class PlayerCollision : MonoBehaviour
{ public GameManager gameManager;
    public void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Umieranko")
        {
            Debug.Log("lolol");
            FindFirstObjectByType<GameManager>().EndGame();
        }
        if (collisionInfo.collider.tag == "Zycka")
        {
            Debug.Log("lolol");
            FindFirstObjectByType<GameManager>().EndGame();
        }
    }

}
