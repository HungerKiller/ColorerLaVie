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
        }
    }
}
