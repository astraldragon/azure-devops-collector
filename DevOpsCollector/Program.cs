using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace DevOpsCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var sdk = new AzureSDK("abc123");
            sdk.StreamProjects("abc123").Subscribe(x => {
                Console.WriteLine(x.ToString());
            });
        }
    }
}
