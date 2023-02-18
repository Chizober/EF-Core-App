using EF_Core_DAL;
using EF_Core_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace EF_Core_App
{
    class Program
    {
        static async Task Main()
        {
            ToDoAppDbContextFactory toDoAppDbContextFactory = new ToDoAppDbContextFactory();
            var dbContext = toDoAppDbContextFactory.CreateDbContext(null);
            begin:
            Console.WriteLine(
                "\n\n\t...................EFCore ConsoleApplication...................\n\n"
                    + "\t\t\tWHAT WOULD YOU LIKE TO DO?\n\n"
                    + "\t\t 1. Create Users\n"
                    + "\t\t 2. Fetch Users\n"
                    + "\t\t 3. Update Users\n"
                    + "\t\t 4. Delete User \n"
                    + "\t\t 5. Search User \n"
                    + "\t\t 6. Create new user with Tasks and Tags \n"
                    + "\t\t 7. Fetch User, the tasks and tags \n"
                    + "\t\t 8. Press 8 to Exit\n"
            );
            Console.Write("Pick One Option:");
            string Input = Console.ReadLine();
            if (Input == "1")
                goto CreateUsers;
            if (Input == "2")
                goto FetchUsers;
            if (Input == "3")
                goto UpdateUser;
            if (Input == "4")
                goto DeleteUser;
            if (Input == "5")
                goto SearchUser;
            if (Input == "6")
                goto CreateUserTasksandTags;
            if (Input == "7")
                goto FetchUserTaskTags;
            if (Input == "8")
                System.Environment.Exit(1);
            Console.WriteLine("Invalid option!");
            goto begin;

            CreateUsers:
            {
                bool anyUser = await dbContext.Users.AnyAsync();
                // To create new users, remove ! in if (!anyUser) then add new users, else it will not get added to the database
                if (!anyUser)
                {
                    List<User> users = new List<User>()
                    {
                        new User
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            PhoneNumber = "0905463721",
                            Email = "john.Doe@gmail.com",
                            Birthday = new DateTime(2001, 1, 1),
                            Created = DateTime.Now,
                            IsActive = true
                        },
                        new User
                        {
                            FirstName = "Mary",
                            LastName = "Franklin",
                            PhoneNumber = "0905468880",
                            Email = "mary.franklin@gmail.com",
                            Birthday = new DateTime(2012, 1, 10),
                            Created = DateTime.Now.AddYears(-4),
                            Updated = DateTime.Now
                        },
                        new User
                        {
                            FirstName = "Grace",
                            LastName = "Okonkwo",
                            MiddleName = "Ugonma",
                            PhoneNumber = "0805467588",
                            Email = "grace.okonkwo@gmail.com",
                            Birthday = new DateTime(2010, 10, 12),
                            Created = DateTime.Now.AddYears(-2),
                            Updated = DateTime.Now,
                            IsActive = true
                        },
                        new User
                        {
                            FirstName = "Deborah",
                            LastName = "Peters",
                            MiddleName = "Anne",
                            PhoneNumber = "0811467402",
                            Email = "debby.peters@gmail.com",
                            Birthday = new DateTime(2020, 05, 12),
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                            IsActive = true
                        },
                        new User
                        {
                            FirstName = "James",
                            LastName = "Okoro",
                            MiddleName = "Matthew",
                            PhoneNumber = "08231467507",
                            Email = "matthew.okoro@gmail.com",
                            Birthday = new DateTime(2012, 10, 12),
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                        }
                    };
                    //AddRange

                    Console.WriteLine(".........Create Users.......");
                    await dbContext.Users.AddRangeAsync(users);
                    Console.WriteLine(await dbContext.SaveChangesAsync());
                }
                goto begin;
            }
            FetchUsers:
            {
                Console.WriteLine("..........Fetched All Users.........");
                var allUsers = await dbContext.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    Console.WriteLine($"{user.FirstName}  {user.LastName}  {user.Id}\n");
                }

                Console.WriteLine("..........Fetched All Active Users.........\n");
                var activeUsers = await dbContext.Users
                    .Where(u => u.IsActive && u.Updated == null)
                    .ToListAsync();
                foreach (var user in activeUsers)
                {
                    Console.WriteLine($"{user.FirstName}  {user.LastName}  {user.Id}\n");
                }
                goto begin;
            }
            SearchUser:
            {
                Console.WriteLine(".......Scalars........\n");
                Console.WriteLine($"Max Age:{await dbContext.Users.MaxAsync(u => u.Birthday)}\n");
                var youngest = await dbContext.Users
                    .OrderByDescending(u => u.Birthday)
                    .FirstOrDefaultAsync();
                Console.WriteLine($"Youngest:{youngest?.FirstName}  {youngest?.LastName}\n");

                var oldest = await dbContext.Users.OrderBy(u => u.Birthday).FirstOrDefaultAsync();
                Console.WriteLine(
                    $"Oldest:{oldest?.FirstName ?? "not found"}  {oldest?.LastName ?? "not found\n"}"
                );
                //deferred
                IQueryable<User> userQuery = dbContext.Users.OrderByDescending(u => u.Birthday);
                await dbContext.SaveChangesAsync();
                goto begin;
            }
            UpdateUser:
            {
                Console.WriteLine(".........Update............\n");
                //change the firstName to make a new update
                var userToUpdate = await dbContext.Users.SingleOrDefaultAsync(
                    u => u.FirstName.ToLower() == "deborah"
                );
                // you can update using Id too,var userToUpdate = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == 3);
                if (userToUpdate != null)
                {
                    userToUpdate.FirstName = "Chizober";
                    var msg =
                        await dbContext.SaveChangesAsync() > 0
                            ? "Updated Successfully"
                            : "Update Failed";
                    Console.WriteLine(msg);
                }
                goto begin;
            }
            DeleteUser:
            {
                Console.WriteLine(".........Delete.........\n");

                // you can delete using firstName too var userToDelete = await dbContext.Users.SingleOrDefaultAsync(u => u.FirstName.ToLower() == "grace");
                var userToDelete = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == 3);
                //change the id to make a new delete
                if (userToDelete != null)
                {
                    dbContext.Users.Remove(userToDelete);
                    var msg =
                        await dbContext.SaveChangesAsync() > 0
                            ? "Deleted Successfully"
                            : "Delete Failed";
                    Console.WriteLine(msg);
                }
                goto begin;
            }
            CreateUserTasksandTags:
            {
                var demo = new User
                {
                    FirstName = "Ifunanya",
                    LastName = "Gabriel",
                    PhoneNumber = "0801997508",
                    Email = "ify.gabriel@gmail.com",
                    Birthday = new DateTime(1993, 10, 25),
                    Created = DateTime.Now,
                    IsActive = true,
                    Tasks = new List<EF_Core_DAL.Entities.Task>()
                    {
                        new EF_Core_DAL.Entities.Task()
                        {
                            Name = "Touring",
                            Description =
                                "Touring has to do with seeing new places like cities, countries and having fun while at it",
                            Tags = new List<Tag>() { new Tag() { Name = "Fun" }, }
                        },
                        new EF_Core_DAL.Entities.Task
                        {
                            Name = "Listening to Music",
                            Tags = new List<Tag>()
                            {
                                new Tag()
                                {
                                    Name = "Stress Release",
                                    Description =
                                        "Releasing stress after close of work by listening to music,while resting"
                                }
                            }
                        }
                    }
                };
                dbContext.Add(demo);
                Console.WriteLine(await dbContext.SaveChangesAsync());
                goto begin;
            }
            FetchUserTaskTags:

            Console.WriteLine(
                "\n\n\t...................Pick one Fetch Option...................\n\n"
                    + "\t\tType A to Fetch using Lazy Loading.\n"
                    + "\t\tType B to to Fetch using Eager Loading.\n"
            );
            Input = "";

            while (Input != "A" && Input != "B")
            {
                Console.WriteLine("\nPlease select A or B");
                Input = Console.ReadLine();
                Input = Input.ToUpper();
            }
            if (Input == "A")
                goto A;
            if (Input == "B")
                goto B;
            A:
            {
                var chizoberTasksTags = await dbContext.Users
                    .OrderBy(u => u.Id)
                    .LastOrDefaultAsync();
                //this fetches only the columns specified,because it does not use the Include and ThenInclude keywords
                Console.WriteLine(
                    $"First and LastName of the user:{chizoberTasksTags?.FirstName} {chizoberTasksTags?.LastName}\n"
                );
                goto begin;
            }
            B:
            {
                var chizoberTasksTags = await dbContext.Users
                    .OrderBy(u => u.Id)
                    .Include(i => i.Tasks)
                    .ThenInclude(i => i.Tags)
                    .LastOrDefaultAsync();
                Console.WriteLine(
                    $" First and LastName of the user:{chizoberTasksTags?.FirstName} {chizoberTasksTags?.LastName}\n"
                );
                if (chizoberTasksTags?.Tasks.Any() == true)
                {
                    Console.WriteLine(".........Tasks...........\n");
                    foreach (var task in chizoberTasksTags?.Tasks)
                    {
                        Console.WriteLine(
                            $"Task Name:{task.Name}\nIsCompleted:{task.IsCompleted}\nCreated:{task.Created}\n"
                        );
                        if (task.Tags.Any())
                        {
                            Console.WriteLine(".........Tags........\n");
                            foreach (var tag in task.Tags)
                            {
                                Console.WriteLine(
                                    $"Tag Name:{tag.Name}\nCreated:{tag.Created}\n"
                                );
                            }
                        }
                    }
                }
                goto begin;
            }
        }
    }
}
