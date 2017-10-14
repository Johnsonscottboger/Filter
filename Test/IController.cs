using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public interface IController
    {
        void HelloWorld();

        void HelloWorld(string name);

        void HelloWorld(string name, DateTime dateTime);

        int Add(int a, int b);

        int Add(int a, int b, int c);

        int Add(int a, int b, int c, int d);

        int Add(int a, int b, int c, int d, int e);

        int AddException();

        void ThrowException();
    }
}
