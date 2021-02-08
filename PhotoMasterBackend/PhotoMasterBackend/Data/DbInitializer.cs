using PhotoMasterBackend.Models;
using System.Linq;

namespace PhotoMasterBackend.Data
{
    public class DbInitializer
    {
        public static void Initialize(PhotoContext context)
        {
            context.Database.EnsureCreated();

            // If DB contains already labels ==> nothing need to be done
            if (context.Labels.Any())
            {
                return;
            }

            // Initialize labels
            var labels = new Label[]
            {
                new Label { Name = "Daily", Color = "red"},
                new Label { Name = "Travel", Color = "blue"},
                new Label { Name = "Pet", Color = "green"},
                new Label { Name = "Foods", Color = "orange"},
                new Label { Name = "Nature", Color = "cyan"}
            };

            // Add labels into DB
            foreach (var label in labels)
            {
                context.Labels.Add(label);
            }
            context.SaveChanges();

            // Initialize users
            var users = new User[]
            {
                new User { Username = "user_admin", Password = "0761174569", Role = Role.Admin},
                new User { Username = "user", Password = "user", Role = Role.Friend},
                new User { Username = "visitor_admin", Password = "0761174569", Role = Role.Admin},
                new User { Username = "visitor", Password = "visitor", Role = Role.Visitor}
            };

            // Add users into DB
            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }
    }
}
