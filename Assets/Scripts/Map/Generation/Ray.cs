using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;

public readonly struct Ray {
    private readonly double x;
    private readonly double y;
    private readonly double angle;
    private readonly Random rng;

    public Ray(double x, double y, double angle, Random rng) {
        this.x = x;
        this.y = y;
        this.angle = angle;
        this.rng = rng;
    }

    public IEnumerable<(int x, int y)> Squares() {
        double x = this.x;
        double y = this.y;
        while (true) {
            int xi, yi, xi2, yi2;
            do {
                xi = (int) x;
                yi = (int) y;
                x += Math.Sin(angle);
                y += Math.Cos(angle);
                xi2 = (int) x;
                yi2 = (int) y;
            } while (xi == xi2 && yi == yi2);
            if (xi == xi2) {
                yield return (xi, yi2);
                continue;
            }
            if (yi == yi2) {
                yield return (xi2, yi);
                continue;
            }
            double m = 1 / Math.Tan(angle);
            double c = y - m * x;
            var d1 = (x: xi2 + 0.5, y: yi + 0.5);
            var d2 = (x: xi + 0.5, y: yi2 + 0.5);
            double dm = (d2.y - d1.y) / (d2.x - d1.x);
            double dc = d1.y - dm * d1.x;
            double ix = (dc - c) / (m - dm);
            double iy = m * ix + c;
            if (Math.Floor(ix) == Math.Ceiling(ix)) {
                if (rng.Next(2) == 0) {
                    ix += 0.5;
                    iy += 0.5 * dm;
                } else {
                    ix -= 0.5;
                    iy -= 0.5 * dm;
                }
            }
            yield return (x: (int) ix, y: (int) iy);
            yield return (x: xi2, y: yi2);
        }
    }
}