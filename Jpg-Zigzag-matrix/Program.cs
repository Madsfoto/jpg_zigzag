using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

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

        static int __LINE__([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
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






        public List<int> NegDiag(int width, int height, Bitmap bm, bool evenW, bool evenH)
        {
            // start at (width, height)
            // go height + 1 and width - 1 until you hit width == 0.

            // evenW: Is the source image even in width?
            // evenH: Is the source image even in height?

            int bmWidth = bm.Width;
            int bmHeight = bm.Height;

            int w = width;
            int h = height;

            //Console.WriteLine("Input to the NegDiag() function: W = "+w + " H = " + h);


            List<int> result = new List<int>();


            // I need to do different things depending of the evenness of the source image. 
            // If evenW is true, 


            if (evenW == true && (width == 0 || width % 2 == 1))

            {
                //Console.WriteLine("Input to the IF function: W = " + w + " H = " + h);

                for (int i = 0; i <= width; i++)
                {
                    // In plain language I want:
                    //      a single pixel function for first and last pixel. 
                    //      a negative diagnoal function from (1,0) to (0,1)
                    //      a negative diagonal function from (3,0) to (2,1) to (1,2) to (0,3)
                    //      
                    // The challenge is getting the limits of the "board"/coordinates right. 
                    // Coordinates are starting from 0, ending in bmWidth-1. 

                    // it should be robust enough to start at (7,2) to (6,3) to (5,4), (4,5), (3,6) and (2,7).



                    if ((w >= 0 && h >= 0 && w < bmWidth && h < bmHeight) || (w == 0 && h == 0) || (w == bmWidth - 1 && h == bmHeight - 1)) // bmWidth and bmHeight starts at 1, so the values will always be larger than the ones get
                    {
                        Console.WriteLine("Inside 1st grid, before move: w = " + w + " | h = " + h);

                        Color c = bm.GetPixel(w, h);
                        //Console.WriteLine("After GetPixel");
                        int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
                        //Console.WriteLine("After Int");
                        result.Add(grayScaleInt);
                        //Console.WriteLine("After result.Add"+"\n");


                        //if ((w == 0 && h == 0) || (w==bmWidth && h==bmHeight)) // Would that work? 
                        
                        if ((w == 0 && h == 0) || (w == bmWidth - 1 && h == bmHeight - 1)) // If at the first or last place in the image, bug out.
                        {
                            Console.WriteLine("Inside 1st grid: w = " + w + " | h = " + h);
                            w = -1;
                            h = -1;

                        }
                        else if (w >= 0 && h >= 0) // normal operation
                        {
                            //Console.WriteLine("Normal operation");
                            if (h < bmHeight)
                            {
                                if (w - 1 < 0)
                                {
                                    //Console.WriteLine("L" + __LINE__() + " if(w-1<0 && h<bmHeight)");
                                    w = 0;
                                    h++;
                                }
                                else if (h >= bmHeight)
                                {
                                    // What should happen when you are at the height, then you're already too far acording to GetPixel().
                                    //Console.WriteLine("L" + __LINE__() + ": h>bmheight. ");
                                }
                                else
                                {
                                    w--;
                                    h++;

                                }
                                //Console.WriteLine("Inside first grid, after move:   w = " + w + " | h = " + h);
                            }
                        }
                        else
                        {
                            Console.WriteLine("H ! < bmHeight | L " + __LINE__());
                        }




                    }
                    else // Meaning we are outside of the grid
                    {

                    }


                }
                // now we are at the w==0 place, meaning we need to go height +1, and go back up to h==0.


                // TODO: Test if the resied image is even pixels (8,10,12 etc) or odd (7,9,11 etc): 
                // If bmWidth is even, then GetPixel() is odd, meaning that the w==max is negDiag. 
                //If bmWidth is odd, then Getpixel() is even, meaning that w==max should start at h==1. 


                //Console.WriteLine("After first loop\n");
                //Console.WriteLine("INSIDE THE IF");
                //Console.WriteLine("w== " + w);
                //Console.WriteLine("h== " + h + "\n");

                if (h >= bmHeight)
                {
                    //Console.WriteLine("h>bmheight. l " + __LINE__());

                    // set h to max
                }
                if (w == 0)
                {

                    //Console.WriteLine("L"+ __LINE__()+": w==0");
                    //Console.WriteLine("L" + __LINE__() + ": h==" + h);
                    if(h<bmHeight)
                    {

                    for (int i = 0; i <= width + 1; i++)
                    {
                        Console.WriteLine("Inside 2nd grid, before move: w = " + w + " | h = " + h);
                        // we have moved one down, so pixel data, then move. 
                        Color c = bm.GetPixel(w, h);
                        //Console.WriteLine("After GetPixel");
                        int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
                        //Console.WriteLine("After Int");
                        result.Add(grayScaleInt);

                        if (h > 0 && w<bmWidth) // more tests? do we need to test?
                        {

                            w++;
                            h--;

                        }
                        else if (h == 0) // at the top line, take pixel data and 
                        {
                            break;
                        }
                        else // h<0 is impossible
                        {

                        }


                    }

                    }

                }

                if (h == bmHeight - 1)
                {
                    Console.WriteLine("if (h == bmHeight - 1) | L " + __LINE__());
                    // go back
                }



            }
            else // Not even width, meaning that 
            {
                //Console.WriteLine(" ELSE OF if (evenW == true && (width == 0 || width % 2 == 1)) | L "+__LINE__());
                //Console.WriteLine("h == " + h+" | L "+__LINE__());
                //Console.WriteLine("w == " + w + " | L " + __LINE__());
                //Console.WriteLine();
            }












           



            return result;

        }


        public List<int> ZigzagFromBitmap(Bitmap bmInput)
        {
            // Input values from the grayscaled input bitmap. Used for loop limits and pixel coordinates. 
            // NOTE: GetPixel() starts at (0,0) so the loops run until the width. 

            // NOTE: The test of is it inside the grid is done inside the function.


            int bmWidth = bmInput.Width;
            int bmHeight = bmInput.Height;
            bool evenW;
            bool evenH;
            if (bmWidth % 2 == 0)
            {
                evenW = true;
            }
            else
            {
                evenW = false;
            }

            if (bmHeight % 2 == 0)
            {
                evenH = true;
            }
            else
            {
                evenH = false;
            }

            List<int> grayData = new List<int>();

            // In reality I only need to make the diagonal lines from the top line (to get 1st half of the board), then the width line for the last half.
            // That way the I don't need the single clause or any other thing. 
            // Every 2nd line in both directions, that way I am going to hit all the numbers. 
            //
            int h = 0;
            int w = 0;

            for (w = 0; w < bmWidth; w++)
            {

                // this way we get a height and width coordinate to give NegDiag()
                // if height == 0 and width / 2 has remainder 1 (meaning unequal), then run NegDiag().

                grayData.AddRange(NegDiag(w, h, bmInput, evenW, evenH));

            }
            

            // Width of an 8x8 pixel image is 8, but GetPixel() goes from 0 to 7, so in order to set the max width GetPixel() can handle, I set w here. 
            // I am not sure if it would be the correct place stylistically, I can argue it would be better in the grayData() function or here.
            // I choose it in the function as it makes more sense to do the calculation there, it's closer to where it needs to be in the end.



            //w = bmWidth;
            // For the second half of the grid, we set w to max width and go down the height (with (0,0) in the top left corner),
            // that way we get the other half of the grid: Negative diagnoal from (width,0) is 
            //If the width is equal the last 


            //for (h=0;h<= bmHeight; h++)
            //{
            //    grayData.AddRange(NegDiag((w-1), h, bmInput));
            //}

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


/* Extra if()'s:
 * //if(h==bmHeight && w==bmWidth)
            //{
            //    h = -1;
            //    w = -1;
            //}
            //if(h>7)
            //{
            //    h--;
            //}
            
            //if (w==0 && h%2==1 && h<7)
            //{
                
            //    h++;
                
            //}
            //if(h==7 && w!=7 && w%2==0)
            //{
                
            //    w++;
            //}






    // First line, do the NegDiag() things on every equal point (starting at 1 so execute at 2,4,6,8).

            //    grayData.AddRange(NegDiag(w-1, h-1, bmInput, false));



            //if (w == sqSize && h%2==0)
            //{
            //    // Last colum, hit (h==2, h==4, h==6).
            //    grayData.AddRange(NegDiag(w - 1, h - 1, bmInput));
            //}
            //if (w== sqSize && h == sqSize) // is covered by above
            //{
            //    // Last point in the grid
            //    //grayData.AddRange(NegDiag(w - 1, h - 1, bmInput, true));
            //}

            //int w = 0;









            */




//for (int i = 0; i < width+2; i++)
//{

//    if ((w >= 0 && h >= 0 && w < bmWidth && h < bmHeight)) // bmWidth and bmHeight starts at 1, so GetPixel() needs to be < bm*
//    {
//        Console.WriteLine("Inside 2nd grid, before move: w = " + w + " | h = " + h);
//        Color c = bm.GetPixel(w, h);

//        int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);

//        result.Add(grayScaleInt);



//            //Console.WriteLine("single != true && w>=0 HIT");
//            w++;
//            h--;
//            if(h==bmHeight)
//            {
//                break;
//            }


//    }
//    else // Meaning we are outside of the grid
//    {

//    }

//}
//Console.WriteLine();