using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
        public enum Sunlight
        {
            None,
            Shade,
            MediumShade,
            FullSun
        }
        public enum LifeCycle
        {
            None,
            Annual,
            Biennial,
            Perennial
        }
        public enum Water
        {
            None,
            Low,
            Medium, 
            High
        }
        public enum Align 
        { 
            Left, 
            Center, 
            Right 
        }
        public enum Role //För att framtidssäkra och visa varförr jag valt enum och inte bool
        {
            Customer,
            Admin,
            WarehouseStaff,
            CustomerSupport
        }
        public enum AdminMenu
        {
            Kategorihantering = '1',
            Produkthantering,
            Kundhantering,
            OnDisplayIsOnPage,
            Statistik,
            Lagerstatus,
            Utloggning
        }

}
