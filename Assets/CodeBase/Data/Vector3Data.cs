using System;

namespace CodeBase.Data {
  [Serializable]
  public class Vector3Data {
    public float Y;
    public float X;
    public float Z;

    public Vector3Data(float y, float x, float z) {
      Y = y;
      X = x;
      Z = z;
    }

    public override string ToString() {
      return $" ({X}, {Y}, {Z})";
    }
  }
}