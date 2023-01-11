using System.Threading;
using Magazine.EF;
using Magazine.Models;
using Microsoft.EntityFrameworkCore;

namespace Magazine;

public class MultithreadingToDatabase
{
    private bool acuiredlock = false;
        private AutoResetEvent wait_handler = new AutoResetEvent(true);
        private Mutex mutex = new Mutex();
        private Semaphore semaphore = new Semaphore(0,1);
        private DbContextOptions options;

        

        public void Lock()
        {
            object? locker = new object();
            for (int i = 0; i < 11; i++)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    Thread myThread = new Thread(() =>
                    {
                        lock (locker!)
                        {
                            context.Persons.Add(new Person() 
                                { LastName = "some_person" + i });
                            context.SaveChanges();
                        }
                    });
                    myThread.Start();
                }
            }
        }

        public void Mutex()
        {
            for (int i = 0; i < 11; i++)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    Thread mythread = new Thread(() =>
                    {
                        mutex.WaitOne();
                        context.Persons.Add(new Person() { LastName = "some_awesome_person" + i });
                        context.SaveChanges();
                        mutex.ReleaseMutex();
                    });
                    mythread.Start();
                }
            }
        }

        public void Semaphore()
        {
            for (int i = 0; i < 11; i++)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    Thread myThread = new(() =>
                    {
                        semaphore.WaitOne();
                        context.Articles.Add(new Article() { Name = "some_article" + i });
                        context.SaveChanges();
                        semaphore.Release();
                    });
                    myThread.Start();
                }
            }
        }

        public void monitor()
        {
            object? locker = new object();
            for (int i = 0; i < 11; i++)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    Thread myThread = new Thread(() =>
                    {
                        bool acquiredlock = false;
                        try
                        {
                            Monitor.Enter(locker, ref acquiredlock);
                            context.Articles.Add(new Article() { Name = "some_article" + i });
                            context.SaveChanges();
                        }
                        finally
                        {
                            if (acquiredlock)
                            {
                                Monitor.Exit(locker);
                            }
                        }                        
                    });
                    myThread.Start();
                }
            }
        }

        public void AutoResetEvent()
        {
            for (int i = 0; i < 11; i++)
            {
                using (AppDbContext context = new AppDbContext())
                {
                    Thread myThread = new(() =>
                    {
                        wait_handler.WaitOne();
                        context.Articles.Add(new Article() { Name = "some_author" + i });
                        context.SaveChanges();
                        wait_handler.Set();
                    });
                    myThread.Start();
                }
            }
        }
}