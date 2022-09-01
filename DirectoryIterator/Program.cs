// See https://aka.ms/new-console-template for more information
using CustomDirectoryIterator.Contracts;

Console.WriteLine("Hello, World!");

//var foo = CustomDirectoryIterator.CustomDirectory.EnumerateDirectories(@"f:\", out IInaccessiblePathCollection inaccessiblePaths);
var directories = CustomDirectoryIterator.CustomDirectory.EnumerateDirectories(@"f:\");

Console.WriteLine(directories.Count());

foreach (var directory in directories)
{
    Console.WriteLine(directory);
}

//foreach (var item in inaccessiblePaths)
//{
//    Console.ForegroundColor = ConsoleColor.Red;
//    Console.WriteLine(item.Exception.Message);
//    Console.ForegroundColor = ConsoleColor.White;
//}