using OpenTK;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Template
{
	class MyApplication
	{
        /*
         eerste elke pixelcolor 0;
         pixelcoordinaten omzetten naar x=10,y=10;
         lijn definieren naar het licht toe.
         kijken of de lijn een intersectie heeft met een object.
         wel, doe niks;
         niet, reken hoeveelheid licht uit.
         plotten.
         */
		// member variables
		public Surface screen;
        // 3 floats for the colors per pixel.
        public float[,][] floatbuffer = new float[640,640][];
        //lightsources list.
        public float[] lightbuffer = new float[4];
        // primitives list
        List<primitives> prim = new List<primitives>(); 
		// initialize
		public void Init()
		{
            lightbuffer[1] = 10f;
        }
		// tick: renders one frame
		public void Tick()
		{   
            screen.Clear( 0 );
            ray ray = new ray();
            circles circ = new circles();
            circ.POS = new Vector2(3, 3);
            circ.R = 1f;
            box b = new box();
            b.X1 = 400;
            b.X2 = 450;
            b.Y1 = 400;
            b.Y2 = 450;
            b.C = MixColor(1, 0, 1);
            prim.Add(b);
            prim.Add(circ);            
            lightbuffer[0] += 1f;
            lightbuffer[1] -= 1f;
            lightbuffer[2] += 0.5f;
            lightbuffer[3] += 1f;
            for(int i=0; i < 639; i++)
            {
                for(int j = 0; j < 639; j++)
                {
                    floatbuffer[i, j] = new float[] { 0, 0, 0 };
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 pos = new Vector2((10f / 639f) * i, (10f / 639f) * j);
                        ray.o = new Vector2(lightbuffer[k], lightbuffer[k+1]);
                        ray.t = distanceToLight(ray, pos);
                        ray.d = normalizedDirectionToLight(ray, pos);
                        if (ray.intersectionc(circ, ray) == false && intersectionBox(ray, pos, b) == false)
                            {
                            floatbuffer[i, j][0] += 0 / (float)((ray.t * Math.PI) + 1);
                            floatbuffer[i, j][1] += 1 / (float)((ray.t * Math.PI) + 1);
                            floatbuffer[i, j][2] += 1 / (float)((ray.t * Math.PI) + 1);
                        }
                        k++;
                    }
                    screen.Plot(i, j, MixColor(floatbuffer[i, j][0], floatbuffer[i, j][1], floatbuffer[i, j][2]));
                   
                }
            }
            screen.Bar(b.X1, b.Y1, b.X2, b.Y2, b.C);
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
            return ((int)(red*255) << 16) + ((int)(green*255) << 8) + (int)(blue*255);
        }

        public bool intersectionBox(ray r, Vector2 p, box b)
        {
            //box pixelcoordinates to world coordinates
            float bx1 = b.X1 * (10f/639f);
            float by1 = b.Y1 * (10f / 639f);
            float bx2 = b.X2 * (10f / 639f);
            float by2 = b.Y2 * (10f / 639f);

            //4 vectors, for every line of the box 1
            Vector2 blc = new Vector2(bx1, by2);
            Vector2 brc = new Vector2(bx2, by2);
            Vector2 tlc = new Vector2(bx1, by1);
            Vector2 trc = new Vector2(bx2, by1);
            //ensure that it is not inside the box. Else you have an intersection immediatly.
            if(p.X>blc.X && p.X<trc.X && p.Y<blc.Y && p.Y>trc.Y && r.o.X > blc.X && r.o.X < trc.X && r.o.Y < blc.Y && r.o.Y > trc.Y)
            {
                return true;
            }
            // check for intersection for every vector of the box.
            if (lineIntersection(r,p, tlc, blc))
                return true;
            if (lineIntersection(r, p, blc, brc))
                return true;
            if (lineIntersection(r, p, tlc, trc))
                return true;
            if (lineIntersection(r, p, trc, brc))
                return true;
            return false;
        }

        public bool lineIntersection(ray r, Vector2 p, Vector2 v1, Vector2 v2)
        {

            float d = ((v2.Y - v1.Y) * (-r.o.X + p.X) - (v2.X - v1.X) * (-r.o.Y + p.Y));
            float n = ((v2.X - v1.X) * (r.o.Y - v1.Y) - (v2.Y - v1.Y) * (r.o.X - v1.X));
            float n2 = ((-r.o.X + p.X) * (r.o.Y - v1.Y) - (-r.o.Y + p.Y) * (r.o.X - v1.X));
            if(d== 0f)
            {
                return false;
            }
            float ua = n / d;
            float ub = n2 / d;
            return (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f);
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
}