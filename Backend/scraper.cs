using System;
using System.IO;
using System.Collections.Generic;
using MetadataExtractor;




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
                    var files = System.IO.Directory.GetFiles(dir);
                    foreach (var file in System.IO.Directory.GetFiles(dir)) {
                        foundFiles.Add(file);
                    }

                    foreach (var subDir in System.IO.Directory.GetDirectories(dir)) {
                        pathsToSearch.Enqueue(subDir);
                    }

                } catch (Exception /* TODO: catch correct exception */) {
                    // Swallow.  Gulp!
                }
            }

            return foundFiles.ToArray();
        }

        //This function is used to extract metadata from each file, and returning it as a string
        public static string GetMetadata(string file){
            string[] ignoredAttributes = {"File Name","File Size","File Modified Date","Detected File Type Name","Detected File Type Long Name","Expected File Name Extension"};
            string FileType = Path.GetExtension(file);
            string FileName = Path.GetFileNameWithoutExtension(file);
            string FileDir = Path.GetDirectoryName(file);
            FileInfo FileData = new FileInfo(file);
            long size = FileData.Length;
            DateTime CreationTime = FileData.CreationTime;
            DateTime AccessTime = FileData.LastAccessTime;
            DateTime UpdatedTime = FileData.LastWriteTime;
            Console.WriteLine(file);
            //We will catch any exceptions thrown due to incompatibilities of certain files with the ImageMetadataReader
            //If an exception is thrown, we will just stick to the basic set of attributes already gathered
            // otherwise, we will gather the extended list of attributes from files that are compatible
            try{
                IEnumerable<MetadataExtractor.Directory> directoriesTest = ImageMetadataReader.ReadMetadata(file);
            }
            catch(MetadataExtractor.ImageProcessingException){
                //need to figure out a way to get the file owner
                string fileMetaDataBasic = "FileName:"+FileName+",FileType:"+FileType+",Size:"+size+",Directory:"+FileDir+",Created:"+CreationTime+",Accessed:"+AccessTime+",Updated:"+UpdatedTime;
            
                // now we have the basic generic file metadata, we will add extra metadata for each file depending on it's type
                return fileMetaDataBasic;    
            }
            
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file);
            foreach (var directory in directories)
            foreach (var tag in directory.Tags)
                if(!(Array.Exists(ignoredAttributes, element => element == tag.Name))){
                    if(tag.Name == "Detected MIME Type"){
                        Console.WriteLine("MIME Type:"+tag.Description);
                    }
                    else{
                         Console.WriteLine($"{tag.Name}:{tag.Description}");
                    }
                   
                }
                

                
                
                
            //need to figure out a way to get the file owner
            string fileMetaData = "FileName:"+FileName+",FileType:"+FileType+",Size:"+size+",Directory:"+FileDir+",Created:"+CreationTime+",Accessed:"+AccessTime+",Updated:"+UpdatedTime;
            
            // now we have the basic generic file metadata, we will add extra metadata for each file depending on it's type
            return fileMetaData;
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
                        
                        Console.WriteLine("Scanning the directory: "+args[1]+" for files recursively...");
                        string[] files = FindAllFiles(args[1]);
                        foreach (string file in files){
                            //Console.WriteLine(file);
                            string fileData = GetMetadata(file);
                            Console.WriteLine(fileData);
                        }

                    }
                    else if (UserInputChoice == "n"){
                        
                        Console.WriteLine("Scanning the directory: "+args[1]+" for file non-recursively...");
                        string[] files = System.IO.Directory.GetFiles(args[1],"*.*");
                        foreach (string file in files){
                            //Console.WriteLine(file);
                            string fileData = GetMetadata(file);
                            Console.WriteLine(fileData);
                        }
                    }
                    
                }
                else{
                    Console.WriteLine("ERROR: Please provide a valid directory");
                    
                }
            
            
            
            }
        }
    }
}
