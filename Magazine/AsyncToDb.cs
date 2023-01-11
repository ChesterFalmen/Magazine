using System;
using System.Threading.Tasks;
using Magazine.EF;
using Magazine.Models;
using Microsoft.EntityFrameworkCore;

namespace Magazine;

public class AsyncToDb
{
    private DbContextOptions options;
    public AsyncToDb(DbContextOptions options)
    {
        this.options = options;
    }

    public async Task AsyncAdd()
    {
        using (AppDbContext context = new AppDbContext())
        {
            for (int i = 0; i < 10; i++)
            {
                await context.Persons.AddAsync(new Person() { FirstName = "some_person" + i});
                await context.SaveChangesAsync();
            }
        }
    }

    public async Task AsyncRead()
    {
        using (AppDbContext context = new AppDbContext())
        {
            var list = await context.Persons.ToListAsync();
            foreach (var person in list)
            {
                Console.WriteLine(person.FirstName + " " + person.LastName);                
            }               
        }
    }
}