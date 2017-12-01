using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigFileWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Write("Enter file directory path (example: C:\\Files):");
            String enteredPath = Console.ReadLine();
            Console.WriteLine(enteredPath);
            Console.Write("Enter file name (example: bigFile):");
            String enteredFileName = Console.ReadLine();
            Console.WriteLine(enteredFileName);

            long fileSize = 2 * 1024 * 1024 * 1024L; // size in bytes
            int numbersCount = (int)(fileSize / 4);
            String filePath = enteredPath + "\\" + enteredFileName;
          
            FileWriter fw = new FileWriter(numbersCount, filePath);
            fw.writeToFile();
            Console.ReadKey();
            
        }
    }
}
