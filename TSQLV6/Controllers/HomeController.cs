using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TSQLV6.Models;

namespace TSQLV6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var headerItem = new HeaderItem
            {
                Title = "Rekrutacja trwa! Nauka w WIT to coś więcej niż tylko wykłady. Zapraszamy na Dzień otwarty - Poznajmy się!",
                Description = "Rozwijaj profesjonalne kompetencje.",
                ImageUrl = "https://static.wit.edu.pl/rekrutacja/Akademia-WIT-Rozwijaj-profesjonalne-kompetencje-w.png",
                Link = "https://www.wit.edu.pl/2024//rekrutacja",
                BackgroundColor = "#B9121C"
            };

            var newsItems = new List<NewsItem>
            {
                new NewsItem
                {
                    Title = "Rekrutacja trwa! Nauka w WIT to coś więcej niż tylko wykłady. Zapraszamy na Dzień otwarty - Poznajmy się!",
                    Date = "20/05/2024",
                    Description = "Zdobądź z Akademią WIT nowe kompetencje i rozwijaj swoją ścieżkę zawodową w oparciu o wybraną specjalność! Wybierz studia oparte o praktykę, dopasowane do Twoich potrzeb kierunki - ws...",
                    ImageUrl = "https://static.wit.edu.pl/images/news/2072/Rekrutacja-Akademia-WIT-2024-2025-thumb.jpg",
                    Link = "https://www.wit.edu.pl/2024//2024/rekrutacja-na-studia-w-akademii-wit",
                    LinkText = "Wybierz studia oparte o praktykę"
                },
                new NewsItem
                {
                    Title = "Rysowniczki i rysownicy - wystawa z Pracowni Rysunku dr Izabeli Łęskiej / Fotorelacja",
                    Date = "18/06/2024",
                    Description = "Przegląd prac studentów II roku Grafiki, wśród których znajdują się prace studyjne z modelem oraz propozycje własne studentów na zadane tematy z wykorzystaniem mniej bądź bardziej kla...",
                    ImageUrl = "https://static.wit.edu.pl/images/news/2082/Rysowniczki i rysownicy-thumb.jpg",
                    Link = "https://www.wit.edu.pl/2024//2024/rysowniczki-i-rysownicy",
                    LinkText = "Czytaj więcej"
                },
                new NewsItem
                {
                    Title = "Studencki magazyn Fenomen – trzecia edycja",
                    Date = "11/06/2024",
                    Description = "Mamy przyjemność ogłosić wydanie trzeciej edycji studenckiego magazynu Fenomen!",
                    ImageUrl = "https://static.wit.edu.pl/images/news/2080/Studencki magazyn Fenomen-trzecia edycja-thumb-N.jpg",
                    Link = "https://www.wit.edu.pl/2024//2024/studencki-magazyn-fenomen-trzecia-edycja",
                    LinkText = "Czytaj więcej"
                },
                new NewsItem
                {
                    Title = "Cykl szkoleń autorskich Moniki Mularskiej-Kucharek",
                    Date = "19/03/2024",
                    Description = "Serdecznie zapraszamy na cykl autorskich szkoleń doktor nauk humanistycznych Moniki Mularskiej-Kucharek. Wszystkie szkolenia są bezpłatne i będą prowadzone w formie ONLINE poprzez Mic...",
                    ImageUrl = "https://static.wit.edu.pl/images/news/2023/CDK-Cykl-szkoleń-autorskich-Moniki-Mularskiej-Kucharek-thumb-N.jpg",
                    Link = "https://www.wit.edu.pl/2024//2024/cykl-szkolen-autorskich-moniki-mularskiej-kucharek",
                    LinkText = "Weź udział i zdobądź nowe umiejętności"
                },
                new NewsItem
                {
                    Title = "Budowanie kompetencji przyszłości - Cykl inspirujących wykładów z Zarządzania",
                    Date = "20/02/2024",
                    Description = "Zapraszamy na cykl inspirujących wykładów z Zarządzania, które będą prowadzone przez nauczycieli akademickich pracujących na kierunku Zarządzanie.",
                    ImageUrl = "https://static.wit.edu.pl/images/news/1997/CDK-budowanie kompetencji przyszłości-thumb-N.jpg",
                    Link = "https://www.wit.edu.pl/2024//2024/budowanie-kompetencji-przyszlosci-cykl-inspirujacych-wykladow-z-zarzadzania",
                    LinkText = "Weź udział i zdobądź kompetencje przyszłości"
                },
                new NewsItem
                {
                    Title = "Future Skills - szkolenia dla dorosłych w erze cyfrowej",
                    Date = "12/12/2023",
                    Description = "Zapraszamy do lektury naszego raportu o kluczowych kompetencjach na przyszłość w erze Rewolucji Przemysłowej 4.0.",
                    ImageUrl = "https://static.wit.edu.pl/images/news/1955/Rewolucja-Przemysłowa-4-0-WIT-thumb-N.jpg",
                    Link = "https://www.wit.edu.pl/2024//2023/raport-badanie-potrzeb",
                    LinkText = "Odkryj kompetencje przyszłości"
                }
            };

            var exhibitions = new List<ExhibitionItem>
        {
            new ExhibitionItem
            {
                Title = "Rysowniczki i rysownicy - wystawa z pracowni rysunku dr Izabeli Łęskiej",
                Subtitle = "",
                Gallery = "Galeria WIT",
                DateRange = "4.06 - 4.07.2024",
                ImageUrl = "https://www.wit.edu.pl/images/wystawy/Rysowniczki-i-rysownicy-Wystawa-z-pracowni-rysunku.jpg",
                Link = "https://www.wit.edu.pl/wystawy/2024/rysowniczki-i-rysownicy-wystawa-z-pracowni-rysunku"
            },
            new ExhibitionItem
            {
                Title = "Grzegorz Pabel",
                Subtitle = "Malarstwo",
                Gallery = "Galeria Opera - Teatr Narodowy",
                DateRange = "25.04 - 30.06.2024",
                ImageUrl = "https://www.wit.edu.pl/images/wystawy/Grzegorz-Pabel-Teatr-Wielki-2024.jpg",
                Link = "https://www.wit.edu.pl/wystawy/2024/grzegorz-pabel-teatr-wielki"
            },
            new ExhibitionItem
            {
                Title = "Mieczysław Wasilewski",
                Subtitle = "Rysunek i plakat",
                Gallery = "Galeria Retroavangarda",
                DateRange = "3.06 - 15.09.2024",
                ImageUrl = "https://www.wit.edu.pl/images/wystawy/Mieczyslaw-Wasilewski-Retroavangarda.jpg",
                Link = "https://www.wit.edu.pl/wystawy/2024/mieczyslaw-wasilewski-rysunek-i-plakat"
            },
            new ExhibitionItem
            {
                Title = "Upcycling",
                Subtitle = "",
                Gallery = "Galeria Stara Prochownia SCEK",
                DateRange = "17.05 - 23.06.2024",
                ImageUrl = "https://www.wit.edu.pl/images/wystawy/Upcycling-czyli-drugie-życie.jpg",
                Link = "https://www.wit.edu.pl/wystawy/2024/upcycling-czyli-drugie-zycie"
            }
        };

            ViewBag.Exhibitions = exhibitions;
            ViewBag.NewsItems = newsItems;
            ViewBag.HeaderItem = headerItem;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
