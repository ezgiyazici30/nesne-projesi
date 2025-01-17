using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static List<IKullanici> kullanicilar = new List<IKullanici>();
    static List<Gonullu> gonulluler = new List<Gonullu>();
    static List<SosyalSorumlulukProjesi> projeler = new List<SosyalSorumlulukProjesi>();
    static List<YardimTeklifi> yardimTeklifleri = new List<YardimTeklifi>();

    static IKullanici mevcutKullanici = null;

    static void Main(string[] args)
    {
        Console.WriteLine("Küresel İyilik Uygulamasına Hoş Geldiniz!");

        // Başlangıçta bir admin kullanıcı ekleniyor
        kullanicilar.Add(new AdminKullanici { KullaniciAdi = "Admin1", Sifre = "admin123" });

        while (mevcutKullanici == null)
        {
            Console.WriteLine("\n1. Giriş Yap");
            Console.WriteLine("2. Kayıt Ol");
            Console.Write("Seçiminizi yapın: ");
            var secim = Console.ReadLine();

            switch (secim)
            {
                case "1": GirisYap(); break;
                case "2": KayitOl(); break;
                default: Console.WriteLine("Geçersiz seçenek. Tekrar deneyin."); break;
            }
        }

        while (true)
        {
            Console.WriteLine("\n1. Gönüllü Olarak Katıl");
            Console.WriteLine("2. Sosyal Sorumluluk Projelerini Gör");
            Console.WriteLine("3. Yardım Tekliflerini Gör (Yakınlardaki Yardımlar)");
            Console.WriteLine("4. Katıldığınız Projeleri Gör");
            Console.WriteLine("5. Yardım Teklifi Ekle");
            Console.WriteLine("6. Yaptığınız Yardım Taleplerini Gör");
            Console.WriteLine("7. GeriGel");
            Console.WriteLine("8. ProjedenÇık");
            Console.WriteLine("9. Çıkış");



            var secim = Console.ReadLine();

            switch (secim)
            {
                case "1": GonulluOl(); break;
                case "2": ProjeleriGor(); break;
                case "3": YakindakiYardimlariGor(); break;
                case "4": KatildiginizProjeleriGor(); break;
                case "5": YardimTeklifiEkle(); break;
                case "6": YaptiginizYardimTalepleriniGor(); break;
                case "7": GeriGel(); break;
                case "8": ProjedenCik(); break;
                case "9": return;
                default: Console.WriteLine("Geçersiz seçenek. Tekrar deneyin."); break;
            }
        }
    }

    private static void GeriGel()
    {
        static void GeriGel()
        {
            // Önceki menüye geri dönme işlemi yapılabilir
            mevcutKullanici = null;
            Console.WriteLine("Geri dönüldü.");
        }

    }



    static string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(intercept: true); // Karakter ekranda görünmeyecek
            if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
            {
                password += keyInfo.KeyChar;
                Console.Write("*"); // Yıldız basılır
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Karakteri geri al ve sil
            }
        }
        while (keyInfo.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }

    static void GirisYap()
    {
        Console.Write("Kullanıcı Adı: ");
        var kullaniciAdi = Console.ReadLine();
        Console.Write("Şifre: ");
        string sifre = ReadPassword(); // Şifre yıldızlarla görünür

        var kullanici = kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi && k.Sifre == sifre);

        if (kullanici != null)
        {
            mevcutKullanici = kullanici;
        }
        else
        {
            Console.WriteLine("Geçersiz kullanıcı adı veya şifre.");
        }
    }

    static void KayitOl()
    {
        Console.Write("Kullanıcı Adı (En az bir büyük harf ve bir rakam içermeli): ");
        var kullaniciAdi = Console.ReadLine();
        var kullaniciAdiRegex = new Regex(@"^(?=.[A-Z])(?=.\d)");

        if (!kullaniciAdiRegex.IsMatch(kullaniciAdi))
        {
            Console.WriteLine("Kullanıcı adı en az bir büyük harf ve bir rakam içermelidir.");
            return;
        }

        if (kullanicilar.Any(k => k.KullaniciAdi == kullaniciAdi))
        {
            Console.WriteLine("Bu kullanıcı adı zaten alınmış. Lütfen başka bir kullanıcı adı seçin.");
            return;
        }

        Console.Write("Şifre (En az 8 karakter olmalı): ");
        string sifre = ReadPassword(); // Şifre yıldızlarla görünür

        if (sifre.Length < 8)
        {
            Console.WriteLine("Şifre en az 8 karakter olmalıdır.");
            return;
        }

        kullanicilar.Add(new Kullanici { KullaniciAdi = kullaniciAdi, Sifre = sifre });
        Console.WriteLine("Kayıt başarılı! Giriş yapabilirsiniz.");
    }

    static void GonulluOl()
    {
        ProjeleriGor();
        Console.Write("\nKatılmak istediğiniz proje numarasını girin: ");
        if (int.TryParse(Console.ReadLine(), out var projeIndex) && projeIndex >= 1 && projeIndex <= projeler.Count)
        {
            var proje = projeler[projeIndex - 1];
            var gonullu = gonulluler.FirstOrDefault(g => g.Ad == mevcutKullanici.KullaniciAdi) ?? new Gonullu(mevcutKullanici.KullaniciAdi);
            if (!gonulluler.Contains(gonullu)) gonulluler.Add(gonullu);
            gonullu.ProjeEkle(proje);
            Console.WriteLine($"{mevcutKullanici.KullaniciAdi} adlı gönüllü {proje.Ad} projesine katıldı!");
        }
        else
        {
            Console.WriteLine("Geçersiz proje numarası.");
        }
    }

    static void ProjeleriGor()
    {
        Console.WriteLine("\nMevcut Sosyal Sorumluluk Projeleri:");
        if (projeler.Count == 0) Console.WriteLine("Henüz proje bulunmamaktadır.");
        else projeler.ForEach(p => Console.WriteLine($"{projeler.IndexOf(p) + 1}. {p.Ad} - {p.Aciklama}"));
    }

    static void YakindakiYardimlariGor()
    {
        Console.Write("\nYardım yapabileceğiniz bölgeyi girin: ");
        var kullaniciKonum = Console.ReadLine();

        var yakinTeklifler = yardimTeklifleri.Where(t => t.Konum.Contains(kullaniciKonum, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!yakinTeklifler.Any())
            Console.WriteLine("Bu bölgede yardım teklifi bulunmamaktadır.");
        else
            yakinTeklifler.ForEach(t => Console.WriteLine($"{t.YardimTuru}: {t.YardimciAdi} - {t.Konum} - {t.TarihSaat}"));
    }

    static void KatildiginizProjeleriGor()
    {
        var gonullu = gonulluler.FirstOrDefault(g => g.Ad == mevcutKullanici.KullaniciAdi);
        if (gonullu == null || !gonullu.KatildigiProjeler.Any())
        {
            Console.WriteLine("Henüz katıldığınız bir proje yok.");
            return;
        }

        gonullu.KatildigiProjeler.ForEach(p => Console.WriteLine($"{p.Ad} - {p.Aciklama}"));
    }

    static void YardimTeklifiEkle()
    {
        Console.WriteLine("\nYardım Teklifi Ekle");

        Console.Write("Yardım türünü (Gıda, Kıyafet, Eğitim vb.) girin: ");
        var yardimTuru = Console.ReadLine();

        Console.Write("Yardım yapabileceğiniz bölgeyi girin (örn: İstanbul, Kadıköy): ");
        var konum = Console.ReadLine();

        Console.Write("Yardımın yapılacağı tarih ve saat: ");
        var tarihSaat = Console.ReadLine();

        var yardimTeklifi = new YardimTeklifi
        {
            YardimciAdi = mevcutKullanici.KullaniciAdi,
            YardimTuru = yardimTuru,
            Konum = konum,
            TarihSaat = tarihSaat
        };

        yardimTeklifleri.Add(yardimTeklifi);
        Console.WriteLine("Yardım teklifiniz başarıyla eklenmiştir!");
    }

    static void YaptiginizYardimTalepleriniGor()
    {
        var yardimlar = yardimTeklifleri.Where(y => y.YardimciAdi == mevcutKullanici.KullaniciAdi).ToList();
        if (!yardimlar.Any())
        {
            Console.WriteLine("Henüz yaptığınız bir yardım teklifi yok.");
            return;
        }

        Console.WriteLine("\nYaptığınız Yardım Talepleri:");
        yardimlar.ForEach(y => Console.WriteLine($"{y.YardimTuru} - {y.Konum} - {y.TarihSaat}"));
    }
    static void ProjedenCik()
    {
        var gonullu = gonulluler.FirstOrDefault(g => g.Ad == mevcutKullanici.KullaniciAdi);
        if (gonullu == null || !gonullu.KatildigiProjeler.Any())
        {
            Console.WriteLine("Henüz katıldığınız bir proje yok.");
            return;
        }

        Console.WriteLine("Katıldığınız projeler:");
        gonullu.KatildigiProjeler.ForEach(p => Console.WriteLine($"{gonullu.KatildigiProjeler.IndexOf(p) + 1}. {p.Ad} - {p.Aciklama}"));

        Console.Write("\nÇıkmak istediğiniz proje numarasını girin: ");
        if (int.TryParse(Console.ReadLine(), out var projeIndex) && projeIndex >= 1 && projeIndex <= gonullu.KatildigiProjeler.Count)
        {
            var proje = gonullu.KatildigiProjeler[projeIndex - 1];
            gonullu.KatildigiProjeler.Remove(proje);
            Console.WriteLine($"{proje.Ad} projesinden çıkıldı.");
        }
        else
        {
            Console.WriteLine("Geçersiz proje numarası.");
        }
    }

    interface IKullanici  //soyutlama 
    {
        string KullaniciAdi { get; set; }
        string Sifre { get; set; }
        void KullaniciBilgileriniGoster();
    }

    private class Kullanici : IKullanici
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        // Kullanıcı bilgilerini gösterme
        public virtual void KullaniciBilgileriniGoster()
        {
            Console.WriteLine($"Kullanıcı Adı: {KullaniciAdi}");
            Console.WriteLine("Rol: Kullanıcı");
        }
    }

    class AdminKullanici : Kullanici    //kalıtım 
    {
        public override void KullaniciBilgileriniGoster()
        {
            base.KullaniciBilgileriniGoster();
            Console.WriteLine("Rol: Admin");
        }

        public void ProjeEkle(SosyalSorumlulukProjesi proje)
        {
            projeler.Add(proje);
            Console.WriteLine($"Yeni proje eklendi: {proje.Ad}");
        }

    }

    public class Gonullu : IKullanici  //kalıtım 
    {
        public string KullaniciAdi => Ad;
        public string Ad { get; }
        public List<SosyalSorumlulukProjesi> KatildigiProjeler { get; } = new List<SosyalSorumlulukProjesi>();
        string IKullanici.KullaniciAdi { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Sifre { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Gonullu(string ad) => Ad = ad;  // Yapıcı Metod

        public void ProjeEkle(SosyalSorumlulukProjesi proje)
        {
            if (!KatildigiProjeler.Contains(proje))
            {
                KatildigiProjeler.Add(proje);
                Console.WriteLine($"{Ad} adlı gönüllü, {proje.Ad} projesine katıldı.");
            }
            else
            {
                Console.WriteLine("Bu projeye zaten katıldınız.");
            }
        }

        public virtual void KullaniciBilgileriniGoster()
        {
            Console.WriteLine($"Gönüllü Adı: {Ad}");
            Console.WriteLine("Rol: Gönüllü");
        }
    }

    class YardimTeklifi
    {
        public string YardimciAdi { get; set; }
        public string YardimTuru { get; set; }
        public string Konum { get; set; }
        public string TarihSaat { get; set; }
    }

    public class SosyalSorumlulukProjesi
    {
        public string Ad { get; set; }
        public string Aciklama { get; set; }
    }

    static Program()  //yapıcı metod
    {
        projeler.AddRange(new[]
        {
            new SosyalSorumlulukProjesi { Ad = "Çevre Temizliği", Aciklama = "Mahallemizde çevre temizliği yapmak." },
            new SosyalSorumlulukProjesi { Ad = "Yardım Kampanyası", Aciklama = "Kış aylarında ihtiyaç sahiplerine giysi yardımı yapmak." },
            new SosyalSorumlulukProjesi { Ad = "Hayvan Barınağı Ziyareti", Aciklama = "Hayvan barınağındaki dostlarımıza mama vermek." },
            new SosyalSorumlulukProjesi { Ad = "Fidan Ekimi", Aciklama = "Doğal hayat ortamımızı rahatlatmak." },
            new SosyalSorumlulukProjesi { Ad = "Huzurevi", Aciklama = "Büyüklerimizi yanlız bırakmamak." },


        }
    }
}