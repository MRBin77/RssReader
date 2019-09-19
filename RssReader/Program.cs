using Newtonsoft.Json;
using NLog;
using System;
using System.IO;


namespace RssReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger(); // Логирование в файл
            try
            {
                {
                    var settingsRss = JsonConvert.DeserializeObject<RssLinks>(File.ReadAllText("Settings.json"));
                    string[] rsslinks = settingsRss.Uri;
                    string path = Environment.CurrentDirectory;
                    RssFeed rssFeed = new RssFeed();

                    while (true)
                    {
                        Console.WriteLine("Enter command: ");
                        string cmdText = Console.ReadLine();
                        Console.WriteLine("\n");
                        switch (cmdText)
                        {
                            case "pull":
                                rssFeed.Pull(path, rsslinks);
                                break;
                            case "list":
                                rssFeed.List(path);
                                break;
                            case "remove":
                                rssFeed.Remove(path);
                                break;
                            case "backup":
                                RssBackup.BackupRss(path);
                                break;
                            case "add":
                                rssFeed.AddRss();
                                break;
                            case "exit":
                                rssFeed.Exit();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }
    }
}
