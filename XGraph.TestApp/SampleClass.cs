using System;

namespace XGraph.TestApp
{
    class SampleClass
    {
        public int OutputPortAsInt { get { return 0;  } }

        public int BiPortAsInt { get; set; }

        public int InputPortAsInt { set { } }

        public event Action<int> TimeChanged;
    }
}
