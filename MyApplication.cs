using OpenTK;
using System;

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
        public float[] lightbuffer = new float[4];
		// initialize
		public void Init()
		{
        }
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
            ray ray = new ray();
            circles circ = new circles();
            circ.POS = new Vector2(3, 3);
            circ.R = 2f;
            lightbuffer[0] = 5;
            lightbuffer[1] = 5;
            for(int i=0; i < 639; i++)
            {
                for(int j = 0; j < 639; j++)
                {
                    floatbuffer[i, j] = new float[] { 0, 0, 0 };
                    for (int k = 0; k < 1; k++)
                    {
                        Vector2 pos = new Vector2((10f / 639f) * i, (10f / 639f) * j);
                        ray.o = new Vector2(lightbuffer[k], lightbuffer[k+1]);
                        ray.t = distanceToLight(ray, pos);
                        ray.d = normalizedDirectionToLight(ray, pos);
                        if (ray.intersection(circ, ray) == false)
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
            float g = 0;
            while (g < Math.PI * 2)
            {
                int x = (int)(circ.POS.X + circ.R * (float)Math.Cos(g)) * 639 / 10;
                int y = (int)(circ.POS.Y + circ.R * (float)Math.Sin(g)) * 639 / 10;
                screen.Plot(x, y, 255*255);
                g += (float)Math.PI * 2 / 360;
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
            return ((int)(red*255) << 16) + ((int)(green*255) << 8) + (int)(blue*255);
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

        public bool intersection(circles circ, ray ray)
        {
            Vector2 c = circ.POS - ray.o;
            float t = Vector2.Dot(c, ray.d);
            if (t > ray.t || t<0)
            {
                return false;
            }
            else
            {
                Vector2 q = c - t * ray.d;
                float p2 = Vector2.Dot(q, q);
                if ((circ.R * circ.R) < p2) { return false; }
                else
                {
                    return true;
                }
            }
        }
    }

    class circles
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
}