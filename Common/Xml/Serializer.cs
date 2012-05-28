using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SoftGPL.Common.Xml
{
    public class Serializer<T>
    {
        private System.Xml.Serialization.XmlSerializer mSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));


        public Serializer()
        {
        }


        public T Deserialize(string buffer)
        {
            System.IO.TextReader textreader = new System.IO.StringReader(buffer);
            T obj = Deserialize(textreader);
            textreader.Close();
            return obj;
        }

        public T Deserialize(System.IO.TextReader reader)
        {
            //string buffer = reader.ReadToEnd();
            T obj = (T)mSerializer.Deserialize(reader);

            /*
             * Test the object that we we just created.
             * TextWriter writer = new StringWriter();
             * m_Serializer.Serialize(writer, obj);
             */

            return obj;
        }

        public string Serialize(T obj)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(memoryStream, Encoding.UTF8);
            mSerializer.Serialize(xmlWriter, obj);

            UTF8Encoding encoder = new UTF8Encoding();
            string response = encoder.GetString(memoryStream.ToArray());
            memoryStream.Close();
            xmlWriter.Close();
            return response.Trim();
        }

    }
}
