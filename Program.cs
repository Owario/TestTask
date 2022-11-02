using System;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ListRandom myList = new ListRandom();
            myList.PushBack("a");
            myList.PushBack("b");
            myList.PushBack("c");
            myList.PushBack("d");
            myList.PushBack("e");
            myList.PushBack("f");
            myList.UpdateDataValid();
            myList.PrintList();
            
            
            Console.WriteLine("Preprocess done!");

            var path = "randomList.bin";
            
            File.WriteAllText(path,string.Empty);


            using (FileStream sourceStream = File.Open(path, FileMode.Open))
            {
                myList.Serialize(sourceStream);
            }

            ListRandom newList = new ListRandom();
            
            
            using (FileStream sourceStream = File.Open(path, FileMode.Open))
            {
                newList.Deserialize(sourceStream);
                newList.PrintList();
            }
        }
    }
    

    
}