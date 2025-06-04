using System;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace States
{
    public abstract class PlayerState : MonoBehaviour
    {
        protected Rigidbody2D _rigidBody;
        protected Animator _anim;
        public virtual void Enter()
        {

        }
        public virtual void Exit()
        {

        }
        public void Setup(Movement player)
        {
            _rigidBody = player.rigidBody;
        }
    }
    public enum PlayerDirection //This is to identify the direction the player is facing.
    {
        Left,
        Right,
        Top,
        Bottom,
    }
    public abstract class NPCstate : MonoBehaviour
    {

    }
}
