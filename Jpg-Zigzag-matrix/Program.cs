using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Jpg_Zigzag_matrix
{
    class Program
    {

        public int currentCount = 0;
        public int maxCount = 0;

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


        public int PixelColor(int w, int h, Bitmap bm)
        {
            Color c = bm.GetPixel(w, h);
            //Console.WriteLine("After GetPixel");
            int grayScaleInt = (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
            return grayScaleInt;

        }
        public int[] CrushInts(int[] InputArr)
        {
            int[] crushedArr = new int[InputArr.Length];
            for (int i = 0; i < InputArr.Length; i++)
            {
                crushedArr[i] = (int)((InputArr[i] / (254 / 89f)) + 10);    // ugly code. Should be 255/89, but then it gets trunciated from 2.876404494 to 2 
                                                                            // and gives wrong results. 255*(255/89f) is 98 so I'm not filling the range properly.
                                                                            // so the answer was 254/89f. Would 253/89f give better results? To be tested!
            }
            return crushedArr;
        }

        // Crush the ints into the 10-99 range
        public string CrushedString(int[] InputArr)
        {
            string output = "";
            for (int i = 0; i < InputArr.Length; i++)
            {
                //output += crushedArr[i].ToString() + ", "; // Gives "xx, xx, xx, "
                output += InputArr[i].ToString();
            }

            return output;
        }

        public void Rename(string input, string output)
        {

            try
            {
                File.Move(input, output);
            }
            catch
            {

            }


        }

        public string CreateOutputArrOnly(string Array)
        {
            string output = Array + ".jpg";

            return output;
        }

        public void Run(string fileName)
        {

            Bitmap bm = new Bitmap(fileName);
            Bitmap Resized = Resize(bm, 8); // hardcoded
            List<int> data = new List<int>();

            data.AddRange(PixelColorList(Resized));

            int[] dataArr = new int[data.Count];
            dataArr = data.ToArray();
            int[] CrushedIntArr = CrushInts(dataArr);
            string crushed = CrushedString(CrushedIntArr);

            Resized.Dispose();
            bm.Dispose();

            //Console.WriteLine("Crushed = " + crushed);


            string output = CreateOutputArrOnly(crushed);
            Rename(fileName, output);

        }

        


        public List<int> PixelColorList(Bitmap bm)
        {

            // Everything is based of GetPixel(), so everything starts at 0!!!


            List<int> result = new List<int> // PixelColor() at the required places, hardcoded
            {
                // PixelColor(int w, int h, Bitmap bm)
                PixelColor(0, 0, bm),

                PixelColor(1, 0, bm),
                PixelColor(0, 1, bm),

                PixelColor(0, 2, bm),
                PixelColor(1, 1, bm),
                PixelColor(2, 0, bm),

                PixelColor(3, 0, bm),
                PixelColor(2, 1, bm),
                PixelColor(1, 2, bm),
                PixelColor(0, 3, bm),

                PixelColor(0, 4, bm),
                PixelColor(1, 3, bm),
                PixelColor(2, 2, bm),
                PixelColor(3, 1, bm),
                PixelColor(4, 0, bm),

                PixelColor(5, 0, bm),
                PixelColor(4, 1, bm),
                PixelColor(3, 2, bm),
                PixelColor(2, 3, bm),
                PixelColor(1, 4, bm),
                PixelColor(0, 5, bm),

                PixelColor(0, 6, bm),
                PixelColor(1, 5, bm),
                PixelColor(2, 4, bm),
                PixelColor(3, 3, bm),
                PixelColor(4, 2, bm),
                PixelColor(5, 1, bm),
                PixelColor(6, 0, bm),

                PixelColor(7, 0, bm),
                PixelColor(6, 1, bm),
                PixelColor(5, 2, bm),
                PixelColor(4, 3, bm),
                PixelColor(3, 4, bm),
                PixelColor(2, 5, bm),
                PixelColor(1, 6, bm),
                PixelColor(0, 7, bm),

                PixelColor(1, 7, bm),
                PixelColor(2, 6, bm),
                PixelColor(3, 5, bm),
                PixelColor(4, 4, bm),
                PixelColor(5, 3, bm),
                PixelColor(6, 2, bm),
                PixelColor(7, 1, bm),

                PixelColor(7, 2, bm),
                PixelColor(6, 3, bm),
                PixelColor(5, 4, bm),
                PixelColor(4, 5, bm),
                PixelColor(3, 6, bm),
                PixelColor(2, 7, bm),

                PixelColor(3, 7, bm),
                PixelColor(4, 6, bm),
                PixelColor(5, 5, bm),
                PixelColor(6, 4, bm),
                PixelColor(7, 3, bm),

                PixelColor(7, 4, bm),
                PixelColor(6, 5, bm),
                PixelColor(5, 6, bm),
                PixelColor(4, 7, bm),

                PixelColor(5, 7, bm),
                PixelColor(6, 6, bm),
                PixelColor(7, 5, bm),

                PixelColor(7, 6, bm),
                PixelColor(6, 7, bm),

                PixelColor(7, 7, bm)
            };

            return result;

        }




        static void Main(string[] args)
        {
            Program p = new Program();

            
            int numberOfMaxDegreeOfParallelismInt = 6;
            string SearchPattern = "*.jpg";

            // for all the jpg files, do rename in increasing number, with padding
            // step 1: Find all jpg's in the current folder

            
            var paths = Directory.GetFiles(Directory.GetCurrentDirectory(), SearchPattern);
            int count = paths.Length;

            p.SetMaxCount(count);
            var watch = Stopwatch.StartNew();

            Parallel.ForEach(paths, new ParallelOptions { MaxDegreeOfParallelism = numberOfMaxDegreeOfParallelismInt }, (fileName) =>
            {
                var timeForOneExec = Stopwatch.StartNew();
                p.Run(fileName);
                
                timeForOneExec.Stop();

                Interlocked.Increment(ref p.currentCount);
                double timeSpanTicks = watch.ElapsedTicks;
                Double avgTimeInMS = 0;

                double avgTimeInS = ((timeSpanTicks / Stopwatch.Frequency) / p.currentCount);
                int intSec = (int)Math.Floor(avgTimeInS);

                // If average time in seconds is more than 1, remove the 1000 ms from the ms calculations. If this block is not there, times would be written as
                // 01:1xxx, where 01 is seconds and 1xxx is the number of ms. Removing 1000 ms gives the expected output of 01:xxx. 
                if (avgTimeInS >= 1)
                {
                    avgTimeInMS = (((timeSpanTicks / Stopwatch.Frequency) * 1000) / p.currentCount) - (1000 * intSec);
                }
                else
                {
                    avgTimeInMS = (((timeSpanTicks / Stopwatch.Frequency) * 1000) / p.currentCount);
                }


                TimeSpan timeS1Exec = timeForOneExec.Elapsed;

                string timeStr = String.Format("{0:00}:{1:000}", timeS1Exec.Seconds, timeS1Exec.Milliseconds);
                string avgTime = String.Format("{0:00}:{1:000}", avgTimeInS, avgTimeInMS);

                // Caclulations for the time remaning

                int totalSeconds = (int)Math.Floor((avgTimeInS * p.ImagesLeft()));

                String timeRemainStr = p.TimeRemaining(totalSeconds); // Done via own function
                TimeSpan timeSpan = TimeSpan.FromSeconds(((timeSpanTicks / Stopwatch.Frequency) / p.currentCount) * p.ImagesLeft());


                Console.WriteLine(p.PercentDone() + " done" + " | Images to go = " + p.ImagesLeft() + " | Images done this session = " + p.ImgsDone() +
                        " | Avg time taken = " + avgTime + " | Time remaning " + timeRemainStr);

            });



            
            
        }
    }
}

