using UnityEngine;

public class Giant : Damageable {
    public GiantsController controller;
    public int index;
    public float health;

    public override void Damage(float amt) {
        throw new System.NotImplementedException();
    }

    public enum ShieldType {
        
    }
}
