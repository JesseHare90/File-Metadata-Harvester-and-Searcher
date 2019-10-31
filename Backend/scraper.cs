using System;
using System.IO;
using System.Collections.Generic;

namespace myApp
{   
    

    class Program
    {
        

        //checks input, if y/Y or n/N, return y or n respectively, otherwise returns "invalid entry"
        public static string YesOrNo(string input){
            string TempInput = input.ToLower();
            if((TempInput == "y")||(TempInput == "n")){
                return TempInput;
            }
            else{
                TempInput = "invalid entry";
                return TempInput;
            }
        }
        //used to perform a recursive search for files in a specified directory, will ignore files/directories that throw access errors
        //returns an array of filepaths for each file found
        public static string[] FindAllFiles(string rootDir) {
            var pathsToSearch = new Queue<string>();
            var foundFiles = new List<string>();

            pathsToSearch.Enqueue(rootDir);

            while (pathsToSearch.Count > 0) {
                var dir = pathsToSearch.Dequeue();

                try {
                    var files = Directory.GetFiles(dir);
                    foreach (var file in Directory.GetFiles(dir)) {
                        foundFiles.Add(file);
                    }

                    foreach (var subDir in Directory.GetDirectories(dir)) {
                        pathsToSearch.Enqueue(subDir);
                    }

                } catch (Exception /* TODO: catch correct exception */) {
                    // Swallow.  Gulp!
                }
            }

            return foundFiles.ToArray();
        }

        public static string GetMetadata(string file){
            string FileType = Path.GetExtension(file);
            string FileName = Path.GetFileNameWithoutExtension(file);
            string FileDir = Path.GetDirectoryName(file);
            return "FileName: "+FileName+", FileType: "+FileType+", Directory: "+FileDir;

        }


        
        static void Main(string[] args)
        {
           //check that the correct number of args have been supplied - the program itself and the directory to scrape
           //if incorrect number of args, print error message and exit
            if (args.Length == 1 | args.Length > 2){
                Console.WriteLine("ERROR: Only require a valid filepath for execution");
            }
            //otherwise, check filepath is valid, and ask user whether they would like to scrap file metadata from the specified directory
            //recursively or non-recursively
            else{
                //check if provided filepath is valid and exists on the filesystem
                //if not, catch exception and exit
                try{
                    FileAttributes TempAttr = File.GetAttributes(args[1]);
                }
                catch (FileNotFoundException ){
                    Console.WriteLine("Error, File not found!");
                    System.Environment.Exit(1);

                }
                FileAttributes attr = File.GetAttributes(args[1]);
                if ((attr.HasFlag(FileAttributes.Directory))){
                    //if so, populate with list of files contained within, otherwise exit with error message
                    Console.WriteLine("You have chosen to scan the directory, "+args[1]);
                    Console.WriteLine("Do you wish to scan this directory recursively? (Y for yes, N for No)");
                    string UserInput = Console.ReadLine();
                    string UserInputChoice = YesOrNo(UserInput);
                    while (UserInputChoice == "invalid entry"){
                        Console.WriteLine("Please choose y for yes, n for no, to scan recursively");
                        UserInput = Console.ReadLine();
                        UserInputChoice = YesOrNo(UserInput);
                    }
                    if (UserInputChoice == "y"){
                        //Console.WriteLine("chose recursive");
                        Console.WriteLine("Scanning the directory: "+args[1]+" for files recursively...");
                        string[] files = FindAllFiles(args[1]);
                        foreach (string file in files){
                            Console.WriteLine(file);
                        }

                    }
                    else if (UserInputChoice == "n"){
                        //Console.WriteLine("chose non recursive");
                        Console.WriteLine("Scanning the directory: "+args[1]+" for file non-recursively...");
                        string[] files = Directory.GetFiles(args[1],"*.*");
                        foreach (string file in files){
                            Console.WriteLine(file);
                            string fileData = GetMetadata(file);
                            Console.WriteLine(fileData);
                        }
                    }
                    //Console.WriteLine(UserInputChoice);
                }
                else{
                    Console.WriteLine("ERROR: Please provide a valid directory");
                    
                }
            
            
            
            }
        }
    }
}
