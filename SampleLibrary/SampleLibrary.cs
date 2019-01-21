namespace HelloWorld
{
    using System;

    public class SampleLibrary
    {
        public static void SayHello()
        {
            string temp = System.Configuration.ConfigurationManager.AppSettings["test"];
            Console.WriteLine(temp);
            
            Console.WriteLine("Hello from SampleLibrary.");
        }
    }
}
