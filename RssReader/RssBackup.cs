using NLog;
using System;
using System.IO;
using System.IO.Compression;

namespace RssReader
{
    public static class RssBackup
    {
        /// <summary>
        /// Возможность создавать бэкап скаченных RSS лент в виде zip архива
        /// </summary>
        /// <param name="path"></param>
        public static void BackupRss(string path)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            try
            {
                var dirInfo = new DirectoryInfo(path + @"\RssFeed");


                if (dirInfo.GetFiles().Length > 0)
                {
                    string currentDate = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                    string zipPath = @".\Backup_RssFeed_" + currentDate + ".zip";
                    ZipFile.CreateFromDirectory(dirInfo.ToString(), zipPath);
                }
                else
                {
                    long fileByteSize = dirInfo.GetFiles().Length;
                    Console.WriteLine("Каталог пуст" + "\n" + "Размер: " + fileByteSize.ToString() + " байт");
                }
                logger.Info("method BackupRss is execute successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }
    }
}
