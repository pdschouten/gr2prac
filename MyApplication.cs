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
        public static float[] lightbuffer = new float[20];

        // worldsize -f to f, increase to zoom out
        public static float worldsize = 2f;

        // primitives list
        public static List<primitives> prim = new List<primitives>();

        //texture
        public Surface map;
        public static float[,][] c;

        //multi-thread
        public static Thread[] tarr = new Thread[8];

        int dir = 0;

        // initialize
        public void Init()
        {
            //load texture and find the colors for every pixel
            map = new Surface("../../assets/dark.png");
            c = new float[640, 640][];            texture(floatbuffer, c, map);

            //lights startvalues: x,y,r,g,b
            lightbuffer[0] = TX(-1.5f);
            lightbuffer[1] = TY(1.5f);
            lightbuffer[2] = 0f;
            lightbuffer[3] = 15f;
            lightbuffer[4] = 15f;
            lightbuffer[5] = TX(1.5f);
            lightbuffer[6] = TY(-1.5f);
            lightbuffer[7] = 15f;
            lightbuffer[8] = 15f;
            lightbuffer[9] = 0f;
            lightbuffer[10] = TX(1.5f);
            lightbuffer[11] = TY(1.5f);
            lightbuffer[12] = 15f;
            lightbuffer[13] = 0f;
            lightbuffer[14] = 15f;
            lightbuffer[15] = TX(-1.5f);
            lightbuffer[16] = TY(-1.5f);
            lightbuffer[17] = 15f;
            lightbuffer[18] = 0f;
            lightbuffer[19] = 0f;

            //box declaration, top-left align            
            box b1 = new box();
            b1.X = -1.1f;
            b1.Y = -0.6f;
            b1.W = 0.2f;
            b1.H = 0.2f;
            b1.X2 = b1.X + b1.W;
            b1.Y2 = b1.Y + b1.H;
            b1.C = MixColor(1, 1, 0);
            b1.TYPE = "box";
            box b2 = new box();
            b2.X = -0.7f;
            b2.Y = -1f;
            b2.W = 0.6f;
            b2.H = 0.2f;
            b2.X2 = b2.X + b2.W;
            b2.Y2 = b2.Y + b2.H;
            b2.C = MixColor(1, 1, 0);
            b2.TYPE = "box";
            box b3 = new box();
            b3.X = 0.1f;
            b3.Y = -1f;
            b3.W = 0.6f;
            b3.H = 0.2f;
            b3.X2 = b3.X + b3.W;
            b3.Y2 = b3.Y + b3.H;
            b3.C = MixColor(1, 1, 0);
            b3.TYPE = "box";
            box b4 = new box();
            b4.X = 0.9f;
            b4.Y = -0.6f;
            b4.W = 0.2f;
            b4.H = 0.2f;
            b4.X2 = b4.X + b4.W;
            b4.Y2 = b4.Y + b4.H;
            b4.C = MixColor(1, 1, 0);
            b4.TYPE = "box";

            //triangle declaration, top-left bounding box align
            triangle tr1 = new triangle();
            tr1.X = -0.3f;
            tr1.Y = 0.2f;
            tr1.W = 0.6f;
            tr1.H = 0.4f;
            tr1.XT = tr1.X + (tr1.W / 2);
            tr1.C = MixColor(0.8f, 0.2f, 0.2f);
            tr1.TYPE = "triangle";

            //circle declaration, center align
            circle circ1 = new circle();
            circ1.X = -0.6f;
            circ1.Y = 0.8f;
            circ1.W = 0.5f;
            circ1.H = circ1.W;
            circ1.POS = new Vector2(circ1.X, circ1.Y);
            circ1.R = circ1.W / 2;
            circ1.C = MixColor(1, 0.5f, 0.5f);
            circ1.TYPE = "circle";

            circle circ2 = new circle();
            circ2.X = 0.6f;
            circ2.Y = 0.8f;
            circ2.W = 0.5f;
            circ2.H = circ1.W;
            circ2.POS = new Vector2(circ2.X, circ2.Y);
            circ2.R = circ1.W / 2;
            circ2.C = MixColor(1, 0.5f, 0.5f);
            circ2.TYPE = "circle";            

            //add primitives to List            
            prim.Add(b1);
            prim.Add(b2);
            prim.Add(b3);
            prim.Add(b4);
            prim.Add(tr1);
            prim.Add(circ1);
            prim.Add(circ2);

        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            //move the lights            
            if (dir%2 == 0)
            {
                lightbuffer[0] += TW(0.1f);
                lightbuffer[5] -= TW(0.1f);
                lightbuffer[11] += TW(0.1f);
                lightbuffer[16] -= TW(0.1f);
            }
            else
            {
                lightbuffer[0] -= TW(0.1f);
                lightbuffer[5] += TW(0.1f);
                lightbuffer[11] -= TW(0.1f);
                lightbuffer[16] += TW(0.1f);
            }
            if (TW(worldsize) < lightbuffer[0] || lightbuffer[0] < TX(-worldsize))
            {
                dir++;
            } 
            Thread t1 = new Thread(() =>
            {
                plot(TX((-worldsize + (0f / 8f * worldsize *2))), TX((-worldsize + (1f / 8f * worldsize *2))));
            });
            Thread t2 = new Thread(() =>
            {
                plot(TX((-worldsize + (1f / 8f * worldsize *2))), TX((-worldsize + (2f / 8f * worldsize *2))));
            });
            Thread t3 = new Thread(() =>
            {
                plot(TX((-worldsize + (2f / 8f * worldsize *2))), TX((-worldsize + (3f / 8f * worldsize *2))));
            });
            Thread t4 = new Thread(() =>
            {
                plot(TX((-worldsize + (3f / 8f * worldsize *2))), TX((-worldsize + (4f / 8f * worldsize *2))));
            });
            Thread t5 = new Thread(() =>
            {
                plot(TX((-worldsize + (4f / 8f * worldsize *2))), TX((-worldsize + (5f / 8f * worldsize *2))));
            });
            Thread t6 = new Thread(() =>
            {
                plot(TX((-worldsize + (5f / 8f * worldsize *2))), TX((-worldsize + (6f / 8f * worldsize *2))));
            });
            Thread t7 = new Thread(() =>
            {
                plot(TX((-worldsize + (6f / 8f * worldsize *2))), TX((-worldsize + (7f / 8f * worldsize *2))));
            });
            Thread t8 = new Thread(() =>
            {
                plot(TX((-worldsize + (7f / 8f * worldsize *2))), (TX((-worldsize + (8f / 8f * worldsize *2))))-1);
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
            for (int i = 0; i < (screen.width-1); i++)
            {
                for (int j = 0; j < (screen.height-1); j++)
                {
                    screen.Plot(i, j, MixColor(MathHelper.Clamp(floatbuffer[i, j][0], 0, 1), MathHelper.Clamp(floatbuffer[i, j][1], 0, 1), MathHelper.Clamp(floatbuffer[i, j][2], 0, 1)));
                }
            }
            primitivesDraw();
        }

        public void primitivesDraw()
        {
            for (int i = 0; i < prim.Count; i++)
            {
                if (prim[i].TYPE == "box")
                {
                    screen.Bar(TX(prim[i].X), TY(prim[i].Y), TX(prim[i].X)+TW(prim[i].W), TY(prim[i].Y)+TW(prim[i].H), prim[i].C);
                }
                if (prim[i].TYPE == "circle")
                {
                    screen.Circle(TX(prim[i].X), TY(prim[i].Y), TW(prim[i].W), prim[i].C);

                }
                if (prim[i].TYPE == "triangle")
                {
                    screen.Triangle(TX(prim[i].X), TY(prim[i].Y), TW(prim[i].W), TW(prim[i].H), prim[i].C);
                }
            }
        }

        public void plot(int x, int y)
        {
            float offset = 0.00001f;
            ray ray = new ray();
            for (int i = 0; i < (screen.width-1); i++)
            {
                for (int j = x; j < y; j++)
                {
                    floatbuffer[i, j] = new float[] { c[i, j][0], c[i, j][1], c[i, j][2] };
                    for (int k = 0; k < lightbuffer.Length; k += 5)
                    {
                        Vector2 pos = new Vector2(i, j);
                        ray.o = new Vector2(lightbuffer[k], lightbuffer[k + 1]);
                        ray.t = distanceToLight(ray, pos) - (2f * offset);
                        ray.d = normalizedDirectionToLight(ray, pos);
                        ray.o = new Vector2(lightbuffer[k] + (ray.d.X * offset), lightbuffer[k + 1] + (ray.d.X * offset));
                        

                        if (intersectionPrimitives(ray, pos) == false)
                        {
                            //Console.WriteLine("i= " + i + " j= " + j + "" + intersectionPrimitives(ray, pos));
                            floatbuffer[i, j][0] += lightbuffer[k + 2] / (float)((ray.t) + 1);
                            floatbuffer[i, j][1] += lightbuffer[k + 3] / (float)((ray.t) + 1);
                            floatbuffer[i, j][2] += lightbuffer[k + 4] / (float)((ray.t) + 1);
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
            float bx1 = TX(b.X) ;
            float by1 = TY(b.Y) ;
            float bx2 = (TX(b.X)) + (TW(b.W)) ;
            float by2 = (TY(b.Y)) + (TW(b.H)) ;

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
            float tx = TX(p.X);
            float ty = TY(p.Y);
            float tw = TW(p.W);
            float th = TW(p.H);

            //3 vectors, for every line of the triangle 1
            Vector2 blc = new Vector2(tx, ty + th);
            Vector2 brc = new Vector2(tx + tw, ty + th);
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
            float radius = ((TW(p.W) / 2));
            Vector2 vec = new Vector2(TX(p.X), TY(p.Y));
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
        //public void Test()
        //{
        //    // top-left corner
        //    float x1 = -1f, y1 = 0.5f;
        //    float rx1 = (float)(x1 * Math.Cos(a) - y1 * Math.Sin(a));
        //    float ry1 = (float)(x1 * Math.Sin(a) + y1 * Math.Cos(a));

        //    // top-right corner
        //    float x2 = 1f, y2 = 0.5f;
        //    float rx2 = (float)(x2 * Math.Cos(a) - y2 * Math.Sin(a));
        //    float ry2 = (float)(x2 * Math.Sin(a) + y2 * Math.Cos(a));


        //    // bottom-right corner
        //    float x3 = 1f, y3 = -0.5f;
        //    float rx3 = (float)(x3 * Math.Cos(a) - y3 * Math.Sin(a));
        //    float ry3 = (float)(x3 * Math.Sin(a) + y3 * Math.Cos(a));


        //    // bottom-left corner
        //    float x4 = -1f, y4 = -0.5f;
        //    float rx4 = (float)(x4 * Math.Cos(a) - y4 * Math.Sin(a));
        //    float ry4 = (float)(x4 * Math.Sin(a) + y4 * Math.Cos(a));

        //    int intx1 = TX(rx1);
        //    int inty1 = TY(ry1);
        //    int intx2 = TX(rx2);
        //    int inty2 = TY(ry2);
        //    int intx3 = TX(rx3);
        //    int inty3 = TY(ry3);
        //    int intx4 = TX(rx4);
        //    int inty4 = TY(ry4);

        //    screen.Line(intx1, inty1, intx2, inty2, 0xff0000);
        //    screen.Line(intx2, inty2, intx3, inty3, 0x00ff00);
        //    screen.Line(intx3, inty3, intx4, inty4, 0x0000ff);
        //    screen.Line(intx4, inty4, intx1, inty1, 0xffff00);
        //}
        int TX(float x)
        {
            float xorigin = 0f;
            float xshift = worldsize;
            float scale = 0f;

            if (screen.width < screen.height)
            {
                scale = screen.width;
            }
            else
            {
                scale = screen.height;
            }

            x = (x + xshift + xorigin) * scale / (xshift * 2);
            x = x + ((screen.width - scale) / 2);
            return (int)x;
        }

        int TY(float y)
        {
            float yorigin = 0f;
            float yshift = worldsize;
            float scale = 0f;

            if (screen.width < screen.height)
            {
                scale = screen.width;
            }
            else
            {
                scale = screen.height;
            }

            y = ((y - yshift - yorigin) * -1) * scale / (yshift * 2);
            y = y + ((screen.height - scale) / 2);
            return (int)y;
        }
        
        int TW(float w)
        {
            float scale = 0f;

            if (screen.width < screen.height)
            {
                scale = screen.width;
            }
            else
            {
                scale = screen.height;
            }
            w = w / (2 * worldsize / scale);
            return (int)w;
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
        float x, y, w, h;
        string type;
        int c;

        public string TYPE
        {
            get { return type; }
            set { type = value; }
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float W
        {
            get { return w; }
            set { w = value; }
        }
        public float H
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
        float x2, y2;
        
        public float X2
        {
            get { return x2; }
            set { x2 = value; }
        }
        public float Y2
        {
            get { return y2; }
            set { y2 = value; }
        }
    }
    class triangle : primitives
    {
        float xt;
        public float XT
        {
            get { return xt; }
            set { xt = value; }
        }
    }
}