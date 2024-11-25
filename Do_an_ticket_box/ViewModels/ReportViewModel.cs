using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Do_an_ticket_box.ViewModels
{
    public class ReportViewModel
    {
        public int? Event_ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá.")]
        public string Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Đánh giá phải nằm trong khoảng từ 1 đến 5.")]
        public int Rate { get; set; }

        public List<SelectListItem> EventList { get; set; } = new List<SelectListItem>();
    }
}
