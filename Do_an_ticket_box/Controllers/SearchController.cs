using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_ticket_box.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public static string RemoveUnicode(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            text = text.ToLower();

            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                   "đ", "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                   "í","ì","ỉ","ĩ","ị", "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                   "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự", "ý","ỳ","ỷ","ỹ","ỵ"};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                   "d", "e","e","e","e","e","e","e","e","e","e","e", "i","i","i","i","i",
                                   "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                   "u","u","u","u","u","u","u","u","u","u","u", "y","y","y","y","y"};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
            }
            return text;
        }

        // GET: Search
        public IActionResult Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Event>());
            }

            
            query = query.Trim();
            while (query.Contains("  "))
            {
                query = query.Replace("  ", " ");
            }
            var queryNormalized = RemoveUnicode(query).ToLower();

            var result = this._context.Events
                .Where(r => r.status != "unvertify") 
                .ToList() 
                .Where(r => (RemoveUnicode(r.Event_Name).ToLower().Contains(queryNormalized) ||
                             RemoveUnicode(r.location).ToLower().Contains(queryNormalized) ||
                             RemoveUnicode(r.description).ToLower().Contains(queryNormalized)))
                .ToList(); 

            return View(result);
        }




    }
}
