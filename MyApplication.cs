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
        public float[] lightbuffer = new float[2];
		// initialize
		public void Init()
		{
        }
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
            screen.Line(100, 100, 100, 200, 120);
            for(int i=0; i < 640; i++)
            {
                for(int j = 0; j < 640; j++)
                {
                    floatbuffer[i, j] = new float[] { 0, 0, 0 };
                }
            }
            screen.Print(floatbuffer[400, 400][1].ToString(), 100, 100, 255);

        }


	}

    class ray
    {

    }
}