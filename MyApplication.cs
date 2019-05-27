using OpenTK;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        // 3 floats for the colors per pixel.
        public float[,][] floatbuffer = new float[640, 640][];
        //lightsources list.
        public float[] lightbuffer = new float[4];
        // worldsize
        public float worldsize = 20f;
        // primitives list
        List<primitives> prim1 = new List<primitives>();
        List<box> prim = new List<box>();
        triangle tr1 = new triangle();
        //texture
        Surface map;
        public float[,][] c;
        // initialize
        public void Init()
        {
            map = new Surface("../../assets/tiles.png");
            c = new float[640, 640][];            texture(floatbuffer, c, map);
            lightbuffer[0] = worldsize - 18f;
            lightbuffer[1] = worldsize - 19f;
            lightbuffer[2] = worldsize - 2f;
            lightbuffer[3] = worldsize - 2f;
            box b = new box();
            b.X1 = 400;
            b.X2 = 450;
            b.Y1 = 400;
            b.Y2 = 450;
            b.C = MixColor(0.5f, 0.5f, 0);
            box b1 = new box();
            b1.X1 = 200;
            b1.X2 = 250;
            b1.Y1 = 200;
            b1.Y2 = 250;
            b1.C = MixColor(1, 0, 1);
            box b2 = new box();
            b2.X1 = 400;
            b2.X2 = 450;
            b2.Y1 = 200;
            b2.Y2 = 250;
            b2.C = MixColor(1, 1, 0);
            prim.Add(b);
            prim.Add(b1);
            prim.Add(b2);
            
            tr1.X = 100;
            tr1.Y = 500;
            tr1.W = 100;
            tr1.H = 100;
            tr1.C = MixColor(0.25f, 5f, 1f);
        }
        // tick: renders one frame
        public void Tick()
        {
            float offset = 0.00001f;
            screen.Clear(0);
            ray ray = new ray();
            circles circ = new circles();
            circ.POS = new Vector2(3, 3);
            circ.R = 1f;
            if (lightbuffer[0] < 15f)
            {
                lightbuffer[0] += 1f;
            }
            else
            {
                lightbuffer[0] -= 1f;
            }
            if (lightbuffer[2] > 15f)
            {
                lightbuffer[2] -= 1f;
            }
            else
            {
                lightbuffer[2] += 1f;
            }
            for (int i = 0; i < 639; i++)
            {
                for (int j = 0; j < 639; j++)
                {
                    floatbuffer[i, j] = new float[] { c[i, j][0], c[i, j][1], c[i, j][2] };
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 pos = new Vector2((worldsize / 639f) * i, (worldsize / 639f) * j);
                        ray.o = new Vector2(lightbuffer[k], lightbuffer[k + 1]);
                        ray.t = distanceToLight(ray, pos)-(2f*offset);
                        ray.d = normalizedDirectionToLight(ray, pos);
                        ray.o = new Vector2(lightbuffer[k]+(ray.d.X*offset), lightbuffer[k + 1]+(ray.d.X*offset));
                        for (int z = 0; z < prim.Count; z++)
                        {
                            if (ray.intersectionc(circ, ray) == false && intersectionBox(ray, pos, prim[z]) == false && intersectionTriangle(ray, pos, tr1) == false)
                            {
                                floatbuffer[i, j][0] +=  0/ (float)((ray.t * Math.PI) + 1);
                                floatbuffer[i, j][1] += 1 / (float)((ray.t * Math.PI) + 1);
                                floatbuffer[i, j][2] += 1 / (float)((ray.t * Math.PI) + 1);
                            }
                        }
                        k++;
                    }
                    screen.Plot(i, j, MixColor(MathHelper.Clamp(floatbuffer[i, j][0],0,1), MathHelper.Clamp(floatbuffer[i, j][1],0,1), MathHelper.Clamp(floatbuffer[i, j][2],0,1)));
                }
            }
            screen.Circle((int)(circ.POS.X*(639/worldsize)), (int)(circ.POS.Y*(639/worldsize)), (int)(circ.R*(639/worldsize)), MixColor(1, 0, 1));
            screen.Bar(prim[0].X1, prim[0].Y1, prim[0].X2, prim[0].Y2, prim[0].C);
            screen.Bar(prim[1].X1, prim[1].Y1, prim[1].X2, prim[1].Y2, prim[1].C);
            screen.Bar(prim[2].X1, prim[2].Y1, prim[2].X2, prim[2].Y2, prim[2].C);
            screen.Triangle(100, 500, 100, 100, tr1.C);
        }

        public float distanceToLight(ray ray, Vector2 pos)
        {
            return (pos - ray.o).Length;
        }

        public Vector2 normalizedDirectionToLight(ray ray, Vector2 pos)
        {
            return Vector2.Normalize(pos - ray.o);
        }

        int MixColor(float red, float green, float blue)
        {
            return ((int)(red * 255) << 16) + ((int)(green * 255) << 8) + (int)(blue * 255);
        }

        public bool intersectionBox(ray r, Vector2 p, box b)
        {
            //box pixelcoordinates to world coordinates
            float bx1 = b.X1 * (worldsize / 639f);
            float by1 = b.Y1 * (worldsize / 639f);
            float bx2 = b.X2 * (worldsize / 639f);
            float by2 = b.Y2 * (worldsize / 639f);

            //4 vectors, for every line of the box 1
            Vector2 blc = new Vector2(bx1, by2);
            Vector2 brc = new Vector2(bx2, by2);
            Vector2 tlc = new Vector2(bx1, by1);
            Vector2 trc = new Vector2(bx2, by1);
            //ensure that it is not inside the box. Else you have an intersection immediatly.
            if (p.X > blc.X && p.X < trc.X && p.Y < blc.Y && p.Y > trc.Y && r.o.X > blc.X && r.o.X < trc.X && r.o.Y < blc.Y && r.o.Y > trc.Y)
            {
                return true;
            }
            // check for intersection for every vector of the box.
            if (lineIntersection(r, p, tlc, blc))
                return true;
            if (lineIntersection(r, p, blc, brc))
                return true;
            if (lineIntersection(r, p, tlc, trc))
                return true;
            if (lineIntersection(r, p, trc, brc))
                return true;
            return false;
        }

        public bool intersectionTriangle(ray r, Vector2 p, triangle t)
        {
            //box pixelcoordinates to world coordinates
            float tx = t.X * (worldsize / 639f);
            float ty = t.Y * (worldsize / 639f);
            float tw = t.W * (worldsize / 639f);
            float th = t.H * (worldsize / 639f);

            //3 vectors, for every line of the triangle 1
            Vector2 blc = new Vector2(tx , ty+th);
            Vector2 brc = new Vector2(tx+tw, ty+th);
            Vector2 tc = new Vector2(tx + (tw / 2), ty);

            ////ensure that it is not inside the triangle. Else you have an intersection immediatly.
            //if (p.X > blc.X && p.X < trc.X && p.Y < blc.Y && p.Y > trc.Y && r.o.X > blc.X && r.o.X < trc.X && r.o.Y < blc.Y && r.o.Y > trc.Y)
            //{
            //    return true;
            //}

            // check for intersection for every vector of the triangle.
            if (lineIntersection(r, p, blc, tc))
                return true;
            if (lineIntersection(r, p, tc, brc))
                return true;
            if (lineIntersection(r, p, brc, blc))
                return true;
            return false;
        }

        public bool lineIntersection(ray r, Vector2 p, Vector2 v1, Vector2 v2)
        {

            float d = ((v2.Y - v1.Y) * (-r.o.X + p.X) - (v2.X - v1.X) * (-r.o.Y + p.Y));
            float n = ((v2.X - v1.X) * (r.o.Y - v1.Y) - (v2.Y - v1.Y) * (r.o.X - v1.X));
            float n2 = ((-r.o.X + p.X) * (r.o.Y - v1.Y) - (-r.o.Y + p.Y) * (r.o.X - v1.X));
            if (d == 0f)
            {
                return false;
            }
            float ua = n / d;
            float ub = n2 / d;
            return (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f);
        }

        public static void texture(float[,][] fb, float[,][] c, Surface map)
        {
            for (int i = 0; i < 639; i++)
            {
                for (int j = 0; j < 639; j++)
                {
                    c[i, j] = new float[] { 0, 0, 0 };
                    c[i, j][0] = ((float)((map.pixels[i + j * 640] & 16711680) >> 16)) / 256f;
                    c[i, j][1] = ((float)((map.pixels[i + j * 640] & 65280) >> 8)) / 256f;
                    c[i, j][2] = ((float)(map.pixels[i + j * 640] & 255)) / 256f;

                }
            }
        }
    }

    class ray
    {
        private Vector2 O, D;
        private float T;
        public Vector2 o
        {
            get { return O; }
            set { O = value; }

        }
        public Vector2 d
        {
            get { return D; }
            set { D = value; }
        }
        public float t
        {
            get { return T; }
            set { T = value; }
        }

        public bool intersectionc(circles circ, ray ray)
        {
            Vector2 c = circ.POS - ray.o;
            float t = Vector2.Dot(c, ray.d);
            Vector2 q = c - (t * ray.d);
            float p2 = Vector2.Dot(q, q);
            float tocircle = t - (float)Math.Sqrt(((circ.R * circ.R) - p2));
            if (tocircle > ray.t || t < 0)
            {
                return false;
            }
            else
            {


                if ((circ.R * circ.R) < p2) { return false; }
                else
                {
                    ray.t = t;
                    return true;
                }
            }
        }
    }

    public class primitives
    {

    }

    class circles : primitives
    {
        float r;
        Vector2 pos;
        public Vector2 POS
        {
            get { return pos; }
            set { pos = value; }
        }
        public float R
        {
            get { return r; }
            set { r = value; }
        }

    }

    class box : primitives
    {
        int x1, y1, x2, y2;
        int c;
        public int X1
        {
            get { return x1; }
            set { x1 = value; }
        }
        public int Y1
        {
            get { return y1; }
            set { y1 = value; }
        }
        public int X2
        {
            get { return x2; }
            set { x2 = value; }
        }
        public int Y2
        {
            get { return y2; }
            set { y2 = value; }
        }
        public int C
        {
            get { return c; }
            set { c = value; }
        }
    }
    class triangle : primitives
    {
        int x, y, w, h;
        int c;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int W
        {
            get { return w; }
            set { w = value; }
        }
        public int H
        {
            get { return h; }
            set { h = value; }
        }
        public int C
        {
            get { return c; }
            set { c = value; }
        }
    }
}