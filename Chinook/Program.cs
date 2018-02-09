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
            AmountOfArtistsWithExactlyFourAlbums();

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

        //6. ktorzy customerzy wydaja najwiecej pieniedzy?
        //7. Ktore Track sa najczesciej kupowane?
        //8. (optional) jakiego Artysty utwory sa najczesciej kupowane w danym kraju?
        //9. Sprobuj wyciagnac dane o ktore zapytano w zadaniach 2-8 z bazy

    }
}
