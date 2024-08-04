using Libray.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Libray.ViewModel;

namespace Libray.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
		{
			Seed();
		}
		public DbSet<LibaryModel> Libaries { get; set; }
		public DbSet<ShelfModel> Shelfs { get; set; }
		public DbSet<SetModel> Sets { get; set; }	
		public DbSet<BookModel> Books { get; set; }

		private void Seed()
		{
			if (Libaries.IsNullOrEmpty())
			{

				List<LibaryModel> libaries = [
					new()
					{
						Ganre="Tora",
						Shelfes =[
							new()
							{
								High= 30,
								Width= 80,
								sets = [
									new()
									{
										SetName="Humash",
										books=[
											new()
											{

												Title="Bereshit",
												Ganre="Tora",
												Hige = 25,
												Width= 3
											},
											new()
											{

												Title="Shemot",
												Ganre="Tora",
												Hige = 25,
												Width= 4
											},
											new()
											{

												Title="Vayikra",
												Ganre="Tora",
												Hige = 25,
												Width= 3
											},
											new()
											{

												Title="Bamidbar",
												Ganre="Tora",
												Hige = 25,
												Width= 5
											},
											new()
											{

												Title="Dvarim",
												Ganre="Tora",
												Hige = 25,
												Width= 4
											}
											
											   ]
									}
									]
							}
							]
					}
				];
				Libaries.AddRange( libaries );
				SaveChanges();
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LibaryModel>()
				.HasMany(l => l.Shelfes)
				.WithOne(s => s.Libary)
				.HasForeignKey(s => s.LibaryId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<ShelfModel>()
				.HasMany(s => s.sets)
				.WithOne(se => se.Shelf)
				.HasForeignKey(se => se.ShelfId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<SetModel>()
				.HasMany(s => s.books)
				.WithOne(b => b.set)
				.HasForeignKey(b => b.SetId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	    
	}
}
