using System;

namespace NorthwindWebApps
{
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using Bogus;
    using Northwind.Serivces.EntityFrameworkCore;
    using Northwind.Services.Employees;
    using Northwind.Services.Products;

    /// <summary>
    /// Generates random data.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Generator of fake data.
        /// </summary>
        /// <param name="context">Context of which data to generate.</param>
        /// <param name="itemCount">Count of generated items.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="context"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="itemCount"/> is less or equals 0.</exception>
        public static void GenerateSeedData(NorthwindContext context, int itemCount)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (itemCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{itemCount} cannot be less or equal zero");
            }

            string[] urls =
            {
                "https://www.c-sharpcorner.com/App_Themes/CSharp/Images/SiteLogo.png",
                "https://thumb.tildacdn.com/tild6237-6265-4232-a233-663832313834/-/resize/379x/-/format/webp/noroot.png",
                "https://e7.pngegg.com/pngimages/679/718/png-clipart-globe-globe.png",
                "https://thumb.tildacdn.com/tild6130-3035-4930-a366-323935343362/-/resize/860x/-/format/webp/noroot.png",
                "https://w7.pngwing.com/pngs/114/579/png-transparent-pink-cross-stroke-ink-brush-pen-red-ink-brush-ink-leave-the-material-text.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/640px-PNG_transparency_demonstration_1.png",
                "https://static.remove.bg/remove-bg-web/f4b1a5b6ab0be77785c26078f8a7569489d184da/assets/start_remove-c851bdf8d3127a24e2d137a55b1b427378cd17385b01aec6e59d5d4b5f39d2ec.png",
                "https://mdn.mozillademos.org/files/12940/picture-element-wide.png",
                "https://mdn.mozillademos.org/files/12934/network-devtools.png",
                "https://posiflora.com/wp-content/uploads/cover-30-2048x910.png",
            };

            using var webClient = new WebClient();

            if (!context.ProductCategories.Any())
            {
                int id = 1;
                context.ProductCategories.AddRange(new Faker<ProductCategory>("en")
                    .RuleFor(x => x.Id, x => id++)
                    .RuleFor(x => x.Name, x => x.Commerce.Categories(1).First())
                    .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
                    .RuleFor(x => x.Picture, f => webClient.DownloadData(urls[f.Random.Number(0, 9)]))
                    .Generate(itemCount));

                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                int id = 1;
                context.Products.AddRange(new Faker<Product>("en")
                    .RuleFor(x => x.Id, x => id++)
                    .RuleFor(x => x.CategoryId, f => f.Random.Int(1, itemCount).OrNull(f, .2f))
                    .RuleFor(x => x.SupplierId, f => f.Random.Int(1, itemCount).OrNull(f, .2f))
                    .RuleFor(x => x.Discontinued, x => x.Random.Bool())
                    .RuleFor(x => x.Name, x => x.Commerce.ProductName())
                    .RuleFor(x => x.QuantityPerUnit, f => f.Random.Number(1, 100).ToString(CultureInfo.CurrentCulture))
                    .RuleFor(x => x.ReorderLevel, f => f.Random.Short(1, 10).OrNull(f, .2f))
                    .RuleFor(x => x.UnitPrice, f => f.Random.Decimal(0.5m, 150m).OrNull(f, .2f))
                    .RuleFor(x => x.UnitsInStock, f => f.Random.Short(0, 50).OrNull(f, .2f))
                    .RuleFor(x => x.UnitsOnOrder, f => f.Random.Short(0, 50).OrNull(f, .2f))
                    .Generate(itemCount));

                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                int id = 1;

                context.Employees.AddRange(new Faker<Employee>("en")
                    .RuleFor(x => x.Id, x => id++)
                    .RuleFor(x => x.BirthDate, f => f.Date.Past(50))
                    .RuleFor(x => x.City, f => f.Address.City())
                    .RuleFor(x => x.Country, f => f.Address.Country())
                    .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                    .RuleFor(x => x.LastName, f => f.Name.LastName())
                    .RuleFor(x => x.Notes, f => f.Lorem.Sentences(2))
                    .RuleFor(x => x.PhotoPath, f => f.System.FilePath())
                    .RuleFor(x => x.ReportsTo, f => f.Random.Number(1, itemCount))
                    .RuleFor(x => x.Photo, f => webClient.DownloadData(urls[f.Random.Number(0, 9)]))
                    .RuleFor(x => x.Extension, f => f.Lorem.Letter(4))
                    .RuleFor(x => x.Region, f => f.Address.County())
                    .RuleFor(x => x.HomePhone, f => f.Phone.PhoneNumber())
                    .RuleFor(x => x.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(x => x.Title, f => f.Name.JobTitle())
                    .RuleFor(x => x.TitleOfCourtesy, f => f.Name.JobDescriptor())
                    .RuleFor(x => x.HireDate, f => f.Date.Past(50))
                    .RuleFor(x => x.Address, f => f.Address.SecondaryAddress())
                    .Generate(itemCount));

                context.SaveChanges();
            }
        }
    }
}
