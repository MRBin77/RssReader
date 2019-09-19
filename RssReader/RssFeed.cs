using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace RssReader
{
    public class RssFeed
    {
        Logger logger = LogManager.GetCurrentClassLogger(); // Логирование в файл

        /// <summary>
        /// Возможность добавлять новые RSS ленты
        /// </summary>
        public void AddRss()
        {
            try
            {
                Console.WriteLine("Enter link");
                string[] links = Console.ReadLine().Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var jDerialize = JsonConvert.DeserializeObject<RssLinks>(File.ReadAllText("Settings.json"));
                jDerialize.Uri = jDerialize.Uri.Concat(links).ToArray();

                JObject obj = new JObject(new JProperty("link", jDerialize.Uri));
                var jSerialize = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

                string path = Environment.CurrentDirectory + @".\Settings.json";
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(jSerialize.ToString());
                    sw.Close();
                }
                logger.Info("method add() is execute successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }
        /// <summary>
        /// Выйти из программы.
        /// </summary>
        public void Exit()
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// Читает RSS ленты из файла настроек, скачивает их и сохраняет на локальный диск.
        /// </summary>
        /// <param name="path"> Путь для сохранения на диск</param>
        /// <param name="rsslinks"> ссылки из файла настроек</param>
        public void Pull(string path, string[] rsslinks)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                XmlDocument doc = new XmlDocument();
                foreach (string link in rsslinks)
                {
                    string writePath = dirInfo.CreateSubdirectory(@"RssFeed\") + link.Substring(8, 12).Replace('/', '_') + ".xml";
                    doc.Load(link);
                    doc.Save(writePath);
                }
                logger.Info("method pull() is execute successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }

        /// <summary>
        /// Удаляет все скаченные RSS ленты.
        /// </summary>
        /// <param name="path">Путь для удаления</param>
        public void Remove(string path)
        {
            try
            {
                Directory.Delete(path + @"\RssFeed", true);
                logger.Info("method remove() is execute successfully");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }

        /// <summary>
        /// Читает скаченные локально RSS ленты и отображает их элементы (item).
        /// Для каждого элемента отображает название(поле item->title), дату публикации(поле item->pubDate) и ссылку(поле item->link).
        /// </summary>
        /// <param name="path">Путь для чтения файлов</param>
        public void List(string path)
        {
            try
            {
                string[] dir = Directory.GetFiles(path + @"\RssFeed", "*.xml");
                foreach (string file in dir)
                {
                    XmlReader xmlreader = XmlReader.Create(file);

                    while (xmlreader.Read())
                    {
                        if (xmlreader.NodeType == XmlNodeType.Element)
                        {
                            string[] xmlNode = { "item" };
                            string[] selectNode = { "title", "pubDate", "link" };

                            foreach (var node in xmlNode)
                            {
                                if (xmlreader.Name == node)
                                {
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(xmlreader.ReadOuterXml());
                                    XmlNode n = xmlDoc.SelectSingleNode(node);

                                    foreach (string item in selectNode)
                                    {
                                        string items = n.SelectSingleNode(item).InnerText;
                                        Console.WriteLine(items);
                                    }
                                    Console.WriteLine("\n");
                                }
                            }

                        }

                    }
                    xmlreader.Close();
                }
                logger.Info("method list() is execute successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
        }
    }
}
