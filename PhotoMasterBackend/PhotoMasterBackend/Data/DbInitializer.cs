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
                new Label { Name = "Daily"},
                new Label { Name = "Travel"},
                new Label { Name = "Pet"},
                new Label { Name = "Foods"},
                new Label { Name = "Nature"}
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
