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
            lightbuffer[0] = 5;
            lightbuffer[1] = 5;
            lightbuffer[2] = 4;
            lightbuffer[3] = 4;
            for(int i=0; i < 639; i++)
            {
                for(int j = 0; j < 639; j++)
                {
                   
                    floatbuffer[i, j] = new float[] { 0, 0, 0 };
                    for (int k = 0; k < 1; k++)
                    {
                        Vector2 lightv = new Vector2(lightbuffer[k], lightbuffer[k+1]);
                        ray.o = new Vector2((10f / 639f) * i, (10f / 639f) * j);
                        ray.t = distanceToLight(ray, lightv);
                        floatbuffer[i, j][0] += 0 / (float)((ray.t * Math.PI) + 1);
                        floatbuffer[i, j][1] += 1 / (float)((ray.t * Math.PI) + 1);
                        floatbuffer[i, j][2] += 1 / (float)((ray.t * Math.PI) + 1);
                        k++;
                    }
                    screen.Plot(i, j, MixColor(floatbuffer[i, j][0], floatbuffer[i, j][1], floatbuffer[i, j][2]));
                }
            }

            //screen.Print(floatbuffer[400, 400][2].ToString(), 100, 100, 255);

        }

        public float distanceToLight(ray ray, Vector2 lightv)
        {
           return (float)Math.Sqrt(Convert.ToSingle((lightv - ray.o).X * (lightv - ray.o).X + (lightv - ray.o).Y * (lightv - ray.o).Y));
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
    }
}