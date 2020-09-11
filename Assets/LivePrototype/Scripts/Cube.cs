using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Renderer renderer;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var pv = GetComponent<PhotonView>();
            pv.RPC("ChangeColor", RpcTarget.AllBuffered, 10);
        }
    }

    [PunRPC]
    public void ChangeColor(int myInt)
    {
        if (myInt != 10)
            return;
        
        var materials = new List<Material>();
        renderer.GetMaterials(materials);
        materials[0].color = Color.red;
    }
}
