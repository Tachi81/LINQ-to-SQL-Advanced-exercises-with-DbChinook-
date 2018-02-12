using Chinook.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinook
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2. Ile mamy rekordow w tabelach Artist i Album?
            // ShowAmountOfArtistsAndAlbums();

            //  3. Polacz tabele Artist i Album – wyswietl rowniez Artystow bez albumow.Ile jest Artystow bez albumow?
            //AmountOfArtistsWithoutAlbum();

            //4. Wyswietl wszystkich artystow ktorzy maja dokladnie 4 albumy
            // AmountOfArtistsWithExactlyFourAlbums();

            //5. Ilu Customerów pochodzi z Germany?
            //AmountOfCustomersFronGermany();

            //6. ktorzy customerzy wydaja najwiecej pieniedzy?
            // ListOfValuableCustomers();

            //7. Ktore Track sa najczesciej kupowane?
            // TracsMostWanted();

            //8. (optional) jakiego Artysty utwory sa najczesciej kupowane w danym kraju?
            BestArtistsPerCountry();

            Console.WriteLine();
            Console.WriteLine("program finished without exceptions");
            Console.ReadKey();
        }

        // 2. Ile mamy rekordow w tabelach Artist i Album?
        private static void ShowAmountOfArtistsAndAlbums()
        {
            using (ChinookContext DbContext = new ChinookContext())
            {
                var ArtistsCount = DbContext.Artists.Count();
                var albumCount = DbContext.Albums.Count();
                Console.WriteLine($" W bazie jest {ArtistsCount} artystów i {albumCount} albumów");
            }
        }

        //  3. Polacz tabele Artist i Album – wyswietl rowniez Artystow bez albumow.Ile jest Artystow bez albumow?
        private static void AmountOfArtistsWithoutAlbum()
        {
            using (ChinookContext DbContext = new ChinookContext())
            {
                var artistWithoutAlbum = from ar in DbContext.Artists
                                         from al in DbContext.Albums.Where(al => al.ArtistId == ar.ArtistId).DefaultIfEmpty()
                                         where al.ArtistId.Equals(null)
                                         select new
                                         {
                                             cos = ar
                                         };
                var liczba = artistWithoutAlbum.Count();
                Console.WriteLine($"{liczba} artystów nie ma albumu");
            }
        }

        //4. Wyswietl wszystkich artystow ktorzy maja dokladnie 4 albumy

        private static void AmountOfArtistsWithExactlyFourAlbums()
        {
            using (ChinookContext DbContext = new ChinookContext())
            {
                var artistWithFourAlbums = from ar in DbContext.Artists
                                           join al in DbContext.Albums
                                           on ar.ArtistId equals al.ArtistId
                                           group al by ar.ArtistId into grpedAlbums
                                           where grpedAlbums.ToList().Count() == 4
                                           select new
                                           {
                                               ArtistId = grpedAlbums.Key,
                                               Albums = grpedAlbums.ToList()
                                           };

                Console.WriteLine("artists With Four Albums:");
                Console.WriteLine();
                foreach (var a in artistWithFourAlbums)
                {
                    var arName = a.Albums.First().Artist.Name;
                    Console.WriteLine($"{arName}");
                }
            }
        }

        //5. Ilu Customerów pochodzi z Germany?

        private static void AmountOfCustomersFronGermany()
        {
            using (ChinookContext dbContext = new ChinookContext())
            {
                Console.WriteLine("liczba klientów z Niemiec to:");
                Console.WriteLine(dbContext.Customers.Where(c => c.Country == "Germany").Count());
            }
        }


        //6. ktorzy customerzy wydaja najwiecej pieniedzy?
        private static void ListOfValuableCustomers()
        {
            using (ChinookContext dbContext = new ChinookContext())
            {
                var valuableCustomers = from cu in dbContext.Customers
                                        join i in dbContext.Invoices
                                        on cu.CustomerId equals i.CustomerId
                                        group i by cu.CustomerId into grouppeCust
                                        orderby grouppeCust.ToList().Sum(c => c.Total) descending
                                        select new
                                        {
                                            name = grouppeCust.ToList().Select(c => c.Customer.FirstName),
                                            surname = grouppeCust.ToList().Select(c => c.Customer.LastName),
                                            moneySpent = grouppeCust.Sum(c => c.Total)
                                        };
                foreach (var cu in valuableCustomers)
                {
                    Console.WriteLine($"klient {cu.name.First(),-14} {cu.surname.First(),-17} wydał {cu.moneySpent}$");
                }

            }
        }

        //7. Ktore Track sa najczesciej kupowane?
        private static void TracsMostWanted()
        {
            using (ChinookContext dbContext = new ChinookContext())
            {
                var soldTracks = from tr in dbContext.Tracks
                                 join i in dbContext.InvoiceLines
                                 on tr.TrackId equals i.TrackId
                                 group i by tr.TrackId into grpedTracks
                                 orderby grpedTracks.Sum(t => t.Quantity) descending
                                 select new
                                 {
                                     trackName = grpedTracks.ToList().Select(t => t.Track.Name),
                                     qty = grpedTracks.Sum(t => t.Quantity)
                                 };
                foreach (var tra in soldTracks)
                {
                    Console.WriteLine($"Track {tra.trackName.First()} sprzedał się {tra.qty} razy");
                }
            }
        }

        //8. (optional) jakiego Artysty utwory sa najczesciej kupowane w danym kraju?
        private static void BestArtistsPerCountry()
        {
            using (ChinookContext dbContext = new ChinookContext())
            {
                var soldTracks = from tr in dbContext.Tracks
                                 join al in dbContext.Albums
                                 on tr.AlbumId equals al.AlbumId
                                 join ar in dbContext.Artists
                                 on al.ArtistId equals ar.ArtistId
                                 join il in dbContext.InvoiceLines
                                 on tr.TrackId equals il.TrackId
                                 join i in dbContext.Invoices
                                 on il.InvoiceId equals i.InvoiceId
                                 group tr by i.BillingCountry into TracksperCountry
                                 //     orderby TracksperCountry.ToList().Select(t => t.InvoiceLines.Sum(i=>i.Quantity))
                                 select new
                                 {
                                     Country = TracksperCountry.Key,
                                     ArtistName = TracksperCountry.ToList().Select(t => t.Album.Artist.Name),
                                     Tracks = TracksperCountry.ToList()
                                 };

                foreach (var it in soldTracks)
                {
                    var qty = it.Tracks.Select(t => t.InvoiceLines.Sum(i => i.Quantity)).First();
                    Console.WriteLine($" Najlepsze wyniki w {it.Country,-14} ma {it.ArtistName.First(),-14}, który sprzedał tam {qty} swoich utworów");
                }
            }
        }
    }
}
