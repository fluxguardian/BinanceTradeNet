using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Structure
{
    public class BookTicker
    {
        public string Stream { get; set; }

        public Data Data { get; set; }
    }
}
