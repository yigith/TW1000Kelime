using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TW1000Kelime.Properties;

namespace TW1000Kelime
{
    public partial class Form1 : Form
    {
        string[] words;

        public Form1()
        {
            InitializeComponent();
            KelimeleriOku();
            Listele();
            // #4
            tsslKelimeAdet.Text = "Kelime sayısı: " + KelimeAdet();
            // #5
            tsslEnUzunKelime.Text = "En uzun: " + EnUzunKelime();
            // #10e
            lstSayisalSirali.DataSource = SayisalSirali();
        }

        string[] SayisalSirali()
        {
            List<string> sirali = new List<string>();

            foreach (string word in words)
            {
                int deger = SayisalKarsilik(word);

                if (sirali.Count == 0)
                {
                    sirali.Add(word);
                }
                else
                {
                    bool eklendiMi = false;
                    // sıralı insert: baştan sonra sıralı listede
                    // sayısal değerine göre yerini bulma algoritması
                    for (int i = 0; i < sirali.Count; i++)
                    {
                        if (deger < SayisalKarsilik(sirali[i]))
                        {
                            sirali.Insert(i, word);
                            eklendiMi = true;
                            break;
                        }
                    }
                    if (!eklendiMi)
                        sirali.Add(word);
                    
                }
            }
            return sirali.ToArray();
        }

        string[] RuhDorduzuKelimeler(int toplam)
        {
            foreach (string word1 in words)
            {
                int deger1 = SayisalKarsilik(word1);
                if (deger1 >= toplam) goto cikis;
                foreach (string word2 in words)
                {
                    if (word1 == word2) continue;
                    int deger2 = SayisalKarsilik(word2);
                    if (deger1 + deger2 >= toplam) goto cikis;
                    foreach (string word3 in words)
                    {
                        if (word3 == word1 || word3 == word2) continue;
                        int deger3 = SayisalKarsilik(word3);
                        if (deger1 + deger2 + deger3 >= toplam) goto cikis;
                        foreach (string word4 in words)
                        {
                            if (word4 == word1 || word4 == word2 || word4 == word3) continue;
                            int deger4 = SayisalKarsilik(word4);

                            if (deger1 + deger2 + deger3 + deger4 == toplam)
                            {
                                return new string[] { word1, word2, word3, word4 };
                            }
                        }

                    }
                }
            }

        cikis:
            return new string[] { "", "", "" };
        }

        string[] RuhUcuzuKelimeler(int toplam)
        {
            foreach (string word1 in words)
            {
                int deger1 = SayisalKarsilik(word1);
                if (deger1 >= toplam) goto cikis;
                foreach (string word2 in words)
                {
                    if (word1 == word2) continue;
                    int deger2 = SayisalKarsilik(word2);
                    if (deger1 + deger2 >= toplam) goto cikis;
                    foreach (string word3 in words)
                    {
                        if (word3 == word1 || word3 == word2) continue;
                        int deger3 = SayisalKarsilik(word3);

                        if (deger1 + deger2 + deger3 == toplam)
                        {
                            return new string[] { word1, word2, word3 };
                        }

                    }
                }
            }

            cikis:
            return new string[] { "", "", "" };
        }

        // sayisal karşılıkları toplamı toplam değerine eşit olan 2 kelimeyi döndürür. 
        // Haliyle dönen dizi uzunluğu 2 olmalıdır.
        string[] RuhIkiziKelimeler(int toplam)
        {
            foreach (string word1 in words)
            {
                int deger1 = SayisalKarsilik(word1);
                if (deger1 >= toplam)
                    continue;

                foreach (string word2 in words)
                {
                    if (word1 == word2)
                        break;
                    int deger2 = SayisalKarsilik(word2);

                    if (deger1 + deger2 == toplam)
                    {
                        return new string[] { word1, word2 };
                    }

                }
            }

            return new string[] { "", "" };
        }

        string[] Kelimeler(int sayisalKarsilik)
        {
            List<string> sonuc = new List<string>();

            foreach (string word in words)
            {
                if (sayisalKarsilik == SayisalKarsilik(word))
                {
                    sonuc.Add(word);
                }
            }

            return sonuc.ToArray();
        }

        int SayisalKarsilik(string kelime)
        {
            kelime = kelime.ToLower(new CultureInfo("en-US")); // I -> i olarak dönüşmeli
            int sonuc = 0;
            foreach (char harf in kelime)
            {
                if (harf < 'a' || harf > 'z') continue;
                sonuc += harf - 'a' + 1;
            }
            return sonuc;
        }

