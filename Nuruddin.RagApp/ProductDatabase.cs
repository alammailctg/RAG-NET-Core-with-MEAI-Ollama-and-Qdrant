using System;
using System.Collections.Generic;
using System.Text;

namespace Nuruddin.RagApp
{
    public static class ProductDatabase
    {
        public static List<Product> GetProducts()
        {
            var productData = new List<Product>()
            {
                new()
                {
                    Id = 1,
                    Name = "Wireless Noise-Canceling Headphones",
                    Description = "Over-ear Bluetooth headphones with active noise cancellation, 30-hour battery life, and high-fidelity sound."
                },
                new()
                {
                    Id = 2,
                    Name = "Ergonomic Gaming Mouse",
                    Description = "Precision optical gaming mouse featuring customizable RGB lighting, high DPI settings, and programmable side buttons."
                },
                new()
                {
                    Id = 3,
                    Name = "34-Inch Ultra-Wide Curved Monitor",
                    Description = "Immersive curved display featuring a 144Hz refresh rate, HDR400 support, and crisp 3440x1440 resolution for gaming and productivity."
                },
                new()
                {
                    Id = 4,
                    Name = "Mechanical Keyboard",
                    Description = "Compact mechanical keyboard built with tactile switches, dynamic backlight settings, and a durable aluminum frame."
                },
                new()
                {
                    Id = 5,
                    Name = "Smart Fitness Watch",
                    Description = "Water-resistant smartwatch equipped with real-time heart rate monitoring, GPS tracking, sleep analysis, and dynamic workouts."
                }
            };

            return productData;
        }
    }
}
