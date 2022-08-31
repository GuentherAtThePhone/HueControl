// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Diagnostics;
using System;


string processID;

processID = args[0].ToString();
string UpdatePath = @"C:\\Users\\Coolj\\source\\repos\\HueControl\\HueControl\\bin\\Debug\\net6.0-windows";

// Create the log file
File.Create(@"UpdateLog.log").Close();
File.AppendAllText(@"UpdateLog.log", "Logfile created at " + DateTime.Now.ToLongDateString() + "\r\n");

Console.WriteLine(processID);
Console.WriteLine(UpdatePath);
File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": given processID: " + processID + "\r\n");

Process.GetProcessById(Convert.ToInt32(processID)).Kill();

File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": Killed the process with the ID: " + processID + "\r\n");

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

    int i = file[3].IndexOf(":") + 2;
    string version = file[3].Substring(i);

    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": successfully requested the newest version: " + version + "\r\n");
}
catch (Exception ex)
{
    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": couldn´t find the newest version due to the error: " + ex.Message.ToString() + "\r\n");
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

    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": Successfully downloaded the newest verion" + "\r\n");
}
catch (Exception ex)
{
    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": couldn´t download the newest version due to the error: " + ex.Message.ToString() + "\r\n");
}

try
{
    Process.Start("HueControl.exe");
    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": Successfully started the progress HueControl.exe" + "\r\n");
}
catch (Exception ex)
{
    File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": couldn´t start the HueControl.exe progress due to the error: " + ex.Message.ToString() + "\r\n");
}

File.AppendAllText(@"UpdateLog.log", DateTime.Now.ToLongDateString() + ": LogFile closed.");