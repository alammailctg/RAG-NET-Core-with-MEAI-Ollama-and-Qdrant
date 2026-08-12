using Nuruddin.RagApp;

public static class ProductDatabase
{
    public static List<Product> GetProducts()
    {
        return new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Wireless Noise-Canceling Headphones", Description = "Over-ear Bluetooth headphones with active noise cancellation, 30-hour battery life, and high-fidelity sound." },
            new() { Id = Guid.NewGuid(), Name = "Ergonomic Gaming Mouse", Description = "Precision optical gaming mouse featuring customizable RGB lighting, high DPI settings, and programmable side buttons." },
            new() { Id = Guid.NewGuid(), Name = "34-Inch Ultra-Wide Curved Monitor", Description = "Immersive curved display featuring a 144Hz refresh rate, HDR400 support, and crisp 3440x1440 resolution for gaming and productivity." },
            new() { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", Description = "Compact mechanical keyboard built with tactile switches, dynamic backlight settings, and a durable aluminum frame." },
            new() { Id = Guid.NewGuid(), Name = "Smart Fitness Watch", Description = "Water-resistant smartwatch equipped with real-time heart rate monitoring, GPS tracking, sleep analysis, and dynamic workouts." }
        };
    }
}