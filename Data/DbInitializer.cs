using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;
namespace WebShop.Data
{
    internal class DbInitializer
    {
        internal static async Task Initializer(MyDbContext db) 
        {
            await db.Database.EnsureCreatedAsync();
            await WholesalerInitializer(db);
            await CategoryInitializer(db);
            await ProductInitializer(db);
            await CustomerInitializer(db);
            await OrderInitializer(db);

        }
        internal static async Task WholesalerInitializer(MyDbContext db)
        {
            if (await db.Wholesalers.AnyAsync()) return;

            db.Wholesalers.AddRange(
                new Wholesaler { Name = "Impecta Fröhandel", Email = "info@impecta.se" },
                new Wholesaler { Name = "Växtriket", Email = "kontakt@vaxtriket.se" },
                new Wholesaler { Name = "Plantgrossisten", Email = "order@plantgrossisten.se" },
                new Wholesaler { Name = "Nordiska Plantor", Email = "info@nordiskaplantor.se" },
                new Wholesaler { Name = "Trädgårdsgrossisten", Email = "kundtjanst@tradgardsgrossisten.se" },
                new Wholesaler { Name = "Blomsterbörsen", Email = "order@blomsterborsen.se" },
                new Wholesaler { Name = "Gröna Marknaden", Email = "info@gronamarknaden.se" },
                new Wholesaler { Name = "Växtspecialisten", Email = "kontakt@vaxtspecialisten.se" },
                new Wholesaler { Name = "Botaniska Partihandeln", Email = "order@botaniska.se" },
                new Wholesaler { Name = "Svenska Odlarföreningen", Email = "info@odlarforeningen.se" }
            ); 

            await db.SaveChangesAsync();
        }
        internal static async Task CategoryInitializer(MyDbContext db) 
        {
            if (await db.Categories.AnyAsync()) return;
            
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
           await db.SaveChangesAsync();

        }
        internal static async Task ProductInitializer(MyDbContext db)
        {
            if (await db.Products.AnyAsync()) return;

            // Vi hämtar de existerande kategorierna från DB till en dictionary
            var cat = await db.Categories.ToDictionaryAsync(c => c.Name);
            var who = await db.Wholesalers.ToDictionaryAsync(w => w.Name);
            db.Products.AddRange(
                // --- KRYDDVÄXTER ---
                new Product("Basilika 'Genovese'", "Ocimum basilicum", 2590, "Klassisk pesto-basilika.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Gräslök", "Allium schoenoprasum", 2250, "Mild löksmak, perenn favorit.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Kruspersilja", "Petroselinum crispum", 1990, "Klassisk dekoration och krydda.", Sunlight.MediumShade, Water.Medium, LifeCycle.Biennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Bienner"], cat["Kryddväxter"] }),
                new Product("Dill 'Mammut'", "Anethum graveolens", 2450, "Högväxande dill för kräftskivan.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Timjan", "Thymus vulgaris", 2890, "Tålig och aromatisk krydda.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Oregano", "Origanum vulgare", 2750, "Ett måste till pizza, älskas av bin.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Pollinerare"], cat["Kryddväxter"] }),
                new Product("Koriander", "Coriandrum sativum", 2190, "Snabbväxande för asiatisk mat.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Kryddväxter"] }),
                new Product("Rosmarin", "Salvia rosmarinus", 3490, "Underbar doft, tål torka bra.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Citronmeliss", "Melissa officinalis", 2350, "Frisk citrondoft, sprider sig lätt.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),
                new Product("Fransk Dragon", "Artemisia dracunculus", 3890, "Gourmetkrydda till bearnaise.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Kryddväxter"] }),

                // --- ANUELLER / BLADVÄXTER ---
                new Product("Jätteverbena", "Verbena bonariensis", 3990, "Hög och elegant, svävar över rabatten.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Pollinerare"] }),
                new Product("Stor Tagetes", "Tagetes erecta", 2650, "Maffiga orange bollar.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Sammetstagetes", "Tagetes patula", 2490, "Tacksam och lågväxande trotjänare.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Palettblad 'Rainbow'", "Solenostemon", 4250, "Fantastiska färger på bladen.", Sunlight.MediumShade, Water.High, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Bladväxter"] }),
                new Product("Lejongap", "Antirrhinum majus", 2290, "Klassisk klassiker som barnen älskar.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),

                // --- SOMMARBLOMMOR ---
                new Product("Styvmorsviol", "Viola tricolor", 1890, "Söt liten pensé för tidig vår.", Sunlight.MediumShade, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Blommande"] }),
                new Product("Stjärnklocka", "Campanula isophylla", 4490, "Perfekt för sommarampeln.", Sunlight.MediumShade, Water.Medium, LifeCycle.Annual, 100, who["Växtriket"].Id, new List<Category> { cat["Plantor"], cat["Annueller"], cat["Blommande"] }), // Korrigerad kategori här!
                new Product("Prästkrage", "Leucanthemum vulgare", 2950, "Svensk sommar i en blomma.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Perenner"], cat["Pollinerare"] }),
                new Product("Ringblomma", "Calendula officinalis", 2390, "Lättodlad och ätbar blomma.", Sunlight.FullSun, Water.Medium, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Pollinerare"] }),
                new Product("Blomman för dagen", "Ipomoea purpurea", 2750, "Snabbväxande klättrare med stora klockor.", Sunlight.FullSun, Water.High, LifeCycle.Annual, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Fröer"], cat["Annueller"], cat["Solälskare"] }),

                // --- LÖKAR & KNÖLAR ---
                new Product("Queen of Night", "Tulipa", 7900, "Sammetslen, nästan svart tulpan.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 50, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Apeldoorn", "Tulipa Darwin", 6900, "Klassisk stor röd tulpan.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 100, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Angelique", "Tulipa", 8900, "Rosa dubbel tulpan som liknar en pion.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 40, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Spring Green", "Tulipa Viridiflora", 7500, "Elegant vit tulpan med gröna strimmor.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 60, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Tête-à-tête", "Narcissus", 4900, "Populär minipåsklilja, perfekt i kruka.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 200, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Mount Hood", "Narcissus", 8500, "Storblommande trumpetnarciss i krämvitt.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 35, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Ice Follies", "Narcissus", 6500, "Vit krage med ljust gul klocka.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 80, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Blue Jacket", "Hyacinthus orientalis", 5900, "Mörkblå hyacint med stark doft.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 120, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("White Pearl", "Hyacinthus orientalis", 5900, "Snövita blommor på kraftig stängel.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 90, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Pärlhyacint", "Muscari armeniacum", 3900, "Täta klarblå klasar, sprider sig lätt.", Sunlight.MediumShade, Water.Low, LifeCycle.Perennial, 150, who["Impecta Fröhandel"].Id, new List<Category> { cat["Lökar & Knölar"], cat["Blommande"], cat["Skuggälskare"] }),

                // --- BUSKAR ---
                new Product("Syren", "Syringa vulgaris", 24900, "Klassisk doftande lila syren.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 25, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Ölandstok", "Dasiphora fruticosa", 14900, "Tålig buske med gula blommor hela sommaren.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 40, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Hortensia", "Hydrangea macrophylla", 28900, "Stora blå blomklasar, trivs i surjord.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 15, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Brudspirea", "Spiraea arguta", 19900, "Översållas av små vita blommor på våren.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 20, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Schersmin", "Philadelphus", 22900, "Underbar stark doft av smultron.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 12, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Buxbom", "Buxus sempervirens", 17900, "Vintergrön buske perfekt för formklippning.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 50, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Smällspirea", "Physocarpus opulifolius", 21900, "Mörkt purpurröda blad som ger kontrast.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 18, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Solälskare"] }),
                new Product("Forsythia", "Forsythia", 18900, "Vårens första gula blommor på bar kvist.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 22, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Liguster", "Ligustrum vulgare", 12900, "Snabbväxande och tålig häckväxt.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 100, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Rhododendron", "Rhododendron", 34900, "Praktfull blomning, kräver skugga och fukt.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 10, who["Växtriket"].Id, new List<Category> { cat["Buskar"], cat["Blommande"], cat["Skuggälskare"] }),

                // --- PERENNER SOM ÄVEN ÄR BUSKAR ---
                new Product("Lavendel", "Lavandula angustifolia", 8900, "Doftande silvergrå buske med lila spiror.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 60, who["Växtriket"].Id, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Solälskare"] }),
                new Product("Bondpion", "Paeonia x festiva", 24900, "Klassisk mormorsblomma med fyllda röda bollar.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 15, who["Växtriket"].Id, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Blommande"] }),
                new Product("Höstsilverax", "Actaea simplex", 19500, "Högrest busklik perenn med mörka blad.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 12, who["Växtriket"].Id, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Skuggälskare"] }),
                new Product("Hybridjulros", "Helleborus", 16900, "Vintergrön och buskig, blommar tidigt på våren.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 25, who["Växtriket"].Id, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Skuggälskare"] }),
                new Product("Salvia", "Salvia officinalis", 6500, "Halvbuske med grågröna blad, utmärkt som krydda.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 45, who["Växtriket"].Id, new List<Category> { cat["Perenner"], cat["Buskar"], cat["Kryddväxter"] }),

                // --- ÖVRIGA PERENNER ---
                new Product("Riddarsporre", "Delphinium", 12900, "Ståtliga blå spiror som ger höjd.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 30, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Funkia", "Hosta", 11500, "Fantastiska bladväxt för skuggiga lägen.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 80, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Daglilja", "Hemerocallis", 13900, "Tålig perenn med liljelika blommor.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 55, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Alunrot", "Heuchera", 9800, "Kompakt perenn med blad i magiska färger.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 110, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Kärleksört", "Hylotelephium", 7900, "Höstblommande favorit som älskas av bin.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 140, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Stäppsalvia", "Salvia nemorosa", 8500, "Lila spiror som blommar länge.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 100, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Jättedaggkåpa", "Alchemilla mollis", 8900, "Limegröna blomskyar.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 180, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Solhatt", "Echinacea purpurea", 11900, "Stora rosa blommor med mörk mitt.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 65, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Akleja", "Aquilegia", 7500, "Elegant och gammaldags perenn.", Sunlight.MediumShade, Water.Medium, LifeCycle.Perennial, 90, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Skuggälskare"] }),
                new Product("Kantnepeta", "Nepeta faassenii", 6900, "Katternas favorit, blommar hela sommaren.", Sunlight.FullSun, Water.Low, LifeCycle.Perennial, 200, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Pollinerare"], cat["Solälskare"] }),
                new Product("Ullungrönn", "Dodonaea", 14500, "Vacker bladväxt med fin höstfärg.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 15, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Solälskare"] }),
                new Product("Balkanförgätmigej", "Brunnera macrophylla", 12500, "Hjärtformade blad och små blå blommor.", Sunlight.MediumShade, Water.High, LifeCycle.Perennial, 40, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Skuggälskare"] }),
                new Product("Höstaster", "Symphyotrichum", 9500, "Ger färg till trädgården sent på säsongen.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 75, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Blommande"], cat["Solälskare"] }),
                new Product("Tuvrör", "Calamagrostis", 13500, "Prydnadsgräs som står fint hela vintern.", Sunlight.FullSun, Water.Medium, LifeCycle.Perennial, 50, who["Trädgårdsgrossisten"].Id, new List<Category> { cat["Perenner"], cat["Bladväxter"], cat["Solälskare"] })
            );

            await db.SaveChangesAsync();
 
        }
        internal static async Task CustomerInitializer(MyDbContext db)
        {
            if (await db.Customers.AnyAsync()) return;

            db.Customers.AddRange(new List<Customer>
        {
        new Customer("Sten Stensson", "sten@stenmail.com", "Stengatan 3", "554 33", "Stenstad", "sten123", false),
        new Customer("Anna Björk", "anna@annamail.com", "Skogsstigen 12", "123 45", "Grönvik", "anna123", true),
        new Customer("Bo Ek", "bo@bomail.com", "Ekallén 1", "998 77", "Lund", "bo123", false),
        new Customer("David Dahlia", "david@davidmail.com", "Blomvägen 8", "443 21", "Uppsala", "david123", false),
        new Customer("Erik Ek", "erik@erikmail.com", "Ekvägen 2", "556 67", "Eksjö", "erik123", true),
        new Customer("Maria Mynta", "maria@mariamail.com", "Örtvägen 5", "443 21", "Örebro", "maria123", true),
        new Customer("Cecilia Citron", "cecilia@ceciliamail.com", "Citrusgränd 1", "112 23", "Malmö", "cecilia123", true),
        new Customer("Fredrik Furu", "fredrik@fredrikmail.com", "Tallvägen 4", "887 66", "Umeå", "fredrik123", false),
        new Customer("Gunilla Gran", "gunilla@gunillamail.com", "Grantoppen 9", "223 44", "Kiruna", "gunilla123", true),
        new Customer("Håkan Hägg", "hakan@hakanmail.com", "Buskgatan 2", "334 55", "Gävle", "hakan123", false),
        new Customer("Inez Idgran", "inez@inezmail.com", "Barrstigen 7", "665 44", "Borås", "inez123", true),
        new Customer("Johan Järnek", "johan@johanmail.com", "Lövstigen 11", "778 88", "Halmstad", "johan123", false),
        new Customer("Karin Kastanj", "karin@karinmail.com", "Nötvägen 3", "998 22", "Västerås", "karin123", true),
        new Customer("Lars Lind", "lars@larsmail.com", "Parkvägen 15", "445 66", "Kalmar", "lars123", false),
        new Customer("Mona Malva", "mona@monamail.com", "Blomstervägen 22", "332 11", "Visby", "mona123", true),
        new Customer("Nils Nypon", "nils@nilsmail.com", "Snårvägen 6", "554 22", "Ystad", "nils123", false),
        new Customer("Olle Olvon", "olle@ollemail.com", "Bärvägen 1", "112 44", "Skövde", "olle123", true),
        new Customer("Pia Pion", "pia@piamail.com", "Rabattvägen 9", "887 11", "Varberg", "pia123", true),
        new Customer("Quentin盆栽", "quentin@quentinmail.com", "Odlingsgränd 4", "221 33", "Piteå", "quentin123", false),
        new Customer("Rosa Ros", "rosa@rosamail.com", "Törnstigen 10", "443 55", "Eskilstuna", "rosa123", true)
        });

            await db.SaveChangesAsync();

        }
        internal static async Task OrderInitializer(MyDbContext db)
        {
            if (await db.Orders.AnyAsync()) return;

            // Hämta in dina 20 kunder och dina befintliga produkter
            var allCustomers = await  db.Customers.ToListAsync();
            var allProducts = await db.Products.ToListAsync();

            if (!allProducts.Any() || !allCustomers.Any())
            {
                Console.WriteLine("Fel: Kör Product- och Customer-initializers först!");
                return;
            }

            var rnd = new Random();
            var allOrders = new List<Order>();

            // Gå igenom VARJE kund så att alla får minst en order
            foreach (var customer in allCustomers)
            {
                // Slumpa antal ordrar för denna kund (mellan 1 och 5)
                int numberOfOrders = rnd.Next(1, 6);

                for (int i = 0; i < numberOfOrders; i++)
                {
                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        CustomerName = customer.CustomerName,
                        StreetAdress = customer.CustomerAdress,
                        ZipCode = customer.ZipCode,
                        City = customer.CustomerCity,
                        CustomerEmail = customer.CustomerEmail,
                        Newsletter = customer.CustomerNewsLetter,
                        // Slumpa datum under det senaste året
                        OrderDate = DateTime.Now.AddDays(-rnd.Next(0, 365)),
                        OrderItems = new List<OrderItem>()
                    };

                    // Slumpa 1-4 olika produkt-rader per order
                    int itemsInOrder = rnd.Next(1, 5);
                    for (int j = 0; j < itemsInOrder; j++)
                    {
                        var p = allProducts[rnd.Next(allProducts.Count)];

                        // Undvik dubbletter av samma produkt i samma order
                        if (!order.OrderItems.Any(x => x.ProductId == p.Id))
                        {
                            order.OrderItems.Add(new OrderItem(
                                p.Id,
                                p.Name,
                                rnd.Next(1, 4), // Antal 1-3
                                p.Price
                            ));
                        }
                    }

                    // Summera totalpriset för ordern
                    order.TotalOrderPrice = order.OrderItems.Sum(oi => oi.PriceAtPurchase * oi.ProductQuantity);
                    allOrders.Add(order);
                }
            }

            // Spara alla ordrar och deras OrderItems i ett svep
            db.Orders.AddRange(allOrders);
            await db.SaveChangesAsync();

            Console.WriteLine($"Klart! Skapade {allOrders.Count} ordrar fördelat på {allCustomers.Count} kunder.");
            Console.WriteLine("Varje kund har nu mellan 1 och 5 ordrar. Tryck på valfri tangent.");
            
        }
    }
    
}
