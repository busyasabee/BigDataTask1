using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigFileWriter
{
    class FileWriter
    {
        int numbersCount;
        String path;

        public FileWriter(int numbersCount, String path)
        {
            this.numbersCount = numbersCount;
            this.path = path;
        }

        public void writeToFile()
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
                {
                    Random random = new Random();

                    for (int i = 0; i < numbersCount; i++)
                    {
                        uint thirtyBits = (uint)random.Next(1 << 30);
                        uint twoBits = (uint)random.Next(1 << 2);
                        uint fullRange = (thirtyBits << 2) | twoBits;
                        writer.Write(fullRange);
                    }

                    Console.WriteLine("File have been written");
                }

            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Not found directoty in path: " + path);
               
            }
            
        }
    }
}
