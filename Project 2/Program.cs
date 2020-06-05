using System;
using System.Threading;

namespace Project2
{
    class filozofy
    {
        private Semaphore[] widelce;
        private Semaphore lokaj;

        int ilość = 0;

        public Thread[] filozofie;


        public filozofy()
        {
            widelce = new Semaphore[5];


            for (int a = 0; a < 5; a++)
            {
                widelce[a] = new Semaphore(1, 1);
            }
            lokaj = new Semaphore(4, 4);


            filozofie = new Thread[5];

            for (int c = 0; c < 5; c++)
            {
                filozofie[c] = new Thread(new ThreadStart(Jedzenie));
                filozofie[c].Name = "Filozof " + c;
            }

        }


        public void Jedzenie()
        {
            int numer = Int32.Parse(Thread.CurrentThread.Name.Split(' ')[1]);
            while (ilość < 5)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} myśli");
                Thread.Sleep(100);
                lokaj.WaitOne();//sprawdzamy, czy możemy usiąść do stołu
                widelce[numer].WaitOne(); //bierzemy  widelec
                Console.WriteLine($"{Thread.CurrentThread.Name} bierze widelec{numer}");

                widelce[(numer + 1) % 5].WaitOne();//bierzemy  widelec
                Console.WriteLine($"{Thread.CurrentThread.Name} bierze widelec{(numer + 1) % 5}");

                Console.WriteLine($"{Thread.CurrentThread.Name} je");

                Console.WriteLine($"{Thread.CurrentThread.Name} skońszył jedzenie i myślenie");
                widelce[numer].Release();//odkładamy  widelec
                widelce[(numer + 1) % 5].Release();//odkładamy  widelec

                lokaj.Release(); //dopuszczamy kolejnego filozofa do stołu    
                ilość++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            filozofy pw = new filozofy();
            foreach (Thread threadd in pw.filozofie)
            {
                threadd.Start();
            }
            Thread.Sleep(10000);
            foreach (Thread threadd in pw.filozofie)
            {
                threadd.Interrupt();
            }
            Console.ReadKey();
        }
    }
}
