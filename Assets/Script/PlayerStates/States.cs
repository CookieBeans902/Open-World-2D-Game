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
        protected Movement player;
        protected float moveSpeed;
        public virtual void Enter() //Creating a virtual function to be overriden in the base classes.
        {
            //Code here is used to run the required stuff when a player enters a new state, such as 
            // changing animations, or starting vfx, timers, etc.
        }
        public virtual void Execute() //Defining an execute method to be executed during the duration of the state.
        {
            
        }
        public virtual void Exit()
        {
            //An exit method, that is implemented if necessary. As of now, basically useless
        }
        public void Setup(Movement variable) // A setup method to be called during the start of the movement script.
        {
            _rigidBody = variable.rigidBody;
            _anim = variable.anim;
            //Assigning a variable to reference the script passed through the code,(remember 'this' was used as input)
            player = variable;
            moveSpeed = variable.GetMoveSpeed(); // Getting the movespeed 
        }
    }
    public enum PlayerDirection //This is to identify the direction the player is facing.
    {
        Left,
        Right,
        Top,
        Bottom,
    }
    public abstract class NPCstate : MonoBehaviour // NPC states for future, if they exist
    {

    }
}
