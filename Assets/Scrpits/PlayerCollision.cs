using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
    /*    if (collisionInfo.collider.tag == "Ground")
        {
            GlobalVariables.ziemia = 1;
        }*/
    }
    private void OnCollisionExit(Collision collisionInfo)
    {

    }
}
