using System;


namespace MODEL
{
    public class User
    {
        string id;
        string rssireceive;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string RSSI
        {
            get { return rssireceive; }
            set { rssireceive = value; }
        }

    }
}
