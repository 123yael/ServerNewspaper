using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public WordController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("FirstWord")]
        public IActionResult FirstWord()
        {
            string f = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\myFile1.docx";
            string c = "אם יש דלגלג - יציאת המערכת תהיה הזמן הנוכחי ( זה בעצם היציאה הבאה אך דלגלג D מוציא את מה שנכנס אליו קודם) ואת אין לי דלגלג יציאת המערכת יצא הזמן הבא.( מוציא את הזמן הבא שיצא מדלגלגי המעגל).\r\nככל שיש לי כמות יותר גדולה של דלגלגים יש יותר עיכוב של דפיקת שעון.\r\nבחרו דוווקא את דלגלג D כי הוא דלגלג זכרון שמוציא את מה שנכנס אליו בלי לשנות (אבל רק בשנייה הבאה).\r\nבדלגלג D הוא בעצם בזמן הבא יוציא את מה שנכנס אליו בזמן הקודם, כי הוא שומר את מה שנכנס בתוכו !!!\r\n";
            string[] cc = { c, c, c, c, c};
            _funcs.FirstWord(f, cc);
            return Ok("FirstWord is finish");
        }
    }
}
