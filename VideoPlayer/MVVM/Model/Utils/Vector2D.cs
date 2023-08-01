using System;

namespace VideoPlayer.MVVM.Model.Utils;

public class Vector2D {
    public double X { get; set; }
    public double Y { get; set; }

    public Vector2D(double x, double y) {
        X = x;
        Y = y;
    }

    public double Magnitude() {
        return Math.Sqrt(X * X + Y * Y);
    }

    public void Normalize() {
        double magnitude = Magnitude();
        X /= magnitude;
        Y /= magnitude;
    }
}