using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace WinFormsApp1_lesson_6Babylon_2026
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    internal sealed class Logger: IDisposable
    {

        
        public class LoggerSettingsModel
        {
            public bool AppendToFile { get; set; } = true;
            public int QueueNumber { get; set; } = 3;
            public string LogFileName { get; set; } = "fileLog.txt";
            public int MaxLinesCount { get; set; } = 10;     
            public int MaxFilesCount { get; set; } = 3;    
            public bool IsCarousel { get; set; } = true;   
        
        }
        private readonly string settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "loggerSettings.json");
        private LoggerSettingsModel settings = new ();
        private readonly BlockingCollection<string> logQueue = new();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task backgroundWriterTask;
        private int recordsNumber = 0;

        private  object lockObject = new object();
        public static Logger Instance { get; } = new();
        

        private Logger()
        {
            LoadSettings();
            RotateFilesOnStart();
            backgroundWriterTask = Task.Run(() => ProcessLogQueue(cancellationTokenSource.Token));
        }
        static Logger()
        {
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsFilePath))
                {
                    string fileJson = File.ReadAllText(settingsFilePath);
                    var loaded = JsonSerializer.Deserialize<LoggerSettingsModel>(fileJson);
                    if (loaded != null) settings = loaded;
                }
                else
                {
                    string fileJson = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(settingsFilePath, fileJson);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
        }
        
        private string GetCarouselFilePath(int index)
        {
            if (index == 1) return Path.Combine(Directory.GetCurrentDirectory(), settings.LogFileName);

            string ext = Path.GetExtension(settings.LogFileName);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(settings.LogFileName);
            return Path.Combine(Directory.GetCurrentDirectory(), $"{nameWithoutExt}{index}{ext}");
        }

        
        private void CheckAndRotateFiles(string currentFilePath, int incomingLinesCount)
        {
            if (!settings.IsCarousel) return;

            if (File.Exists(currentFilePath))
            {
                int currentLines = File.ReadAllLines(currentFilePath).Length;
                if (currentLines + incomingLinesCount > settings.MaxLinesCount)
                {
                    
                    string lastFile = GetCarouselFilePath(settings.MaxFilesCount);
                    if (File.Exists(lastFile))
                    {
                        File.Delete(lastFile);
                    }

                   
                    for (int i = settings.MaxFilesCount - 1; i >= 1; i--)
                    {
                        string oldFile = GetCarouselFilePath(i);
                        string newFile = GetCarouselFilePath(i + 1);

                        if (File.Exists(oldFile))
                        {
                            if (File.Exists(newFile)) File.Delete(newFile);
                            File.Move(oldFile, newFile);
                        }
                    }
                }
            }
        }

        
        private void RotateFilesOnStart()
        {
            if (!settings.IsCarousel) return;
            string primaryFile = GetCarouselFilePath(1);
            if (File.Exists(primaryFile))
            {
                int lines = File.ReadAllLines(primaryFile).Length;
                if (lines >= settings.MaxLinesCount)
                {
                    CheckAndRotateFiles(primaryFile, 0);
                }
            }
        }
        private void ProcessLogQueue(CancellationToken cancellationToken)
        {
            string fullPath = GetCarouselFilePath(1);
            List<string> batchBuffer = new List<string>();

            
            lock (lockObject)
            {
                if (!settings.AppendToFile && File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                    if (logQueue.TryTake(out string? logItem, Timeout.Infinite, cancellationToken))
                    {
                        batchBuffer.Add(logItem);                    
                        if (batchBuffer.Count >= settings.QueueNumber)
                        {
                            WriteBatchToFile(fullPath, batchBuffer);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            { 
                Debug.WriteLine("Background writer task was canceled.");
            }
            
            if (batchBuffer.Count > 0)
            {
                WriteBatchToFile(fullPath, batchBuffer);
            }
        }

        private void WriteBatchToFile(string filePath, List<string> buffer)
        {
            lock (lockObject) 
            {
                try
                {
                    CheckAndRotateFiles(filePath, buffer.Count);
                    filePath=GetCarouselFilePath(1);
                    File.AppendAllLines(filePath, buffer);
                    buffer.Clear();
                    Debug.WriteLine("log saved!!!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"error saving log: {ex.Message}");
                }
            }
        }

               
        public void LogData(LogLevel logLevel, string message, [System.Runtime.CompilerServices.CallerMemberName]string methodName="")
        {
            
            string dateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.");
            int threadId=Thread.CurrentThread.ManagedThreadId;
            int processId=Process.GetCurrentProcess().Id;
            var frame= new System.Diagnostics.StackTrace().GetFrame(1);
            string functionName=frame?.GetMethod()?.DeclaringType?.Name ?? string.Empty;
            int currentRecordNumber = System.Threading.Interlocked.Increment(ref recordsNumber);

            string data = $"{recordsNumber}. [{dateTime}]:[{threadId}]:[{processId}]: [{logLevel}] | {functionName}:{methodName} - {message}";

            Debug.WriteLine(data);

            
            logQueue.Add(data);

            recordsNumber++;

        }

        public void Dispose()
        {
            logQueue.CompleteAdding();
            try
            {
                backgroundWriterTask.Wait(2000);
            }
            catch { }
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            logQueue.Dispose();
        }
    }
}


//private readonly string logPath = Directory.GetCurrentDirectory();
//private string GetFilePath(int index)
//{
//    return Path.Combine(Directory.GetCurrentDirectory(), $"file{index}.txt");
//}
//public int MaxFilesCount = 5;
//public bool IsCarusel { get; set; } = true;
//private readonly string fileRotate= Path.Combine(Directory.GetCurrentDirectory(), "fileRotate.json");

//private class LoggerSettings
//{
//    public int BufferSize { get; set; } = 10;
//    public bool IsCarusel { get; set; }= true;
//    public int CaruselNumber { get; set; } = 3;
//}

//public void FileSaveRotate(int maxFilesCount, bool isCarusel, bool newFile = true)
//{
//    if(IsCarusel)
//    {
//        MaxFilesCount = maxFilesCount;
//        IsCarusel = isCarusel;
//    }
//    else
//    {
//        MaxFilesCount = 1;
//        IsCarusel = isCarusel;

//    }
//    if (newFile)
//    {
//        DeleteAllLogFiles();
//    }
//}

//private int recordsNumber = 0;







//private void DeleteAllLogFiles()
//{
//    string[] files= Directory.GetFiles(Directory.GetCurrentDirectory(),"file*.txt");
//    foreach (var item in files)
//    {
//        File.Delete(item);
//    }

//}
//private void RotateFileOnStart()
//{
//    if (!IsCarusel) return;
//    string activFile = GetFilePath(1);
//    string MaxFile = GetFilePath(MaxFilesCount+1);
//    if (File.Exists(MaxFile))
//    {
//        File.Delete(MaxFile);
//    }
//    if (File.Exists(activFile))
//    {
//        var lineCount=File.ReadAllLines(activFile).Length;
//        if(lineCount>=10)
//        {

//            for(int i=MaxFilesCount-1;i>=1;i--)
//            {
//                string currentFile = GetFilePath(i);
//                string nextFile = GetFilePath(i + 1);
//                if(File.Exists(currentFile))
//                {
//                    if (File.Exists(nextFile))
//                    {
//                        File.Delete(nextFile);
//                    }
//                    File.Move(currentFile, nextFile);
//                }

//            }
//        }
//    }
//}
// Formats:
// [DateTime] : [Thread ID] : [Process Id] : [Log levels] | [Functionality] : [Method] - "Some error"
// 23.06.2025 19:14:53 : [UI] : [Debug]     | Music playback : StartPlayback(...) - Playback was started with no errors. 
// 23.06.2025 19:14:58 : [UI] : [Warning]   | Music playback : GetNetxBufferPart(...) - Cannot load all the bytes data in time. Method completed with timout. 

// 1. Playback was started with no errors. 
// 2. Playback was started with no errors.