using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Jpg_Zigzag_matrix
{
    class Program
    {
        
        public int currentCount = 0;
        public int maxCount = 0;
        public int squareSize = 8;

        public string PercentDone()
        {
            float currentCountFloat = currentCount;
            float percentdone = (currentCountFloat / maxCount);
            string percStr = percentdone.ToString("P");
            return percStr;
        }

        public int ImagesLeft()
        {
            int imgsLeft = maxCount - currentCount;
            return imgsLeft;
        }

        public int ImgsDone()
        {
            return currentCount;
        }

        public string TimeRemaining(int seconds)
        {
            int daysLeft = 0;
            int hoursLeft = 0;
            int minutesLeft = 0;
            int secondsLeft = 0;
            string timeRemainStr = "";

            int totalIntInS = seconds;

            Double daysLeftDB = 0;
            daysLeftDB = Math.Floor((double)totalIntInS / 60 / 60 / 24);
            daysLeft = (int)daysLeftDB;

            totalIntInS = totalIntInS - (daysLeft * 60 * 60 * 24);

            Double hoursLeftDB = 0;
            hoursLeftDB = Math.Floor((double)totalIntInS / 60 / 60);
            hoursLeft = (int)hoursLeftDB;

            totalIntInS = totalIntInS - (hoursLeft * 60 * 60);

            Double minutesLeftDB = 0;
            minutesLeftDB = Math.Floor((double)totalIntInS / 60);
            minutesLeft = (int)minutesLeftDB;

            totalIntInS = totalIntInS - (minutesLeft * 60);

            secondsLeft = totalIntInS;

            return timeRemainStr = String.Format("{0:00} days, {1:00} hours, {2:00} minutes and {3:00} seconds", daysLeft, hoursLeft, minutesLeft, secondsLeft);
        }
        public void SetMaxCount(int count)
        {
            maxCount = count;
        }

       
      

        public Bitmap Resize(Bitmap image, int new_width)
        {
            Bitmap new_image = new Bitmap(new_width, new_width);
            Graphics graphics = Graphics.FromImage(new_image);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(image, 0, 0, new_width, new_width);
            graphics.Dispose();

            return new_image;
        }

        




        public List<int> NegDiag(int width, int height, Bitmap bm, bool single)
        {
            // start at (width, height)
            // go height + 1 and width - 1 until you hit width == 0.
            
            int bmWidth = bm.Width;
            int bmHeight = bm.Height;
            
            int w = width;
            int h = height;
            //Console.WriteLine("Input to the NegDiag() function: W = "+w + " H = " + h);
            
            
            List<int> result = new List<int>();
            
            

            for (int i=0;i<=width;i++) 
            {
                // In plain language I want:
                //      a single pixel function for first and last pixel. 
                //      a negative diagnoal function from (1,0) to (0,1)
                //      a negative diagonal function from (3,0) to (2,1) to (1,2) to (0,3)
                //      
                // The challenge is getting the limits of the "board"/coordinates right. 
                // Coordinates are starting from 0, ending in 7. 
                
                // it should be robust enough to start at (7,2) to (6,3) to (5,4), (4,5), (3,6) and (2,7).

                // 
                // boundry check

                if (w>=0 && h>=0 && w< bmWidth && h< bmHeight || single==true) // bmWidth and bmHeight starts at 1, so the values will always be larger than the ones get
                {
                    Console.WriteLine("Inside 1st grid, before move: w = " + w + " | h = " + h);
                    
                    Color c = bm.GetPixel(w, h);
                    //Console.WriteLine("After GetPixel");
                    int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
                    //Console.WriteLine("After Int");
                    result.Add(grayScaleInt);
                    //Console.WriteLine("After result.Add"+"\n");

                    if (single!=true && w>0 && h>=0) // If it is normal operation, single has NOT been set to true
                    {
                        w--;
                        h++;
                        //Console.WriteLine("Inside first grid, after move:   w = " + w + " | h = " + h);
                    }
                    else if (single==true) // Only a single 
                    {
                        //Console.WriteLine("First loop: SINGLE == TRUE");
                        break;
                        // make no progress.
                    }
                }
                else // Meaning we are outside of the grid
                {
                    
                }


            }

            // In reality I can just call the PosDiag() function from here, I know the next pixel to get (as I know the change between the two functions,
            // are height +1, then all the way back up until h=1 (first row).
            // and I know the coordinates 



            //Console.WriteLine("After first loop\n");

            // First check if we are at the end of the first half above. 
            if(single==true)
            {
                h=-1;
                w=-1;
                single = false;
            }
            if(h==bmHeight && w==bmWidth)
            {
                h = -1;
                w = -1;
            }
            if(h>7)
            {
                h--;
            }
            
            if (w==0 && h%2==1 && h<7)
            {
                
                h++;
                
            }
            if(h==7 && w!=7 && w%2==0)
            {
                
                w++;
            }
            
            
          

            for (int i = 0; i < width+2; i++)
            {
               
                if ((w >= 0 && h >= 0 && w < bmWidth && h < bmHeight) || single == true) // bmWidth and bmHeight starts at 1, so GetPixel() needs to be < bm*
                {
                    Console.WriteLine("Inside 2nd grid, before move: w = " + w + " | h = " + h);
                    Color c = bm.GetPixel(w, h);
                    
                    int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
                    
                    result.Add(grayScaleInt);
                    

                    if (single != true ) // If it is normal operation, single has NOT been set to true
                    {
                        //Console.WriteLine("single != true && w>=0 HIT");
                        w++;
                        h--;
                        if(h==8)
                        {
                            break;
                        }
                    }
                    else if (single == true) // Only a single 
                    {
                        
                        break;
                        // make no progress.
                    }
                }
                else // Meaning we are outside of the grid
                {
                    
                }

            }




            return result;

        }

       
        public List<int> ZigzagFromBitmap(Bitmap bmInput)
        {
            // the input bitmap is grayscale and resized, so we know the size of the array, it is square of the width.
            int sqSize = bmInput.Width;
            List<int> grayData = new List<int>();

            // In reality I only need to make the diagonal lines from the top line (to get 1st half of the board), then the width line for the last half.
            // That way the I don't need the single clause or any other thing. 
            // Every 2nd line in both directions, that way I am going to hit all the numbers. 
            //

            for (int h=1;h<= sqSize; h++)
            {
                for(int w=1;w<= sqSize; w++)
                {
                    // this way we get a height and width coordinate to give NegDiag() and PosDiag()
                    // if height == 0 and width / 2 has remainder 1 (meaning unequal), then run NegDiag().
                    if (h==1 && w ==1)
                    {
                       grayData.AddRange(NegDiag(w - 1, h - 1, bmInput, true));
                    }
                    if ((h == 1 && w %2==0) )
                    {
                        // First line, do the NegDiag() things on every equal point (starting at 1 so execute at 2,4,6,8).
                        
                        grayData.AddRange(NegDiag(w-1, h-1, bmInput, false));
                        
                    
                    }
                    
                    if (w == sqSize && h%2==0)
                    {
                        // Last colum, hit (h==2, h==4, h==6).
                        grayData.AddRange(NegDiag(w - 1, h - 1, bmInput, false));
                    }
                    //if (w== sqSize && h == sqSize) // is covered by above
                    //{
                    //    // Last point in the grid
                    //    //grayData.AddRange(NegDiag(w - 1, h - 1, bmInput, true));
                    //}
                }
            }
            //foreach (int i in grayData)
            //{
            //    System.Console.WriteLine(i);
            //}




            return grayData;
        }


        static void Main(string[] args)
        {
            Program p = new Program();
            Bitmap bm = new Bitmap(args[0]);
            
            // resize to 8x8 or 10x10
            Bitmap Resized = p.Resize(bm, p.squareSize);
            // Grayscale resized bitmap
            

            // we now have a sized and grayscale bitmap. The plan is then to get the gray values in the zigzag pattern like JPG is compressed,
            // meaning that the pixels that we should get are (0,0) (top left), (0,1), (1,0), (2,0), (1,1), (0,2), (0,3) etc. https://en.wikipedia.org/wiki/File:JPEG_ZigZag.svg

            // The pixel data is returned as an int array: 
            List<int> data = new List<int>();
            
            data.AddRange(p.ZigzagFromBitmap(Resized));
            Console.WriteLine("Contents of dataint= ");
            foreach (int dataint in data)
            {

                Console.Write(dataint + " ");
            }
            

        }
    }
}
