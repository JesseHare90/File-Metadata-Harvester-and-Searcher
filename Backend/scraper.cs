using System;
using System.IO;

namespace myApp
{   
    

    class Program
    {
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
                catch (FileNotFoundException ex){
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

                    }
                    else if (UserInputChoice == "n"){
                        //Console.WriteLine("chose non recursive");
                        Console.WriteLine("Scanning the directory: "+args[1]+" for file non-recursively...");
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
