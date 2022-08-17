using CleaningManagement.DAL.Models;

namespace CleaningManagement.DAL.Infrastructure
{
    /// <summary>
    /// Seeder class.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds in-memory database.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Seed(CleaningManagementDbContext context)
        {
            var hotel = new Customer()
            {
                Name = "Hotel Park",
            };

            context.Customers.Add(hotel);

            var caffe = new Customer()
            {
                Name = "Caffe Romano"
            };

            context.Customers.Add(caffe);

            context.SaveChanges();

            for (int i = 0; i < 5; i++)
            {
                var hotelCleaninpPlan = new CleaningPlan()
                {
                    Description = i % 2 == 0 ? "YAT cleaning plan." : i == 3 ? "Germophobe plan." : null,
                    Title = $"{hotel.Name} {i + 1}",
                    CustomerId = hotel.Id,
                    CreatedAt = System.DateTime.UtcNow
                };

                context.CleaningPlans.Add(hotelCleaninpPlan);
            }

            var caffeCleaningPlan = new CleaningPlan()
            {
                Description = "Morning cleaning.",
                Title = $"{caffe.Name} morning cleaning",
                CustomerId = caffe.Id,
                CreatedAt = System.DateTime.UtcNow
            };

            context.CleaningPlans.Add(caffeCleaningPlan);

            context.SaveChanges();

            var activeUser = new User()
            {
                Email = "some.email@mail.com",
                IsActive = true,
                PasswordHash = Repositories.UserRepository.GetPasswordHash("Karamba123"),
                Username = "user1"
            };

            context.Users.Add(activeUser);

            var inActiveUser = new User()
            {
                Email = "other.email@mail.com",
                IsActive = false,
                PasswordHash = Repositories.UserRepository.GetPasswordHash("Password1"),
                Username = "user2"
            };

            context.Users.Add(inActiveUser);

            context.SaveChanges();
        }
    }
}
