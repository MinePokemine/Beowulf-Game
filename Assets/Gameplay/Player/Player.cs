using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : GridObject, Damageable {
    public int dashLen = 3;

    public Gametick tick;

    InputSystem input;
    (Action, float) queuedAction = (Action.NONE, 0f);

    public Image healthBubble;

    public float maxHealth = 3f;

    float health;
    bool warning = false;

    void Awake() {
        health = maxHealth;
        input = new();
        input.Player.Enable();
        input.Player.Attack.performed += Attack;
        input.Player.MoveU.performed += MoveU;
        input.Player.MoveL.performed += MoveL;
        input.Player.MoveD.performed += MoveD;
        input.Player.MoveR.performed += MoveR;

        tick.onPlayerTick += Tick;

        healthBubble.material = Instantiate(healthBubble.material);
    }

    void Update() { 
        base.Update();
        bool isPressed = false;

        isPressed = isPressed || (queuedAction.Item1.type == Action.Type.Attack && input.Player.Attack.IsInProgress());

        isPressed = isPressed || (queuedAction.Item1.type == Action.Type.Move && (
            (queuedAction.Item1.direction == Vector2.up && input.Player.MoveU.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.down && input.Player.MoveD.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.left && input.Player.MoveL.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.right && input.Player.MoveR.IsInProgress()) ));

        isPressed = isPressed || (queuedAction.Item1.type == Action.Type.Dash && input.Player.DashMod.IsInProgress() && (
            (queuedAction.Item1.direction == Vector2.up && input.Player.MoveU.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.down && input.Player.MoveD.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.left && input.Player.MoveL.IsInProgress()) ||
            (queuedAction.Item1.direction == Vector2.right && input.Player.MoveR.IsInProgress()) ));

        if (!isPressed) queuedAction.Item2 -= Time.deltaTime;

        healthBubble.material.SetFloat("_Health", health / maxHealth);
    }

    void Attack(InputAction.CallbackContext c) { queuedAction = (new Action(queuedAction.Item1.direction, Action.Type.Attack), 1f); }
    
    void MoveU (InputAction.CallbackContext c) { 
        if (input.Player.DashMod.IsInProgress()) 
             queuedAction = (new Action(Vector2.up   , Action.Type.Dash), tick.tickLength);
        else queuedAction = (new Action(Vector2.up   , Action.Type.Move), tick.tickLength); }
    void MoveL (InputAction.CallbackContext c) { 
        if (input.Player.DashMod.IsInProgress()) 
             queuedAction = (new Action(Vector2.left , Action.Type.Dash), tick.tickLength);
        else queuedAction = (new Action(Vector2.left , Action.Type.Move), tick.tickLength); }
    void MoveD (InputAction.CallbackContext c) { 
        if (input.Player.DashMod.IsInProgress()) 
             queuedAction = (new Action(Vector2.down , Action.Type.Dash), tick.tickLength);
        else queuedAction = (new Action(Vector2.down , Action.Type.Move), tick.tickLength); }
    void MoveR (InputAction.CallbackContext c) { 
        if (input.Player.DashMod.IsInProgress()) 
             queuedAction = (new Action(Vector2.right, Action.Type.Dash), tick.tickLength);
        else queuedAction = (new Action(Vector2.right, Action.Type.Move), tick.tickLength); }


    void Tick() {
        if (queuedAction.Item1.type == Action.Type.None || queuedAction.Item2 <= 0f) return;

        switch (queuedAction.Item1.type) {
            case Action.Type.Attack:
                break;
            case Action.Type.Move:
                Vector2Int move = queuedAction.Item1.direction.normalized.Floor();
                Debug.Log("Moving" + move);
                Move(grid.ForceInGrid(gridPos + move), 0.5f, Ease.Linear);
                break;
            case Action.Type.Dash:
                Debug.Log("Dashing");
                Vector2Int dash = (queuedAction.Item1.direction.normalized * dashLen).Floor(); 
                Move(grid.ForceInGrid(gridPos + dash), 0.5f, Ease.Linear);
                break;
        }
    }

    public void Damage(float amt) {
        health -= amt;
        warning = health <= amt;
    }

    struct Action {
        public Vector2 direction;
        public Type type;

        public static Action NONE = new Action(Vector2.zero, Type.None);

        public Action(Vector2 dir, Type action) {
            direction = dir;
            type = action;
        }

        public enum Type {
            Attack,
            Move,
            Dash,
            None
        }
    }
}
