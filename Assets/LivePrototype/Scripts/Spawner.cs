using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private SaveState[] saveState;

    [SerializeField] private GameObject resourcesCube;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        PhotonNetwork.JoinOrCreateRoom("My Room", null, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Created room {PhotonNetwork.CurrentRoom.Name}!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}!");
        
        for
    }

    private void SpawnResourceCubes(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var rcPos = saveState[0].position + Vector3.up * (1 + i);
            var rcSpawn = PhotonNetwork.InstantiateSceneObject($"ResourcesCube", rcPos, Quaternion.Euler(saveState[0].rotation));
            // var rcPhotonView = rcSpawn.GetComponent<PhotonView>();
            // rcPhotonView.gameObject.name = $"ResourcesCube{i}";
            // PrintDetails(rcPhotonView);
        }
    }

    private void PrintDetails(PhotonView photonView)
    {
        Debug.Log($"go:{photonView.gameObject.name} CreatorActorNr:{photonView.CreatorActorNr.ToString()} OwnerActorNr:{photonView.OwnerActorNr.ToString()} ControllerActorNr:{photonView.ControllerActorNr.ToString()} ViewId:{photonView.ViewID.ToString()}");
    }

    [System.Serializable]
    public struct SaveState
    {
        public string name;
        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotation;
        public int viewId;
    }
}
