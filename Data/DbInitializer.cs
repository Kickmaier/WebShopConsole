using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Data
{
    internal class DbInitializer
    {
        public static void Initializer(MyDbContext db) 
        {
            db.Database.EnsureCreated();
            CategoryInitializer(db);
            ProductInitializer(db);
        }
        public static void CategoryInitializer(MyDbContext db) 
        {
            if (db.Categories.Any())
            {
                return;
            }

            db.Categories.AddRange(
        new Category { Name = "Perenner" },
        new Category { Name = "Annueller" },
        new Category { Name = "Bienner" },
        new Category { Name = "Fröer" },
        new Category { Name = "Lökar & Knölar" },
        new Category { Name = "Plantor" },
        new Category { Name = "Pollinerare" },
        new Category { Name = "Solälskare" },
        new Category { Name = "Skuggälskare" },
        new Category { Name = "Buskar" },
        new Category { Name = "Blommande" },
        new Category { Name = "Bladväxter" },
        new Category { Name = "Kryddväxter" }
        );
            Console.WriteLine("Categoreis Done\nPress any key");
            db.SaveChanges();
            Console.ReadLine();
        }

public static void ProductInitializer(MyDbContext db)
        {
            if (db.Products.Any()) return;

            // Vi hämtar de existerande kategorierna från DB till en dictionary
            var cat = db.Categories.ToDictionary(c => c.Name);

            db.Products.AddRange(
                // --- KRYDDVÄXTER ---
                new Product("Basilika 'Genovese'", "Ocimum basilicum", 2590, "Klassisk pesto-basilika.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Gräslök", "Allium schoenoprasum", 2250, "Mild löksmak, perenn favorit.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Kruspersilja", "Petroselinum crispum", 1990, "Klassisk dekoration och krydda.", Sunlight.MediumShade, Water.Medium, LifeCycle.Biennial, 100, new List<Category> { cat["Fröer"], cat["Bienner"], cat["Kryddväxter"] }),
                new Product("Dill 'Mammut'", "Anethum graveolens", 2450, "Högväxande dill för kräftskivan.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Timjan", "Thymus vulgaris", 2890, "Tålig och aromatisk krydda.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Oregano", "Origanum vulgare", 2750, "Ett måste till pizza, älskas av bin.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Pollinerare"], cat["Kryddväxter"] }),
                new Product("Koriander", "Coriandrum sativum", 2190, "Snabbväxande för asiatisk mat.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Rosmarin", "Salvia rosmarinus", 3490, "Underbar doft, tål torka bra.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Citronmeliss", "Melissa officinalis", 2350, "Frisk citrondoft, sprider sig lätt.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Fransk Dragon", "Artemisia dracunculus", 3890, "Gourmetkrydda till bearnaise.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),

                // --- ANUELLER / BLADVÄXTER ---
                new Product("Jätteverbena", "Verbena bonariensis", 3990, "Hög och elegant, svävar över rabatten.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Pollinerare"] }),
                new Product("Stor Tagetes", "Tagetes erecta", 2650, "Maffiga orange bollar.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Sammetstagetes", "Tagetes patula", 2490, "Tacksam och lågväxande trotjänare.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Palettblad 'Rainbow'", "Solenostemon", 4250, "Fantastiska färger på bladen.", Sunlight.MediumShade, Water.High, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Bladväxter"] }),
                new Product("Lejongap", "Antirrhinum majus", 2290, "Klassisk klassiker som barnen älskar.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),

                // --- SOMMARBLOMMOR ---
                new Product("Styvmorsviol", "Viola tricolor", 1890, "Söt liten pensé för tidig vår.", Sunlight.MediumShade, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Stjärnklocka", "Campanula isophylla", 4490, "Perfekt för sommarampeln.", Sunlight.MediumShade, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Plantor"], cat["Annueller"], cat["Blommande"] }), // Korrigerad kategori här!
                new Product("Prästkrage", "Leucanthemum vulgare", 2950, "Svensk sommar i en blomma.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Pollinerare"] }),
                new Product("Ringblomma", "Calendula officinalis", 2390, "Lättodlad och ätbar blomma.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Pollinerare"] }),
                new Product("Blomman för dagen", "Ipomoea purpurea", 2750, "Snabbväxande klättrare med stora klockor.", Sunlight.FullSun, Water.High, LifeCycle.Annual, 100, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Solälskare"] }),

                // --- LÖKAR & KNÖLAR ---
                new Product("Queen of Night", "Tulipa", 7900, "Sammetslen, nästan svart tulpan.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 50, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Apeldoorn", "Tulipa Darwin", 6900, "Klassisk stor röd tulpan.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Angelique", "Tulipa", 8900, "Rosa dubbel tulpan som liknar en pion.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 40, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Spring Green", "Tulipa Viridiflora", 7500, "Elegant vit tulpan med gröna strimmor.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 60, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Tête-à-tête", "Narcissus", 4900, "Populär minipåsklilja, perfekt i kruka.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 200, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Mount Hood", "Narcissus", 8500, "Storblommande trumpetnarciss i krämvitt.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 35, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Ice Follies", "Narcissus", 6500, "Vit krage med ljust gul klocka.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 80, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Blue Jacket", "Hyacinthus orientalis", 5900, "Mörkblå hyacint med stark doft.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 120, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("White Pearl", "Hyacinthus orientalis", 5900, "Snövita blommor på kraftig stängel.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 90, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Pärlhyacint", "Muscari armeniacum", 3900, "Täta klarblå klasar, sprider sig lätt.", Sunlight.MediumShade, Water.Low, LifeCycle.Perennial, 150, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),

                // --- BUSKAR ---
                new Product("Syren", "Syringa vulgaris", 24900, "Klassisk doftande lila syren.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 25, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Ölandstok", "Dasiphora fruticosa", 14900, "Tålig buske med gula blommor hela sommaren.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 40, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Hortensia", "Hydrangea macrophylla", 28900, "Stora blå blomklasar, trivs i surjord.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 15, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Brudspirea", "Spiraea arguta", 19900, "Översållas av små vita blommor på våren.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 20, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Schersmin", "Philadelphus", 22900, "Underbar stark doft av smultron.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 12, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Buxbom", "Buxus sempervirens", 17900, "Vintergrön buske perfekt för formklippning.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 50, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Smällspirea", "Physocarpus opulifolius", 21900, "Mörkt purpurröda blad som ger kontrast.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 18, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Solälskare"] }),
                new Product("Forsythia", "Forsythia", 18900, "Vårens första gula blommor på bar kvist.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 22, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Liguster", "Ligustrum vulgare", 12900, "Snabbväxande och tålig häckväxt.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 100, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Rhododendron", "Rhododendron", 34900, "Praktfull blomning, kräver skugga och fukt.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 10, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Skuggälskare"] }),

                // --- PERENNER SOM ÄVEN ÄR BUSKAR ---
                new Product("Lavendel", "Lavandula angustifolia", 8900, "Doftande silvergrå buske med lila spiror.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 60, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Solälskare"] }),
                new Product("Bondpion", "Paeonia x festiva", 24900, "Klassisk mormorsblomma med fyllda röda bollar.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 15, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Blommande"] }),
                new Product("Höstsilverax", "Actaea simplex", 19500, "Högrest busklik perenn med mörka blad.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 12, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Skuggälskare"] }),
                new Product("Hybridjulros", "Helleborus", 16900, "Vintergrön och buskig, blommar tidigt på våren.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 25, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Skuggälskare"] }),
                new Product("Salvia", "Salvia officinalis", 6500, "Halvbuske med grågröna blad, utmärkt som krydda.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 45, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Kryddväxter"] }),

                // --- ÖVRIGA PERENNER ---
                new Product("Riddarsporre", "Delphinium", 12900, "Ståtliga blå spiror som ger höjd.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 30, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Funkia", "Hosta", 11500, "Fantastiska bladväxt för skuggiga lägen.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 80, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Daglilja", "Hemerocallis", 13900, "Tålig perenn med liljelika blommor.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 55, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Alunrot", "Heuchera", 9800, "Kompakt perenn med blad i magiska färger.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 110, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Kärleksört", "Hylotelephium", 7900, "Höstblommande favorit som älskas av bin.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 140, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Stäppsalvia", "Salvia nemorosa", 8500, "Lila spiror som blommar länge.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Jättedaggkåpa", "Alchemilla mollis", 8900, "Limegröna blomskyar.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 180, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Solhatt", "Echinacea purpurea", 11900, "Stora rosa blommor med mörk mitt.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 65, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Akleja", "Aquilegia", 7500, "Elegant och gammaldags perenn.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 90, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Kantnepeta", "Nepeta faassenii", 6900, "Katternas favorit, blommar hela sommaren.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 200, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Ullungrönn", "Dodonaea", 14500, "Vacker bladväxt med fin höstfärg.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 15, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Solälskare"] }),
                new Product("Balkanförgätmigej", "Brunnera macrophylla", 12500, "Hjärtformade blad och små blå blommor.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 40, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Höstaster", "Symphyotrichum", 9500, "Ger färg till trädgården sent på säsongen.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 75, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Tuvrör", "Calamagrostis", 13500, "Prydnadsgräs som står fint hela vintern.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 50, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Solälskare"] })
            );

            db.SaveChanges();
        }
    }
    
}
