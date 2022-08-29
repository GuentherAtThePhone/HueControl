// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Diagnostics;

string updatePath, processID;

processID = args[0].ToString();
string UpdatePath = @"C:\\Users\\Coolj\\source\\repos\\HueControl\\HueControl\\bin\\Debug\\net6.0-windows";

Console.WriteLine(processID);
Console.WriteLine(UpdatePath);

Process.GetProcessById(Convert.ToInt32(processID)).Kill();

try
{
    // Get newest Version
    string remoteUri = "https://raw.githubusercontent.com/GuentherAtThePhone/HueControl/master/";
    string fileName = "README.md", myStringWebResource = null;
    // Create a new WebClient instance.
    WebClient myWebClient = new WebClient();
    // Concatenate the domain with the Web resource filename.
    myStringWebResource = remoteUri + fileName;
    // Download the Web resource and save it into the current filesystem folder.
    myWebClient.DownloadFile(myStringWebResource, fileName);

    string[] file = File.ReadAllLines(fileName);
    File.Delete(fileName);
    // Current Version: 1.0.0
    int i = file[3].IndexOf(":") + 2;
    string version = file[3].Substring(i);
    Console.WriteLine(version);
}
catch (Exception)
{

    throw;
}


try
{
    // Download The new File
    string remoteUriUpate = "https://github.com/GuentherAtThePhone/HueControl/releases/download/v1.0.0/";
    string fileNameUpate = "HueControl.exe", myStringWebResourceUpdate = null;
    // Create a new WebClient instance.
    WebClient myWebClientUpdate = new WebClient();
    // Concatenate the domain with the Web resource filename.
    myStringWebResourceUpdate = remoteUriUpate + fileNameUpate;
    // Download the Web resource and save it into the current filesystem folder.
    myWebClientUpdate.DownloadFile(myStringWebResourceUpdate, fileNameUpate);

}
catch (Exception ex)
{
	Console.WriteLine(ex.Message.ToString());
    if(ex.InnerException != null)
    Console.WriteLine(ex.InnerException.ToString());
}

try
{
    Process.Start("HueControl.exe");
}
catch (Exception)
{

    throw;
}


Console.ReadKey();
Console.ReadLine();