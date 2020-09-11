using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviourPunCallbacks
{
    [FormerlySerializedAs("saveState")] [SerializeField] private SaveState[] saveStates;

    [SerializeField] private Vector3 actorPosition;

    public void Connect()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected to Master on region:{PhotonNetwork.CloudRegion}!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected to lobby:{PhotonNetwork.CurrentLobby.Name}!");
        PhotonNetwork.JoinOrCreateRoom("My Room", null, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Created room {PhotonNetwork.CurrentRoom.Name}!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}!");
        PhotonNetwork.LocalPlayer.NickName = $"Player{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}";
        SpawnSceneObjects();
        SpawnActorObject();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left!");
    }

    private void SpawnSceneObjects()
    {
        foreach (var s in saveStates)
        {
            var spawn = Instantiate(s.prefab, s.position, Quaternion.Euler(s.rotation));
            spawn.name = $"[Scene]{s.name}";
            
            var pv = spawn.AddComponent<PhotonView>();
            pv.ViewID = s.viewId;
            
            var ptv = spawn.AddComponent<PhotonTransformView>();
            ptv.m_SynchronizePosition = true;
            ptv.m_SynchronizeRotation = true;
            ptv.m_SynchronizeScale = true;

            var c = spawn.AddComponent<Cube>();
            c.renderer = spawn.GetComponentInChildren<Renderer>();

            pv.ObservedComponents = new List<Component> {ptv};
            pv.Synchronization = ViewSynchronization.Unreliable;

            PrintDetails(pv);
        }
    }

    private void SpawnActorObject()
    {
        var pos = actorPosition;
        var rot = Quaternion.identity;
        var spawn = PhotonNetwork.Instantiate("Actor", pos, rot);
        spawn.name = $"[{PhotonNetwork.LocalPlayer.NickName}]Actor";

        // var ptv = spawn.AddComponent<PhotonTransformView>();
        // ptv.m_SynchronizePosition = true;
        // ptv.m_SynchronizeRotation = true;
        // ptv.m_SynchronizeScale = true;
        //
        // var pv = spawn.GetComponent<PhotonView>();
        // pv.ObservedComponents = new List<Component> {ptv};
            
        PrintDetails(spawn.GetComponent<PhotonView>());
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
