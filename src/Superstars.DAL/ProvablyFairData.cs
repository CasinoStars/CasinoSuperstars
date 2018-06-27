﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Superstars.DAL
{
    public class ProvablyFairData
    {
        public string UncryptedPreviousServerSeed { get; set; }

        public string UncryptedServerSeed { get; set; }

        public string CryptedServerSeed { get; set; }

        public string ClientSeed { get; set; }

        public int Nonce { get; set; }
    }
}
