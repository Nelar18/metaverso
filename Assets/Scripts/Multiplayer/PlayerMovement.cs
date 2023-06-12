using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CharacterController charController;
    [SerializeField] private Transform camProxy;
    [SerializeField] private float movSpeed;

    private bool[] _inputs;
    private float _movSpeed;


    private void OnValidate()
    {
        if (charController == null)
            charController = GetComponent<CharacterController>();
        if (player == null)
            player = GetComponent<Player>();
        
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        _inputs = new bool[5];
    }

    private void Initialize()
    {
        _movSpeed = movSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate() // tiene q ser FIXED
    {
        Vector3 inputDirecton = Vector3.zero;
        if (_inputs[0])
            inputDirecton.y += 1;
        if (_inputs[1])
            inputDirecton.y -= 1;
        if (_inputs[2])
            inputDirecton.x -= 1;
        if (_inputs[3])
            inputDirecton.x += 1;
        
        Move(inputDirecton,_inputs[4]);
    }

    private void Move(Vector2 inputDirection, bool sprint)
    {
        Vector3 moveDirection =
            Vector3.Normalize(camProxy.right * inputDirection.x + Vector3.Normalize(FlattenVector3((camProxy.forward)) * inputDirection.y));
        moveDirection *= _movSpeed;

        if (sprint)
            moveDirection *= 2f;

        charController.Move(moveDirection);

        SendMovement();
    }

    private Vector3 FlattenVector3(Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }

    public void SetInput(bool[] inputs, Vector3 forward)
    {
        _inputs = inputs;
        camProxy.forward = forward;
    }

    private void SendMovement()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddVector3(transform.position);
        message.AddVector3(camProxy.forward);
        NetworkManager.Singleton.Server.SendToAll(message);
    }
}
