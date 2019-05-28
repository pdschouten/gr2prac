using OpenTK;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;

        // 3 floats for the colors per pixel.
        public static float[,][] floatbuffer = new float[640, 640][];

        //lightsources list.
        public static float[] lightbuffer = new float[10];

        // worldsize
        public static float worldsize = 20f;

        // primitives list
        public static List<primitives> prim = new List<primitives>();        
        
        //texture
        public Surface map;
        public static float[,][] c;

        //multi-thread
        public static Thread[] tarr = new Thread[8];

        // initialize
        public void Init()
        {
            //load texture and find the colors for every pixel
            map = new Surface("../../assets/tiles.png");
            c = new float[640, 640][];            texture(floatbuffer, c, map);
            
            //lights startvalues
            lightbuffer[0] = worldsize - 18f;
            lightbuffer[1] = worldsize - 19f;
            lightbuffer[2] = 0f;
            lightbuffer[3] = 1f;
            lightbuffer[4] = 1f;
            lightbuffer[5] = worldsize - 2f;
            lightbuffer[6] = worldsize - 2f;
            lightbuffer[7] = 1f;
            lightbuffer[8] = 1f;
            lightbuffer[9] = 0f;

            //box declaration            
            box b1 = new box();
            b1.X = 200;
            b1.Y = 200;
            b1.W = 50;
            b1.H = 50;
            b1.X2 = b1.X + b1.W;
            b1.Y2 = b1.Y + b1.H;
            b1.C = MixColor(0, 1, 1);
            b1.TYPE = "box";
            box b2 = new box();
            b2.X = 400;
            b2.Y = 200;
            b2.W = 50;
            b2.H = 50;
            b2.X2 = b2.X + b2.W;
            b2.Y2 = b2.Y + b2.H;
            b2.C = MixColor(0, 1, 1);
            b2.TYPE = "box";
            box b3 = new box();
            b3.X = 400;
            b3.Y = 400;
            b3.W = 50;
            b3.H = 50;
            b3.X2 = b3.X + b3.W;
            b3.Y2 = b3.Y + b3.H;
            b3.C = MixColor(1, 1, 0);
            b3.TYPE = "box";

            //triangle declaration
            triangle tr1 = new triangle();
            tr1.X = 100;
            tr1.Y = 500;
            tr1.W = 100;
            tr1.H = 100;
            tr1.XT = tr1.X + (tr1.W / 2);
            tr1.C = MixColor(0.25f, 5f, 1f);
            tr1.TYPE = "triangle";

            //circle declaration
            circle circ1 = new circle();
            circ1.X = 200;
            circ1.Y = 200;
            circ1.W = 50;
            circ1.H = circ1.W;
            circ1.POS = new Vector2(3, 3);
            circ1.R = circ1.W/2;
            circ1.TYPE = "circle";

            //add primitives to List            
            prim.Add(b1);
            prim.Add(b2);
            prim.Add(b3);
            prim.Add(tr1);
            prim.Add(circ1);

        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            ////move the lights
            //if (lightbuffer[0] < 15f)
            //{
            //    lightbuffer[0] += 1f;
            //}
            //else
            //{
            //    lightbuffer[0] -= 1f;
            //}
            //if (lightbuffer[2] > 15f)
            //{
            //    lightbuffer[2] -= 1f;
            //}
            //else
            //{
            //    lightbuffer[2] += 1f;
            //}
            //int tr = 0;
            //while(tr< tarr.Length)
            //{
            //    if (tr == 7)
            //    {
            //        tarr[tr] = new Thread(() => { plot(tr * 80, 639); });

            //    }
            //    else
            //    {
            //        tarr[tr] = new Thread(() => { plot(tr * 80, (tr + 1) * 80); });
            //    }
            //        tr++;
            //}
            //tr = 0;
            Thread t1 = new Thread(() =>
            {
                plot(0, 80);
            });
            Thread t2 = new Thread(() =>
            {
                plot(80, 160);
            });
            Thread t3 = new Thread(() =>
            {
                plot(160, 240);
            });
            Thread t4 = new Thread(() =>
            {
                plot(240, 320);
            });
            Thread t5 = new Thread(() =>
            {
                plot(320, 400);
            });
            Thread t6 = new Thread(() =>
            {
                plot(400, 480);
            });
            Thread t7 = new Thread(() =>
            {
                plot(480, 560);
            });
            Thread t8 = new Thread(() =>
            {
                plot(560, 639);
            });

            tarr[0] = t1;
            tarr[1] = t2;
            tarr[2] = t3;
            tarr[3] = t4;
            tarr[4] = t5;
            tarr[5] = t6;
            tarr[6] = t7;
            tarr[7] = t8;
            for (int td = 0; td < tarr.Length; td++)
            {
                tarr[td].Start();
            }
            for (int td = 0; td < tarr.Length; td++)
            {
                tarr[td].Join();
            }
            for (int i = 0; i < 639; i++)
            {
                for (int j = 0; j < 639; j++)
                {
                    screen.Plot(i, j, MixColor(MathHelper.Clamp(floatbuffer[i, j][0], 0, 1), MathHelper.Clamp(floatbuffer[i, j][1], 0, 1), MathHelper.Clamp(floatbuffer[i, j][2], 0, 1)));
                }
            }
            primitivesDraw();            
        }

        public void primitivesDraw()
        {
            for(int i=0; i<prim.Count; i++)
            {
                if (prim[i].TYPE == "box")
                {
                    screen.Bar(prim[i].X, prim[i].Y, prim[i].W, prim[i].H, prim[i].C);
                }
                if (prim[i].TYPE == "circle")
                {
                    screen.Circle(prim[i].X, prim[i].Y, prim[i].W, prim[i].C);

                }
                if (prim[i].TYPE == "triangle")
                {
                    screen.Triangle(prim[i].X, prim[i].Y, prim[i].W, prim[i].H, prim[i].C);
                }
            }
        }

        public void plot(int x, int y)
        {
            float offset = 0.00001f;
            ray ray = new ray();
            for (int i = 0; i < 639; i++)
            {
                for (int j = x; j < y; j++)
                {
                    floatbuffer[i, j] = new float[] { c[i, j][0], c[i, j][1], c[i, j][2] };
                    for (int k = 0; k < lightbuffer.Length; k+=5)
                    {
                        Vector2 pos = new Vector2((worldsize / 639f) * i, (worldsize / 639f) * j);
                        ray.o = new Vector2(lightbuffer[k], lightbuffer[k + 1]);
                        ray.t = distanceToLight(ray, pos) - (2f * offset);
                        ray.d = normalizedDirectionToLight(ray, pos);
                        ray.o = new Vector2(lightbuffer[k] + (ray.d.X * offset), lightbuffer[k + 1] + (ray.d.X * offset));

                        if (intersectionPrimitives(ray, pos) == false)
                        {
                            floatbuffer[i, j][0] += lightbuffer[k+2] / (float)((ray.t * Math.PI) + 1);
                            floatbuffer[i, j][1] += lightbuffer[k+3] / (float)((ray.t * Math.PI) + 1);
                            floatbuffer[i, j][2] += lightbuffer[k+4] / (float)((ray.t * Math.PI) + 1);
                        }
                    }
                }
            }
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

        public bool intersectionPrimitives(ray r, Vector2 v)
        {
            for (int z = 0; z < prim.Count; z++)
            {
                if (prim[z].TYPE == "box")
                {
                    if (intersectionBox(r, v, prim[z]) == true)
                    {
                        return true;
                    }
                }
                if (prim[z].TYPE == "circle")
                {
                    if (intersectionCircle(r, v, prim[z]) == true)
                    {
                        return true;
                    }
                }
                if (prim[z].TYPE == "triangle")
                {
                    if (intersectionTriangle(r, v, prim[z]) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool intersectionBox(ray r, Vector2 p, primitives b)
        {
            //box pixelcoordinates to world coordinates
            float bx1 = b.X * (worldsize / 639f);
            float by1 = b.Y * (worldsize / 639f);
            float bx2 = (b.X+b.W) * (worldsize / 639f);
            float by2 = (b.Y+b.H) * (worldsize / 639f);

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
            if (intersectionLine(r, p, tlc, blc))
                return true;
            if (intersectionLine(r, p, blc, brc))
                return true;
            if (intersectionLine(r, p, tlc, trc))
                return true;
            if (intersectionLine(r, p, trc, brc))
                return true;
            return false;
        }

        public bool intersectionTriangle(ray r, Vector2 v, primitives p)
        {
            //box pixelcoordinates to world coordinates
            float tx = p.X * (worldsize / 639f);
            float ty = p.Y * (worldsize / 639f);
            float tw = p.W * (worldsize / 639f);
            float th = p.H * (worldsize / 639f);

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
            if (intersectionLine(r, v, blc, tc))
                return true;
            if (intersectionLine(r, v, tc, brc))
                return true;
            if (intersectionLine(r, v, brc, blc))
                return true;
            return false;
        }

        public bool intersectionCircle(ray r, Vector2 v, primitives p)
        {
            int radius = p.W/2;
            Vector2 vec = new Vector2(p.X, p.Y);
            Vector2 c = vec - r.o;
            float t = Vector2.Dot(c, r.d);
            Vector2 q = c - (t * r.d);
            float p2 = Vector2.Dot(q, q);
            float tocircle = t - (float)Math.Sqrt(((radius * radius) - p2));
            if (tocircle > r.t || t < 0)
            {
                return false;
            }
            if ((radius * radius) < p2)
            {
                return false;
            }
            else
            {
                r.t = t;
                return true;
            }
        }

        public bool intersectionLine(ray r, Vector2 p, Vector2 v1, Vector2 v2)
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
    }

    public class primitives
    {
        int x, y, w, h, c;
        string type;

        public string TYPE
        {
            get { return type; }
            set { type = value; }
        }
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

    class circle : primitives
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
        int x2, y2;
        
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
    }
    class triangle : primitives
    {
        int xt;
        public int XT
        {
            get { return xt; }
            set { xt = value; }
        }
    }
}