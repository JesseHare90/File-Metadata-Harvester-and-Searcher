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
            if (args.Length == 1 | args.Length > 2){
                Console.WriteLine("ERROR: Only require a valid filepath for execution");
            }
            else{
                //check if provided filepath is valid and exists on the filesystem
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
            
            
            /* 
            //get the directory you wish to scan for metadata
            Console.WriteLine("Please enter your search directory");
            string c = Console.ReadLine();
            Console.WriteLine("you have chosen to scan the directory: "+c);
            Console.WriteLine("Press enter to continue, any key to quit");
            string choice = Console.ReadLine();
            if(choice == ""){
                Console.WriteLine("Do you wish to perform a recursive or non-recursive search?");

                
            }
            else{
                Console.WriteLine("Exiting Program");
            }
            */
            }
        }
    }
}
