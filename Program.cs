using System;
using System.IO;

namespace LabLogicTest
{
    class Program
    {

        static void DisplayMenu() {

            Console.WriteLine("---------------");
            Console.WriteLine("1. Display Tree");
            Console.WriteLine("2. Add Node");
            Console.WriteLine("3. Delete Node");
            Console.WriteLine("4. Move Node");
            Console.WriteLine("5. Search Node");
            Console.WriteLine("---------------");
            Console.WriteLine(" >> ");
        }

        static void Main(string[] args)
        {
            RootNode rootNode = new RootNode(@"C:\Users\KJ\Desktop\FileSystem");

            while (true) {

                try
                {
                    rootNode.Display();

                    DisplayMenu();

                    string option = Console.ReadLine();

                    if (option == "1")
                    {
                        continue;
                    }

                    else if (option == "2")
                    {
                        Console.WriteLine("Enter Path:");
                        string path = Console.ReadLine();

                        Console.WriteLine("Enter Name:");
                        string name = Console.ReadLine();

                        Console.WriteLine("Enter Type (1.Folder, 2.Item):");
                        string type = Console.ReadLine();

                        Node node = rootNode.Add(path.Trim(), name.Trim(), Convert.ToInt32(type));

                        Console.WriteLine("Node added successfully:", node.Path);
                    }

                    else if (option == "3")
                    {
                        Console.WriteLine("Enter Path:");
                        string path = Console.ReadLine();

                        Node node = rootNode.Delete(path.Trim());

                        Console.WriteLine("Node deleted successfully:", node.Path);
                    }

                    else if (option == "4")
                    {
                        Console.WriteLine("Enter Source Path:");
                        string sourcePath = Console.ReadLine();

                        Console.WriteLine("Enter Destination Path:");
                        string destPath = Console.ReadLine();

                        Node node = rootNode.Move(sourcePath.Trim(), destPath.Trim());

                        Console.WriteLine("Node moved successfully:", node.Path);
                    }

                    else if (option == "5")
                    {
                        Console.WriteLine("Enter Search String:");
                        string search = Console.ReadLine();

                        int count = rootNode.Search(search);

                        Console.WriteLine($"{count} Nodes found:");
                    }

                    else
                    {
                        Console.WriteLine("error: invalid option, please try again");
                    }
                }
                catch (Exception ex) {

                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
