using UnityEngine;

public class Barrier : GridObject
{
    public override bool Collide(GridObject obj) {
        return true;
    }
}
