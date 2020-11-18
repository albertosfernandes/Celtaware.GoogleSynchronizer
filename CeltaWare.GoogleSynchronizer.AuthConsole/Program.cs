using Celtaware.GoogleSynchronizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeltaWare.GoogleSynchronizer.AuthConsole
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine("Pressione Enter para solicitar autenticação");
            Console.ReadLine();
            ModelGoogleDrive.GetFolders("GDrive");
        }
    }
}
