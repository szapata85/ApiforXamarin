﻿namespace DB.Data
{
    public class SeedDb
    {
        private readonly DatabaseContext context;
        private readonly Random random;

        public SeedDb(DatabaseContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            if (!this.context.Clients.Any())
            {
                this.AddClient("First Client");
                this.AddClient("Second Client");
                this.AddClient("Third Client");

                await this.context.SaveChangesAsync();
            }
        }

        private void AddClient(string name)
        {
            Model.Client client = new Model.Client()
            {
                Name = name,
                Dna = this.random.Next(1000000, 1999999).ToString()
            };

            this.context.Clients.Add(client);
        }
    }
}