        // ?o?? -> work, word, your, etc...
        string[] Ara(string patern)
        {
            List<string> sonuc = new List<string>();
            foreach (string word in words)
            {
                if (word.Length != patern.Length)
                    continue; // uzunluk aynı değilse aranan değildir

                bool uyusuyor = true;
                for (int i = 0; i < patern.Length; i++)
                {
                    if (patern[i] == '?' || patern[i] == word[i])
                        continue;
                    uyusuyor = false;
                }

                if (uyusuyor)
                    sonuc.Add(word);
            }
            return sonuc.ToArray();
        }

        string[] KelimelerSadece(params char[] harfler)
        {
            List<string> sonuc = new List<string>();

            foreach (string word in words)
            {
                string temp = word;
                bool hepsiniIceriyor = true;

                foreach (char harf in harfler)
                {
                    if (!word.Contains(harf))
                    {
                        hepsiniIceriyor = false;
                        break;
                    }
                    temp = temp.Replace(harf.ToString(), "");
                }

                // hepsini içeriyorsa ve onlar dışında da içermiyorsa
                if (hepsiniIceriyor && string.IsNullOrWhiteSpace(temp))
                {
                    sonuc.Add(word);
                }

            }

            return sonuc.ToArray();
        }

        string[] KelimelerIcerir(params char[] harfler)
        {
            List<string> sonuc = new List<string>();

            foreach (string word in words)
            {
                bool icerirMi = true;
                foreach (char harf in harfler)
                {
                    if (!word.Contains(harf))
                    {
                        icerirMi = false;
                        break;
                    }
                }
                if (icerirMi)
                    sonuc.Add(word);
            }

            return sonuc.ToArray();
        }

        int KelimeAdet(int harfSayisi)
        {
            int adet = 0;

            foreach (string word in words)
            {
                if (word.Length == harfSayisi)
                {
                    adet++;
                }
            }

            return adet;
        }

        string EnUzunKelime()
        {
            string enUzun = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > enUzun.Length)
                {
                    enUzun = words[i];
                }
            }

            return enUzun;
        }

        int KelimeAdet()
        {
            return words.Length;
        }

        void Listele()
        {
            // lstWords.DataSource = words;

            // diğer yöntem
            lstWords.Items.Clear();
            foreach (string word in words)
            {
                lstWords.Items.Add(word);
            }
        }

        void KelimeleriOku()
        {
            char[] ayraclar = { '\r', '\n' };
            words = Resources.common_words.Split(ayraclar, StringSplitOptions.RemoveEmptyEntries);
        }

        private void nud6HarfAdet_ValueChanged(object sender, EventArgs e)
        {
            nud6KelimeAdet.Value = KelimeAdet((int)nud6HarfAdet.Value);
        }

        private void txt7Harfler_TextChanged(object sender, EventArgs e)
        {
            char[] harfler = txt7Harfler.Text.ToCharArray();
            lst7Sonuc.DataSource = KelimelerIcerir(harfler);
        }

        private void txt8Harfler_TextChanged(object sender, EventArgs e)
        {
            char[] harfler = txt8Harfler.Text.ToCharArray();
            lst8Sonuc.DataSource = KelimelerSadece(harfler);
        }

        private void txtPatern_TextChanged(object sender, EventArgs e)
        {
            string patern = txtPatern.Text;
            lst9Sonuc.DataSource = Ara(patern);
        }

        private void nud10aSayisalKarsilik_ValueChanged(object sender, EventArgs e)
        {
            lst10aSonuc.DataSource = Kelimeler((int)nud10aSayisalKarsilik.Value);
        }

        private void nud10bSayisalKarsilik_ValueChanged(object sender, EventArgs e)
        {
            lst10bSonuc.DataSource = RuhIkiziKelimeler((int)nud10bSayisalKarsilik.Value);
        }

        private void nud10cSayisalKarsilik_ValueChanged(object sender, EventArgs e)
        {
            lst10cSonuc.DataSource = RuhUcuzuKelimeler((int)nud10cSayisalKarsilik.Value);
        }

        private void nud10dSayisalKarsilik_ValueChanged(object sender, EventArgs e)
        {
            lst10dSonuc.DataSource = RuhDorduzuKelimeler((int)nud10dSayisalKarsilik.Value);
        }
    }
}
