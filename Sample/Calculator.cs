using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class Caluculator
    {
        private int i;
        private int j;

        public Caluculator()
        { 
        }

        public Caluculator(int firstValue, int secondValue,int b)
        { 
        }
        
        public Caluculator(int firstValue,int secondValue)
        {
            if (firstValue > 10)
                throw new Exception("Value can't be greater than 10");
            i = firstValue;
            j = secondValue;
        }

        private int Add(int firstValue, int secondValue)
        {
            return firstValue + secondValue;
        }

        public int IADD()
        {
            return Add(i, j);
        }

        public int Multiply()
        {
            return i*j;
        }

        public int Divide()
        {
            return i / j;
        }
    }
}
