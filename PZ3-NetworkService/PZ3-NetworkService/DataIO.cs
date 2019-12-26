using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PZ3_NetworkService
{
    public class DataIO
    {
        public void SerializeObject<T>(T serializableObject, string fileName)			                //templejt prihvata bilo koju klasu,ime fajla
        {
            if (serializableObject == null) { return; }						                            //da li je objekat koji je poslat null

            try
            {
                XmlDocument xmlDocument = new XmlDocument();					                        //klasa za serijalizaciju				
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());	            //klasa(trazi kojeg je tipa klasa)
                using (MemoryStream stream = new MemoryStream())				                        //streamovanje u memoriji
                {
                    serializer.Serialize(stream, serializableObject);				                    //koji stream koristimo i koji objekat serializujemo
                    stream.Position = 0;
                    xmlDocument.Load(stream);							                                //load i sacuvaj pod tim filenameom
                    xmlDocument.Save(fileName);
                    stream.Close();								                                        //zatvori stream
                }
            }
            catch /*(Exception ex)*/
            {
                //Log exception here
            }
        }

        public T DeSerializeObject<T>(string fileName)						                            //opet rdimo sa generickom klasom(prima file name)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }				                    //da li je stream null ili empty

            T objectOut = default(T);								                                    //default je ugradjeni konstruktor

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))				                    //treba name reader za citanje
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);				                    //kastovanje
                        reader.Close();
                    }

                    read.Close();
                }
            }

            catch /*(Exception ex)*/
            {
                //Log exception here								                                    //ispis da se desila greska
            }

            return objectOut;
        }
    }
}
